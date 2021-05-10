using Store.Vendas.Application.Queries.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Store.Vendas.Application.Queries
{
    /* Query Facade */
    public interface IPedidoQueries
    {
        Task<CarrinhoDTO> ObterCarrinhoCliente(Guid clienteId);
        Task<IEnumerable<PedidoDTO>> ObterPedidosCliente(Guid clienteId);
    }
}
