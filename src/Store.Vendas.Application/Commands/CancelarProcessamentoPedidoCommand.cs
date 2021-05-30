using Store.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Vendas.Application.Commands
{
   
    public class CancelarProcessamentoPedidoCommand : Command
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }

        public CancelarProcessamentoPedidoCommand(Guid pedidoId, Guid clientId)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clientId;
        }

    }
}
