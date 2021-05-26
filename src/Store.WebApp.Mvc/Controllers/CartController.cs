using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.Catalogo.Application.Services;
using Store.Core.Communication.Mediator;
using Store.Core.Messages.Common.Notifications;
using Store.Vendas.Application.Commands;
using Store.Vendas.Application.Queries;
using Store.Vendas.Application.Queries.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.WebApp.Mvc.Controllers
{
    public class CartController : ControllerBase
    {
        private readonly INotificationHandler<DomainNotification> _notifications;
        private readonly IProdutoAppService _produtoAppService;
        private readonly IMediatRHandler _mediatRHandler;
        private readonly IPedidoQueries _pedidoQueries;

        public CartController(INotificationHandler<DomainNotification> notifications, 
                              IProdutoAppService produtoAppService, 
                              IMediatRHandler mediatRHandler,
                              IPedidoQueries pedidoQueries) : base(notifications, mediatRHandler)
        {
            _notifications = notifications;
            _produtoAppService = produtoAppService;
            _mediatRHandler = mediatRHandler;
            _pedidoQueries = pedidoQueries;
        }

        [Route("meu-carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("meu-carrinho")]
        public async Task<IActionResult> AdicionarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null) return NotFound();

            if(produto.QuantidadeEstoque < quantidade)
            {
                TempData["Erro"] = "Estoque insuficiente";
                return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
            }

            var command = new AdicionarItemPedidoCommand(ClienteId, produto.Id, produto.Nome, quantidade, produto.Valor);
            await _mediatRHandler.EnviarComando(command);

            if (IsOperacaoValida())
            {
                return RedirectToAction("Index");
            }

            TempData["Erros"] = ObterMensagensErro();
            return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
        }

        [HttpPost]
        [Route("remover-item")]
        public async Task<IActionResult> RemoverItem(Guid id)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null) return BadRequest();
            var command = new RemoverItemPedidoCommand(ClienteId, id);

            await _mediatRHandler.EnviarComando(command);

            if (IsOperacaoValida())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("atualizar-item")]
        public async Task<IActionResult> AtualizarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null) return BadRequest();
            var command = new AtualizarItemPedidoCommand(ClienteId, id, quantidade);

            await _mediatRHandler.EnviarComando(command);

            if (IsOperacaoValida())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        {
            var command = new AplicarVoucherPedidoCommand(ClienteId, voucherCodigo);

            await _mediatRHandler.EnviarComando(command);

            if (IsOperacaoValida())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [Route("resumo-compra")]
        public async Task<IActionResult> ResumoCompra()
        {
            return View(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("iniciar-pedido")]
        public async Task<IActionResult> IniciarPedido(CarrinhoDTO carrinhoDTO)
        {
            var cart = await _pedidoQueries.ObterCarrinhoCliente(ClienteId);

            var command = new IniciarPedidoCommand(ClienteId, cart.PedidoId, cart.ValorTotal, cart.Pagamento.NomeCartao,
                cart.Pagamento.NumeroCartao, cart.Pagamento.ExpiracaoCartao, cart.Pagamento.CvvCartao);

            await _mediatRHandler.EnviarComando(command);

            if (IsOperacaoValida())
            {
                return RedirectToAction("Index", "Pedido");
            }

            return View("ResumoCompra", cart);
        }

    }
}
