using Store.Core.Messages;
using Store.Core.Messages.Common.Notifications;
using System.Threading.Tasks;

/* Mediator pode ser uma interface para o message bus */
namespace Store.Core.Communication.Mediator
{
    public interface IMediatRHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<bool> EnviarComando<T>(T comando) where T : Command;
        Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;
    }
}
