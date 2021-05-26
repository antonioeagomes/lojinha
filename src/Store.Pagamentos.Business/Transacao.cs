using Store.Core.DomainObjects;
using System;

namespace Store.Pagamentos.Business
{
    public class Transacao : Entity
    {
        public Guid PedidoId { get; private set; }
        public Guid PagamentoId { get; private set; }
        public decimal Total { get; private set; }
        public StatusTransacao StatusTransacao { get; private set; }

        public Pagamento Pagamento { get; set; }
    }
}
