using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Messages
{
    public abstract class Message
    {
        public string MessageType { get; private set; }

        public Guid AggregateId { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
