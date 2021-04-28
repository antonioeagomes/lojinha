﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Store.Catalogo.Application.Services;
using Store.Catalogo.Data;
using Store.Catalogo.Data.Repository;
using Store.Catalogo.Domain;
using Store.Catalogo.Domain.Events;
using Store.Core.Bus;
using Store.Vendas.Application.Commands;
using Store.Vendas.Data;
using Store.Vendas.Data.Repository;
using Store.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.WebApp.Mvc.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {

            services.AddScoped<IMediatRHandler, MediatRHandler>();
            
            //Catalogo
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoAppService, ProdutoAppService>();
            services.AddScoped<IEstoqueService, EstoqueService>();
            services.AddScoped<CatalogoContext>();

            services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();

            //Vendas
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<VendasContext>();            

            services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
        }
    }
}
