using AutoMapper;
using Store.Catalogo.Application.Dtos;
using Store.Catalogo.Domain;
using Store.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Store.Catalogo.Application.Services
{
    public class ProdutoAppService : IProdutoAppService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMapper _mapper;

        public ProdutoAppService(IProdutoRepository produtoRepository, IEstoqueService estoqueService, IMapper mapper)
        {
            this._produtoRepository = produtoRepository;
            this._mapper = mapper;
            _estoqueService = estoqueService;
        }


        public async Task Adicionar(ProdutoDto produto)
        {
            await _produtoRepository.Adicionar(_mapper.Map<Produto>(produto));
            await _produtoRepository.UnitOfwork.Commit();
        }

        public async Task Atualizar(ProdutoDto produto)
        {
            _produtoRepository.Atualizar(_mapper.Map<Produto>(produto));
            await _produtoRepository.UnitOfwork.Commit();
        }

        public async Task<ProdutoDto> DebitarEstoque(Guid produtoId, int quantidade)
        {
            if (!_estoqueService.DebitarEstoque(produtoId, quantidade).Result)
            {
                throw new DomainException("Falha ao debitar estoque");
            }

            return _mapper.Map<ProdutoDto>(await _produtoRepository.ObterPorId(produtoId));
        }        

        public async Task<IEnumerable<CategoriaDto>> ObterCategorias()
        {
            return _mapper.Map<IEnumerable<CategoriaDto>>(await _produtoRepository.ObterCategorias());
        }

        public async Task<IEnumerable<ProdutoDto>> ObterPorCategoria(int codigo)
        {
            return _mapper.Map<IEnumerable<ProdutoDto>>(await _produtoRepository.ObterPorCategoria(codigo));
        }

        public async Task<ProdutoDto> ObterPorId(Guid id)
        {
            return _mapper.Map<ProdutoDto>(await _produtoRepository.ObterPorId(id));
        }

        public async Task<IEnumerable<ProdutoDto>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoDto>>(await _produtoRepository.ObterTodos());
        }

        public async Task<ProdutoDto> ReporEstoque(Guid produtoId, int quantidade)
        {
            if (!_estoqueService.ReporEstoque(produtoId, quantidade).Result)
            {
                throw new DomainException("Falha ao debitar estoque");
            }

            return _mapper.Map<ProdutoDto>(await _produtoRepository.ObterPorId(produtoId));
        }
        public void Dispose()
        {
            _produtoRepository?.Dispose();
            _estoqueService?.Dispose();
        }
    }
}
