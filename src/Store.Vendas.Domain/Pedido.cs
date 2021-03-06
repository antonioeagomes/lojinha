using FluentValidation.Results;
using Store.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Vendas.Domain
{
    /* Objetos de Domínio */
    public class Pedido : Entity, IAggregateRoot
    {
        public static int MIN_UNIDADES_ITEM => 1;
        public static int MAX_UNIDADES_ITEM => 15;
        public int Codigo { get; private set; }
        public Guid ClienteId { get; private set; }
        public Guid? VoucherId { get; set; }
        public bool VoucherUtilizado { get; set; }
        public decimal Desconto { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }

        /* Evitar acesso externo */
        private readonly List<PedidoItem> _pedidoItems;

        /* Propriedade publica recebe os itens da lista interna 
         * Queremos evitar que qualquer externo adicione um "PedidoItems.Add()"
         */
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        public virtual Voucher Voucher { get; private set; }

        public Pedido(Guid clienteId, bool voucherUtilizado, decimal desconto, decimal valorTotal)
        {
            ClienteId = clienteId;
            VoucherUtilizado = voucherUtilizado;
            Desconto = desconto;
            ValorTotal = valorTotal;
            _pedidoItems = new List<PedidoItem>();
        }

        protected Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }

        public void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;
            var valor = ValorTotal;

            if (Voucher.TipoDescontoVoucher == TipoDescontoVoucher.Porcentagem)
            {
                if (Voucher.Percentual.HasValue)
                {
                    desconto = (valor * Voucher.Percentual.Value) / 100;
                    valor -= desconto;
                }
            }
            else
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;
                    valor -= desconto;
                }
            }

            ValorTotal = valor <= 0 ? 0.01m : valor;
            Desconto = desconto;
        }

        public void CalcularValorPedido()
        {
            ValorTotal = PedidoItems.Sum(p => p.CalcularValor());
            CalcularValorTotalDesconto();
        }

        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var validation = voucher.ValidarAplicabilidade();

            if (!validation.IsValid) return validation;

            Voucher = voucher;
            VoucherId = voucher.Id;
            VoucherUtilizado = true;
            CalcularValorPedido();

            return validation;
        }

        public bool PedidoItemExiste(PedidoItem item)
        {
            return _pedidoItems.Any(p => p.ProdutoId == item.ProdutoId);
        }

        public void AdicionarItem(PedidoItem item)
        {
            if (!item.IsValido()) return;

            ValidarQuantidadeItemPermitida(item);

            item.AssociarPedido(Id);

            if (PedidoItemExiste(item))
            {
                var itemExistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
                itemExistente.AdicionarUnidades(item.Quantidade);

                item = itemExistente;

                _pedidoItems.Remove(itemExistente);
            }

            item.CalcularValor();
            _pedidoItems.Add(item);

            CalcularValorPedido();
        }

        private void ValidarQuantidadeItemPermitida(PedidoItem item)
        {
            var quantidadeItems = item.Quantidade;

            if(PedidoItemExiste(item))
            {
                var itemExistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
                quantidadeItems += itemExistente.Quantidade;
            }

            if(quantidadeItems > MAX_UNIDADES_ITEM) throw new DomainException($"Máximo de {Pedido.MAX_UNIDADES_ITEM} unidades por produto");
        }

        public void RemoverItem(PedidoItem item)
        {
            if (!item.IsValido()) return;

            ValidarItemExistente(item);
            
            _pedidoItems.Remove(item);

            CalcularValorPedido();
        }

        public void AtualizarItem(PedidoItem item)
        {
            if (!item.IsValido()) return;

            ValidarQuantidadeItemPermitida(item);
            ValidarItemExistente(item);
            
            item.AssociarPedido(Id);

            var itemExistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
            
            _pedidoItems.Remove(itemExistente);
            _pedidoItems.Add(item);

            CalcularValorPedido();
        }

        public void ValidarItemExistente(PedidoItem item)
        {
            if (!PedidoItemExiste(item)) throw new DomainException("Item não pertence ao pedido");
        }

        public void AtualizarUnidades(PedidoItem item, int unidade)
        {
            item.AtualizarUnidades(unidade);
            AtualizarItem(item);
        }

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }

        public void IniciarPedido()
        {
            PedidoStatus = PedidoStatus.Iniciado;
        }

        public void FinalizarPedido()
        {
            PedidoStatus = PedidoStatus.Pago;
        }

        public void CancelarPedido()
        {
            PedidoStatus = PedidoStatus.Cancelado;
        }

        /* Nested class */
        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteId,
                };
                pedido.TornarRascunho();
                return pedido;
            }
        }

    }
}
