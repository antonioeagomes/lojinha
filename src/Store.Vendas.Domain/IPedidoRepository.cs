using Store.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.Vendas.Domain
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido> ObterPedidoPorId(Guid id);

        Task<IEnumerable<Pedido>> ObterPedidosPorClienteId(Guid clienteId);

        Task<Pedido> ObterPedidoRascunhoPorClienteId(Guid clienteId);

        void Adicionar(Pedido pedido);

        void Atualizar(Pedido pedido);

        Task<PedidoItem> ObterItemPorId(Guid id);
        Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId);

        void AdicionarItem(PedidoItem pedidoItem);
        void AtualizarItem(PedidoItem pedidoItem);
        void RemoverItem(PedidoItem pedidoItem);

        Task<Voucher> ObterVoucherPorCodigo(string codigo);
    }
}
