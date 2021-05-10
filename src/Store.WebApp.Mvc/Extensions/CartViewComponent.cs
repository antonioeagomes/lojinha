using Microsoft.AspNetCore.Mvc;
using Store.Vendas.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.WebApp.Mvc.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IPedidoQueries _pedidoQueries;

        protected Guid ClientId = Guid.Parse("d5622e45-bda1-4de4-b4c4-c5554b37f8c8");

        public CartViewComponent(IPedidoQueries pedidoQueries)
        {
            _pedidoQueries = pedidoQueries;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var carrinho = await _pedidoQueries.ObterCarrinhoCliente(ClientId);
            var itens = carrinho?.Itens.Count ?? 0;

            return View(itens);
        }
    }
}
