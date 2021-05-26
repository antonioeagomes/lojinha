using Store.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Pagamentos.Business
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        void Adicionar(Pagamento pagamento);
        void AdicionarTransacao(Transacao transacao);
    }
}
