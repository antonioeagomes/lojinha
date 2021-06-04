using EventStore.ClientAPI;
using Newtonsoft.Json;
using Store.Core.Data.EventSourcing;
using Store.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing
{
    public class EventSourcingRepository : IEventSourcingRepository
    {
        private readonly IEventStoreService _eventStoreService;

        public EventSourcingRepository(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }

        public async Task<IEnumerable<StoredEvent>> ObterEventos(Guid aggregateId)
        {
            var eventos = await _eventStoreService.GetConnection().ReadStreamEventsForwardAsync(
                    aggregateId.ToString(), 0, 500, false);
            
            var listaEventos = new List<StoredEvent>();

            foreach (var resolvedEvent in eventos.Events)
            {
                var dataEncoded = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
                var jsonData = JsonConvert.DeserializeObject<Event>(dataEncoded);

                var evento = new StoredEvent(resolvedEvent.Event.EventId,
                    resolvedEvent.Event.EventType,
                    jsonData.Timestamp,
                    dataEncoded);

                listaEventos.Add(evento);
            }

            return listaEventos;
        
        }

        public async Task SalvarEvento<TEvent>(TEvent evento) where TEvent : Event
        {
            var conn = _eventStoreService.GetConnection();
            await conn.AppendToStreamAsync(
                evento.AggregateId.ToString(), 
                ExpectedVersion.Any, 
                FormatarEvento(evento));
        }

        private static IEnumerable<EventData> FormatarEvento<TEvent>(TEvent evento) where TEvent : Event
        {
            yield return new EventData(
                Guid.NewGuid(),
                evento.MessageType,
                true,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evento)),
                null
                );
        }
    }
}
