using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Pagamentos.Business
{
    // Implementada na camada anticorrupção
    public interface IPagamentoCartaoCreditoFacade
    {
        Transacao RealizarPagamento(Pedido pedido, Pagamento pagamento);
    }
}
