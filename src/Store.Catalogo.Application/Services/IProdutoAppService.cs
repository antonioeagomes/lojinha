using Store.Catalogo.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Store.Catalogo.Application.Services
{
    public interface IProdutoAppService : IDisposable
    {
        Task<IEnumerable<ProdutoDto>> ObterTodos();
        Task<ProdutoDto> ObterPorId(Guid id);
        Task<IEnumerable<ProdutoDto>> ObterPorCategoria(int codigo);
        Task<IEnumerable<CategoriaDto>> ObterCategorias();
        Task Adicionar(ProdutoDto produto);
        Task Atualizar(ProdutoDto produto);
        Task<ProdutoDto> DebitarEstoque(Guid produtoId, int quantidade);
        Task<ProdutoDto> ReporEstoque(Guid produtoId, int quantidade);
    }
}
