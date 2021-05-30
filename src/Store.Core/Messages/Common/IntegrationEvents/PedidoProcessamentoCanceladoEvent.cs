using Store.Core.DomainObjects.DTO;
using Store.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Messages.Common.IntegrationEvents
{
    public class PedidoProcessamentoCanceladoEvent : Event
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }
        public ListaProdutosPedido ListaProdutosPedido { get; private set; }

        public PedidoProcessamentoCanceladoEvent(Guid pedidoId, Guid clienteId, ListaProdutosPedido listaProdutosPedido)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
            ListaProdutosPedido = listaProdutosPedido;
        }
    }
}
