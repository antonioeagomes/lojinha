using Store.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Store.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar Item Novo Pedido Vazio")]
        [Trait("Vendas", "Testes de Pedido")]
        public void AdicionarItemPedido_NovoPedido_DeveCriarUmPedidoNovo()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Nome do produto", 2, 50);

            // Act
            pedido.AdicionarItem(pedidoItem);

            //Assert
            Assert.Equal(100, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente")]
        [Trait("Vendas", "Testes de Pedido")]
        public void AdicionarItemPedido_PedidoExistente_DeveAtualizarValor()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Nome do produto", 2, 50);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Nome do segundo produto", 1, 50);

            // Act
            pedido.AdicionarItem(pedidoItem2);


            //Assert
            Assert.Equal(150, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Atualizar Quantidade Item Pedido Existente")]
        [Trait("Vendas", "Testes de Pedido")]
        public void AdicionarItemPedido_PedidoExistente_DeveAtualizarQuantidadeEValor()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Nome do produto", 2, 50);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItem2 = new PedidoItem(produtoId, "Nome do produto", 1, 50);

            // Act
            pedido.AdicionarItem(pedidoItem2);

            //Assert
            Assert.Equal(150, pedido.ValorTotal);
            Assert.Equal(1, pedido.PedidoItems.Count);
            Assert.Equal(3, pedido.PedidoItems.FirstOrDefault()?.Quantidade);
        }

        [Fact(DisplayName = "Adicionar Item Novo Pedido Acima do permitido")]
        [Trait("Vendas", "Testes de Pedido")]
        public void AdicionarItemPedido_ItemAcimaDoPermitido_DeveRetornarUmaException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto excedendo estoque", Pedido.MAX_UNIDADES_ITEM + 1, 50);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem));
            
        }

        [Fact(DisplayName = "Adicionar Item Existente Acima do permitido")]
        [Trait("Vendas", "Testes de Pedido")]
        public void AdicionarItemPedido_ItemExistenteAcimaDoPermitido_DeveRetornarUmaException()
        {
            //Arrange
            var produtoId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(produtoId, "Produto excedendo estoque", Pedido.MAX_UNIDADES_ITEM, 50);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItem2 = new PedidoItem(produtoId, "Produto excedendo estoque", 1, 50);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));
        }
    }
}
