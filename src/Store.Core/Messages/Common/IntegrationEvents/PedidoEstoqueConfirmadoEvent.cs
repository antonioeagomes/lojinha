using Store.Core.DomainObjects.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Messages.Common.IntegrationEvents
{
    public class PedidoEstoqueConfirmadoEvent : IntegrationEvent
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }
        public decimal Total { get; private set; }
        public string NomeCartao { get; private set; }
        public string NumeroCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CvvCartao { get; private set; }
        public ListaProdutosPedido ProdutosPedido { get; private set; }

        public PedidoEstoqueConfirmadoEvent(Guid clienteId, Guid pedidoId, decimal total,
            string nomeCartao, string numeroCartao, string expiracaoCartao, string cvvCartao,
            ListaProdutosPedido produtosPedido)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            Total = total;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CvvCartao = cvvCartao;
            ProdutosPedido = produtosPedido;

        }
    }
}
