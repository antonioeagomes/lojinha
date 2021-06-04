using MediatR;
using Store.Core.Data.EventSourcing;
using Store.Core.Messages;
using Store.Core.Messages.Common.Notifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Communication.Mediator
{
    public class MediatRHandler : IMediatRHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventSourcingRepository _eventSourcingRepository;

        public MediatRHandler(IMediator mediator, IEventSourcingRepository eventSourcingRepository)
        {
            _mediator = mediator;
            _eventSourcingRepository = eventSourcingRepository;
        }

        public async Task<bool> EnviarComando<T>(T comando) where T : Command
        {
            return await _mediator.Send(comando);
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
            if (!evento.GetType().BaseType.Name.Equals("DomainEvent"))
                await _eventSourcingRepository.SalvarEvento(evento);
        }
        public async Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification
        {
            await _mediator.Publish(notificacao);
        }
    }
}
