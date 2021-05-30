using Store.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Vendas.Application.Events
{
    // Poderia ser um evento de integração. Com o módulo Fiscal
    public class PedidoFinalizadoEvent : Event
    {
        public Guid PedidoId { get; private set; }

        public PedidoFinalizadoEvent(Guid pedidoId)
        {
            PedidoId = pedidoId;
            AggregateId = pedidoId;
        }
    }
}
