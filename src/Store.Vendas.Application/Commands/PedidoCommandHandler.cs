using MediatR;
using Store.Core.Communication.Mediator;
using Store.Core.DomainObjects.DTO;
using Store.Core.Extensions;
using Store.Core.Messages;
using Store.Core.Messages.Common.IntegrationEvents;
using Store.Core.Messages.Common.Notifications;
using Store.Vendas.Application.Events;
using Store.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Vendas.Application.Commands
{
    public class PedidoCommandHandler : 
            IRequestHandler<AdicionarItemPedidoCommand, bool>,
            IRequestHandler<AtualizarItemPedidoCommand, bool>,
            IRequestHandler<RemoverItemPedidoCommand, bool>,
            IRequestHandler<AplicarVoucherPedidoCommand, bool>,
            IRequestHandler<IniciarPedidoCommand, bool>,
            IRequestHandler<FinalizarPedidoCommand, bool>,
            IRequestHandler<CancelarProcessamentoPedidoEstornarEstoque, bool>,
            IRequestHandler<CancelarProcessamentoPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediatRHandler _mediatorHandler;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediatRHandler mediatorHandler)
        {
            _pedidoRepository = pedidoRepository;
            _mediatorHandler = mediatorHandler;

        }
        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarCommand(message)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
            var pedidoItem = new PedidoItem(message.ProdutoId, message.ProdutoNome, message.Quantidade, message.ValorUnitario);

            if (pedido == null)
            {
                pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
                pedido.AdicionarItem(pedidoItem);
                _pedidoRepository.Adicionar(pedido);
                pedido.AdicionarEvento(new PedidoRascunhoIniciadoEvent(message.ClienteId, pedido.Id));

            }
            else
            {
                var pedidoExistente = pedido.PedidoItemExiste(pedidoItem);

                pedido.AdicionarItem(pedidoItem);

                if (pedidoExistente)
                {
                    _pedidoRepository.AtualizarItem(pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId));
                    
                }
                else
                {
                    _pedidoRepository.AdicionarItem(pedidoItem);
                }

                _pedidoRepository.Atualizar(pedido);

                pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            }

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id,
                    message.ProdutoId, message.ValorUnitario, message.Quantidade));
            
            return await _pedidoRepository.UnitOfwork.Commit();
        }

        public async Task<bool> Handle(AtualizarItemPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarCommand(request)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(request.ClienteId);

            if (pedido == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            var pedidoItem = await _pedidoRepository.ObterItemPorId(request.ProdutoId);

            if (pedidoItem == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedidoItem", "Item do pedido não encontrado"));
                return false;
            }

            pedido.AtualizarUnidades(pedidoItem, request.Quantidade);

            pedido.AdicionarEvento(new PedidoProdutoAtualizadoEvent(pedido.ClienteId, pedido.Id, request.ProdutoId, request.Quantidade));

            _pedidoRepository.AtualizarItem(pedidoItem);
            _pedidoRepository.Atualizar(pedido);            

            return await _pedidoRepository.UnitOfwork.Commit();

        }

        public async Task<bool> Handle(RemoverItemPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarCommand(request)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(request.ClienteId);

            if (pedido == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            var pedidoItem = await _pedidoRepository.ObterItemPorId(request.ProdutoId);

            if (pedidoItem == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedidoItem", "Item do pedido não encontrado"));
                return false;
            }

            pedido.RemoverItem(pedidoItem);

            pedido.AdicionarEvento(new PedidoProdutoRemovidoEvent(pedido.ClienteId, pedido.Id, request.ProdutoId));

            _pedidoRepository.RemoverItem(pedidoItem);
            _pedidoRepository.Atualizar(pedido);

            return await _pedidoRepository.UnitOfwork.Commit();
        }

        public async Task<bool> Handle(AplicarVoucherPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarCommand(request)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(request.ClienteId);

            if (pedido == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            var voucher = await _pedidoRepository.ObterVoucherPorCodigo(request.VoucherCodigo);

            if (voucher == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Voucher não encontrado"));
                return false;
            }

            var voucherValidation = pedido.AplicarVoucher(voucher);

            if (!voucherValidation.IsValid)
            {
                foreach (var error in voucherValidation.Errors)
                {
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(error.ErrorCode, error.ErrorMessage));
                }

                return false;
            }

            pedido.AdicionarEvento(new VoucherAplicadoPedidoEvent(pedido.ClienteId, pedido.Id, request.VoucherCodigo));
            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));

            _pedidoRepository.Atualizar(pedido);

            return await _pedidoRepository.UnitOfwork.Commit();
        }

        public async Task<bool> Handle(IniciarPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarCommand(request)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(request.ClienteId);

            if (pedido == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            pedido.IniciarPedido();

            var listItem = new List<Item>();

            foreach (var item in pedido.PedidoItems)
            {
                listItem.Add(new Item { Id = item.ProdutoId, Quantidade = item.Quantidade });
            }

            // pedido.PedidoItems.ForEach(i => listItem.Add(new Item { Id = i.ProdutoId, Quantidade = i.Quantidade }));

            var listaProdutosPedido = new ListaProdutosPedido { PedidoId = pedido.Id, Itens = listItem };

            pedido.AdicionarEvento(new PedidoIniciadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal,
                request.NomeCartao, request.NumeroCartao, request.ExpiracaoCartao, request.CvvCartao, listaProdutosPedido));

            _pedidoRepository.Atualizar(pedido);

            return await _pedidoRepository.UnitOfwork.Commit();
        }

        public async Task<bool> Handle(FinalizarPedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.ObterPedidoPorId(request.PedidoId);

            if(pedido == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            pedido.FinalizarPedido();
            pedido.AdicionarEvento(new PedidoFinalizadoEvent(request.PedidoId));
            return await _pedidoRepository.UnitOfwork.Commit();
        }

        public async Task<bool> Handle(CancelarProcessamentoPedidoEstornarEstoque request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.ObterPedidoPorId(request.PedidoId);

            if (pedido == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            var itensPedido = new List<Item>();
            pedido.PedidoItems.ForEach(i => itensPedido.Add(new Item { Id = i.ProdutoId, Quantidade = i.Quantidade }));
            var listaProdutosPedido = new ListaProdutosPedido { PedidoId = pedido.Id, Itens = itensPedido };

            pedido.AdicionarEvento(new PedidoProcessamentoCanceladoEvent(pedido.Id, pedido.ClienteId, listaProdutosPedido));
            pedido.TornarRascunho();

            return await _pedidoRepository.UnitOfwork.Commit();
        }

        public async Task<bool> Handle(CancelarProcessamentoPedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.ObterPedidoPorId(request.PedidoId);

            if (pedido == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            pedido.TornarRascunho();
            return await _pedidoRepository.UnitOfwork.Commit();
        }       

        private bool ValidarCommand(Command message)
        {
            if (message.IsValido())
            {
                return true;
            }

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}
