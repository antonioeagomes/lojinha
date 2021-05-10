using Store.Core.Messages;
using System;

namespace Store.Vendas.Application.Events
{
    public class VoucherAplicadoPedidoEvent : Event
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public string VoucherCodigo { get; private set; }

        public VoucherAplicadoPedidoEvent(Guid clienteId, Guid pedidoId, string voucherCodigo)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            VoucherCodigo = voucherCodigo;
        }
    }
}
