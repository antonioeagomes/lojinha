using Store.Vendas.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Store.Vendas.Application.Tests
{
    public class AdicionarItemPedidoCommandTests
    {
        [Fact(DisplayName = "Adicionar Item Pedido Command Válido")]
        [Trait("Command", "Pedido / Adicionar")]
        public void AdicionarItemPedidoCommand_CommandValido_DevePassarNaValidacao()
        {
            //Arrange 
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Nome", 2, 50);

            //Act
            var result = pedidoCommand.IsValido();

            //Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Command Invalido")]
        [Trait("Command", "Pedido / Adicionar")]
        public void AdicionarItemPedidoCommand_CommandInvalido_DeveRetornarFalse()
        {
            //Arrange 
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            //Act
            var result = pedidoCommand.IsValido();

            //Assert
            Assert.False(result);
            Assert.Equal(5,pedidoCommand.ValidationResult.Errors.Count);
            Assert.Contains("Id do Cliente inválido. Deve estar logado", pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }
}
