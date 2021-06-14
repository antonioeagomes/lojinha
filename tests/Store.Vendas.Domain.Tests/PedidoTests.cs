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

        [Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
        [Trait("Vendas", "Testes de Pedido")]
        public void AtualizaItemPedido_ItemInexistente_DeveRetornarUmaException()
        {
            //Arrange
            var produtoId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(produtoId, "Produto", 1, 50);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItem));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Válido")]
        [Trait("Vendas", "Testes de Pedido")]
        public void AtualizaItemPedido_ItemValido_DeveAtualizarAQuantidade()
        {
            //Arrange
            var produtoId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(produtoId, "Produto Válido", 2, 50);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItem2 = new PedidoItem(produtoId, "Produto Válido", 7, 50);
            var novaQuantidade = pedidoItem2.Quantidade;
            //Act
            pedido.AtualizarItem(pedidoItem2);
            //Assert
            Assert.Equal(novaQuantidade, pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId)?.Quantidade);

        }

        [Fact(DisplayName = "Atualizar Valor Total Pedido")]
        [Trait("Vendas", "Testes de Pedido")]
        public void AtualizaItemPedido_PedidoProdutosDiferentes_DeveAtualizarValorTotal()
        {
            //Arrange
            var produtoId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Um", 2, 50);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItem2 = new PedidoItem(produtoId, "Produto Dois", 7, 12);
            pedido.AdicionarItem(pedidoItem2);

            var pedidoItem2Atualizado = new PedidoItem(produtoId, "Produto Dois", 3, 12);
            var valorTotal = (pedidoItem.Quantidade * pedidoItem.ValorUnitario) +
                              (pedidoItem2Atualizado.Quantidade * pedidoItem2Atualizado.ValorUnitario);

            //Act
            pedido.AtualizarItem(pedidoItem2Atualizado);
            //Assert
            Assert.Equal(valorTotal, pedido.ValorTotal);

        }

        [Fact(DisplayName = "Atualizar Item Quantidade Acima do Permitido")]
        [Trait("Vendas", "Testes de Pedido")]
        public void AtualizarItemPedido_ItemExistenteAcimaDoPermitido_DeveRetornarUmaException()
        {
            //Arrange
            var produtoId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(produtoId, "Produto excedendo estoque", 4, 50);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItem2 = new PedidoItem(produtoId, "Produto excedendo estoque", Pedido.MAX_UNIDADES_ITEM + 1, 50);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItem2));
        }

        [Fact(DisplayName = "Remover Item Pedido Válido")]
        [Trait("Vendas", "Testes de Pedido")]
        public void RemoverItemPedido_ItemValido_DeveAtualizarValorTotal()
        {
            //Arrange
            var produtoId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(produtoId, "Produto Um", 2, 100);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Dois", 1, 50);
            pedido.AdicionarItem(pedidoItem);
            pedido.AdicionarItem(pedidoItem2);
            var valorTotal = pedidoItem2.Quantidade * pedidoItem2.ValorUnitario;
            //Act
            pedido.RemoverItem(pedidoItem);
            //Assert
            Assert.Equal(valorTotal, pedido.ValorTotal);

        }
        
        [Fact(DisplayName = "Aplicar Voucher Inválido")]
        [Trait("Vendas", "Pedido / Voucher")]
        public void Pedido_AplicarVoucherInvalido_DeveRetornarComErros()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucherInvadido = new Voucher("PROMO10", null, 10, 1,
                TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), true, false);
            //Act
            var result = pedido.AplicarVoucher(voucherInvadido);

            //Assert
            Assert.False(result.IsValid);
            

        }

        [Fact(DisplayName = "Aplicar Voucher Válido")]
        [Trait("Vendas", "Pedido / Voucher")]
        public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("PROMO10", null, 10, 1,
                TipoDescontoVoucher.Valor, DateTime.Now.AddDays(1), true, false);
            //Act
            var result = pedido.AplicarVoucher(voucher);

            //Assert
            Assert.True(result.IsValid);
            Assert.Equal(voucher.Id, pedido.VoucherId);
            Assert.True(pedido.VoucherUtilizado);
        }

        [Fact(DisplayName = "Aplicar Voucher Tipo Valor E Realizar Desconto")]
        [Trait("Vendas", "Pedido / Voucher")]
        public void Pedido_AplicarVoucher_DeveDarODesconto()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("PROMO10", null, 10, 1,
                TipoDescontoVoucher.Valor, DateTime.Now.AddDays(1), true, false);
            var item1 = new PedidoItem(Guid.NewGuid(), "Protudo um", 1, 100);
            var item2 = new PedidoItem(Guid.NewGuid(), "Protudo dois", 2, 50);
            pedido.AdicionarItem(item1);
            pedido.AdicionarItem(item2);

            var valorComDesconto = pedido.ValorTotal - voucher.ValorDesconto;

            //Act
            pedido.AplicarVoucher(voucher);

            //Assert
            Assert.Equal(valorComDesconto, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher Tipo Percentual E Realizar Desconto")]
        [Trait("Vendas", "Pedido / Voucher")]
        public void Pedido_AplicarVoucherPercentual_DeveDarODesconto()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("PROMO50", 50, 10, 1,
                TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(1), true, false);
            var item1 = new PedidoItem(Guid.NewGuid(), "Protudo um", 1, 100);
            var item2 = new PedidoItem(Guid.NewGuid(), "Protudo dois", 2, 50);
            pedido.AdicionarItem(item1);
            pedido.AdicionarItem(item2);

            var desc = (pedido.ValorTotal * voucher.Percentual) / 100;
            var valorComDesconto = pedido.ValorTotal - desc;

            //Act
            pedido.AplicarVoucher(voucher);

            //Assert
            Assert.Equal(valorComDesconto, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher Desconto Maior que o Total")]
        [Trait("Vendas", "Pedido / Voucher")]
        public void Pedido_AplicarVoucher_DeveZerarOValorTotal()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("PROMOCEM", null, 100, 1,
                TipoDescontoVoucher.Valor, DateTime.Now.AddDays(1), true, false);
            var item1 = new PedidoItem(Guid.NewGuid(), "Protudo um", 2, 39);
            
            pedido.AdicionarItem(item1);

            //Act
            pedido.AplicarVoucher(voucher);

            //Assert
            Assert.Equal(0.01m, pedido.ValorTotal);
        }
    }
}
