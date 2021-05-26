using Store.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Pagamentos.Business
{
    public class Pagamento : Entity, IAggregateRoot
    {
        public Guid PedidoId { get; private set; }
        public string Status { get; private set; }
        public decimal Valor { get; private set; }
        public string NomeCartao { get; private set; }
        public Transacao Transacao { get; set; }
    }
}
