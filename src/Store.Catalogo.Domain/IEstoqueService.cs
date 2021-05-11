using Store.Core.DomainObjects.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Store.Catalogo.Domain
{
    /* Service Domain */
    public interface IEstoqueService : IDisposable
    {
        Task<bool> DebitarEstoque(Guid produtoId, int quantidade);
        Task<bool> DebitarEstoqueListaProdutos(ListaProdutosPedido produtosPedido);
        Task<bool> ReporEstoque(Guid produtoId, int quantidade);
        Task<bool> ReporEstoqueListaProdutos(ListaProdutosPedido produtosPedido);
    }
}
