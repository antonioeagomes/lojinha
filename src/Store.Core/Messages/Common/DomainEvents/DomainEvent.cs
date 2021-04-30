using Store.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Messages.Common.DomainEvents
{
    public class DomainEvent : Event
    {
        public DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
