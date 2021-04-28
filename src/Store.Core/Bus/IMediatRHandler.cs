using Store.Core.Messages;
using System.Threading.Tasks;

/* Mediator pode ser uma interface para o message bus */
namespace Store.Core.Bus
{
    public interface IMediatRHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<bool> EnviarComando<T>(T comando) where T : Command;
    }
}
