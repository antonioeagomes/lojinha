using Store.Pagamentos.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Pagamentos.AntiCorruption
{
    public class PagamentoCartaoCreditoFacade : IPagamentoCartaoCreditoFacade
    {
        private readonly IPayPalGateway _payPalGateway;
        private readonly IConfigurationManager _configurationManager;

        public PagamentoCartaoCreditoFacade(IPayPalGateway payPalGateway, IConfigurationManager configurationManager)
        {
            _payPalGateway = payPalGateway;
            _configurationManager = configurationManager;
        }
        public Transacao RealizarPagamento(Pedido pedido, Pagamento pagamento)
        {
            var apiKey = _configurationManager.GetValue("ApiKey");
            var encriptinKey = _configurationManager.GetValue("EncriptionKey");

            var serviceKey = _payPalGateway.GetPayPalServiceKey(apiKey, encriptinKey);
            var cardHashkey = _payPalGateway.GetCardHashKey(serviceKey, pagamento.NumeroCartao);
            // No mundo real, retornaria um objeto a ser convertido em Transacao
            var pagamentoResult = _payPalGateway.CommitTransaction(cardHashkey, pedido.Id.ToString(), pedido.Valor);

            var transacao = new Transacao
            {
                PedidoId = pedido.Id,
                Total = pedido.Valor,
                PagamentoId = pagamento.Id,
                StatusTransacao = StatusTransacao.Pago // pagamentoResult ? StatusTransacao.Pago : StatusTransacao.Recusado
            };
            
            return transacao;
        }
    }
}
