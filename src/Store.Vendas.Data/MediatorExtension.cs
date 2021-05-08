using Store.Core.Communication.Mediator;
using Store.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Vendas.Data
{
    public static class MediatorExtension
    {
        public static async Task PublicarEventos(this IMediatRHandler mediator, VendasContext context)
        {
            // Todas as entidades que possuem notificações
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

            // Todos os eventos
            var domainEvents = domainEntities.SelectMany(e => e.Entity.Notificacoes).ToList();

            domainEntities.ToList()
                .ForEach(e => e.Entity.LimparEventos());

            // Publica um a um
            var tasks = domainEvents.Select(async (domainEvent) =>
            {
                await mediator.PublicarEvento(domainEvent);
            });

            await Task.WhenAll(tasks);
        }
    }
}
