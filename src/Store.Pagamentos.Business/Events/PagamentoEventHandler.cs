using MediatR;
using Store.Core.DomainObjects.DTO;
using Store.Core.Messages.Common.IntegrationEvents;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Pagamentos.Business.Events
{
    public class PagamentoEventHandler : INotificationHandler<PedidoEstoqueConfirmadoEvent>
    {
        private readonly IPagamentoService _pagamentoService;

        public PagamentoEventHandler(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }
        public async Task Handle(PedidoEstoqueConfirmadoEvent notification, CancellationToken cancellationToken)
        {
            var pagamentoPedido = new PagamentoPedido
            {
                PedidoId = notification.PedidoId,
                ClienteId = notification.ClienteId,
                Total = notification.Total,
                CvvCartao = notification.CvvCartao,
                ExipracaoCartao = notification.ExpiracaoCartao,
                NomeCartao = notification.NomeCartao,
                NumeroCartao = notification.NumeroCartao
            };

            await _pagamentoService.RealizarPagamentoPedido(pagamentoPedido);
        }
    }
}
