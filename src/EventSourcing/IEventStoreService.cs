using EventStore.ClientAPI;
using System.Collections.Generic;
using System.Text;

namespace EventSourcing
{
    public interface IEventStoreService
    {
       IEventStoreConnection GetConnection();
    }
}
