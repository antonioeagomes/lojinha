using MediatR;
using Moq;
using Moq.AutoMock;
using Store.Core.Communication.Mediator;
using Store.Core.Messages.Common.Notifications;
using Store.Vendas.Application.Commands;
using Store.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Store.Vendas.Application.Tests
{
    public class PedidoCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly PedidoCommandHandler _commandHandler;
        private readonly Guid _clienteId;
        private readonly Guid _produtoId;
        private readonly Pedido _pedidoRascunho;

        public PedidoCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _commandHandler = _mocker.CreateInstance<PedidoCommandHandler>();

            _clienteId = Guid.NewGuid();
            _produtoId = Guid.NewGuid();
            _pedidoRascunho = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
        }

        [Fact(DisplayName = "Adicionar Item Novo com Sucesso")]
        [Trait("Command", "Pedido / Command Handler")]
        public async Task AdicionarItem_NovoPedido_DeveExecutarComSucesso()
        {
            // Arrange 
            var pedidoCommand = new AdicionarItemPedidoCommand(_clienteId, _produtoId,
                "Produto a ser adicionado", 3, 75);        

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfwork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _commandHandler.Handle(pedidoCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Adicionar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfwork.Commit(), Times.Once);            

        }

        [Fact(DisplayName = "Adicionar Item Pedido Rascunho com Sucesso")]
        [Trait("Command", "Pedido / Command Handler")]
        public async Task AdicionarItem_PedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange 
            var pedidoItem1 = new PedidoItem(_produtoId, "Product", 3, 45);
            _pedidoRascunho.AdicionarItem(pedidoItem1);

            var pedidoCommand = new AdicionarItemPedidoCommand(_clienteId, Guid.NewGuid(), "Another Product", 2, 50);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(repo => repo.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(_pedidoRascunho));

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfwork.Commit()).Returns(Task.FromResult(true));

            // Act

            var result = await _commandHandler.Handle(pedidoCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AdicionarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfwork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Item Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            // Act
            var result = await _commandHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatRHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(5));
        }
    }
}
