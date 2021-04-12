using Store.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.DomainObjects
{
    public class DomainEvent : Event
    {
        public DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
