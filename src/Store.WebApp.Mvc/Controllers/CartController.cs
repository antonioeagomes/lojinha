using Microsoft.AspNetCore.Mvc;
using Store.Catalogo.Application.Services;
using Store.Core.Bus;
using Store.Vendas.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.WebApp.Mvc.Controllers
{
    public class CartController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IMediatRHandler _mediatRHandler;

        public CartController(IProdutoAppService produtoAppService, IMediatRHandler mediatRHandler)
        {
            _produtoAppService = produtoAppService;
            _mediatRHandler = mediatRHandler;
        }
        public IActionResult Index()
        {
            return View();
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
                return RedirectToAction("ProdutoDetalhe", "VitrineController", new { id });
            }

            var command = new AdicionarItemPedidoCommand(ClienteId, produto.Id, produto.Nome,quantidade,produto.Valor);
            await _mediatRHandler.EnviarComando(command);

            TempData["Erro"] = "Estoque insuficiente";
            return RedirectToAction("ProdutoDetalhe", "VitrineController", new { id });
        }

    }
}
