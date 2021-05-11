using Store.Catalogo.Domain.Events;
using Store.Core.Communication.Mediator;
using Store.Core.DomainObjects.DTO;
using Store.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Store.Catalogo.Domain
{
    /*
     * Cross aggregate
     * Quando a regra de negócio não cabe na aolicação nem na entidade
     */
    public class EstoqueService : IEstoqueService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMediatRHandler _mediatorHandler;

        public EstoqueService(IProdutoRepository produtoRepository, IMediatRHandler mediatorHandler)
        {
            _produtoRepository = produtoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
        {
            var sucesso = await DebitarItemEstoque(produtoId, quantidade);

            if (!sucesso) return false;

            return await _produtoRepository.UnitOfwork.Commit();
        }

        public async Task<bool> DebitarEstoqueListaProdutos(ListaProdutosPedido produtosPedido)
        {
            foreach (var item in produtosPedido.Itens)
            {
                if (!await DebitarItemEstoque(item.Id, item.Quantidade)) return false;
            }

            return await _produtoRepository.UnitOfwork.Commit();
        }        

        private async Task<bool> DebitarItemEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorId(produtoId);

            if (produto == null) return false;

            if (!produto.PossuiEstoque(quantidade)) return false;

            produto.DebitarEstoque(quantidade);

            if (produto.QuantidadeEstoque < 10)
            {
                await _mediatorHandler.PublicarEvento(new ProdutoAbaixoEstoqueEvent(produto.Id, produto.QuantidadeEstoque));
            }

            _produtoRepository.Atualizar(produto);
            return true;
        }

        public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
        {
            var sucesso = await ReporItemEstoque(produtoId, quantidade);

            if (!sucesso) return false;

            return await _produtoRepository.UnitOfwork.Commit();
        }

        public async Task<bool> ReporEstoqueListaProdutos(ListaProdutosPedido produtosPedido)
        {
            foreach (var item in produtosPedido.Itens)
            {
                if (!await ReporItemEstoque(item.Id, item.Quantidade)) return false;
            }

            return await _produtoRepository.UnitOfwork.Commit();
        }

        private async Task<bool> ReporItemEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorId(produtoId);

            if (produto == null) return false;

            produto.ReporEstoque(quantidade);

            _produtoRepository.Atualizar(produto);

            return true;
        }

        public void Dispose()
        {
            _produtoRepository.Dispose();
        }
    }
}
