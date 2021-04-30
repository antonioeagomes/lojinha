using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Communication.Mediator;
using Store.Core.Messages.Common.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.WebApp.Mvc.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatRHandler _mediatorHandler;

        protected Guid ClienteId = Guid.Parse("d5622e45-bda1-4de4-b4c4-c5554b37f8c8");

        public ControllerBase(INotificationHandler<DomainNotification> notifications, IMediatRHandler mediatorHandler)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediatorHandler = mediatorHandler;
        }

        protected bool IsOperacaoValida()
        {
            return !_notifications.PossuiNotificacao();
        }

        protected IEnumerable<string> ObterMensagensErro()
        {
            return _notifications.ObterNotificacoes().Select(c => c.Value).ToList();
        }

        protected void NotificarErro(string codigo, string mensagem)
        {
            _mediatorHandler.PublicarNotificacao(new DomainNotification(codigo, mensagem));
        }
        
    }
}
