using EventSourcing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Store.Catalogo.Application.Services;
using Store.Catalogo.Data;
using Store.Catalogo.Data.Repository;
using Store.Catalogo.Domain;
using Store.Catalogo.Domain.Events;
using Store.Core.Communication.Mediator;
using Store.Core.Data.EventSourcing;
using Store.Core.Messages.Common.IntegrationEvents;
using Store.Core.Messages.Common.Notifications;
using Store.Pagamentos.AntiCorruption;
using Store.Pagamentos.Business;
using Store.Pagamentos.Business.Events;
using Store.Pagamentos.Data;
using Store.Pagamentos.Data.Repository;
using Store.Vendas.Application.Commands;
using Store.Vendas.Application.Events;
using Store.Vendas.Application.Queries;
using Store.Vendas.Data;
using Store.Vendas.Data.Repository;
using Store.Vendas.Domain;

namespace Store.WebApp.Mvc.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Communication
            services.AddScoped<IMediatRHandler, MediatRHandler>();

            // Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Event Sourcing
            services.AddSingleton<IEventStoreService, EventStoreService>();
         
            services.AddSingleton<IEventSourcingRepository, EventSourcingRepository>();

            //Catalogo
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoAppService, ProdutoAppService>();
            services.AddScoped<IEstoqueService, EstoqueService>();
            services.AddScoped<CatalogoContext>();

            services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoIniciadoEvent>, ProdutoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoProcessamentoCanceladoEvent>, ProdutoEventHandler>();

            //Vendas
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoQueries, PedidoQueries>();
            services.AddScoped<VendasContext>();            

            services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<RemoverItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<AplicarVoucherPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<IniciarPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<CancelarProcessamentoPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<CancelarProcessamentoPedidoEstornarEstoque, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<FinalizarPedidoCommand, bool>, PedidoCommandHandler>();

            services.AddScoped<INotificationHandler<PedidoRascunhoIniciadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoAtualizadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoItemAdicionadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PagamentoRealizadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PagamentoRecusadoEvent>, PedidoEventHandler>();


            // Pagamento
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
            services.AddScoped<IConfigurationManager, ConfigurationManager>();
            services.AddScoped<IPayPalGateway, PayPalGateway>();

            services.AddScoped<INotificationHandler<PedidoEstoqueConfirmadoEvent>, PagamentoEventHandler>();

            services.AddScoped<PagamentoContext>();
            
        }
    }
}
