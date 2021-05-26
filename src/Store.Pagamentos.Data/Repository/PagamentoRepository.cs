using Store.Core.Data;
using Store.Pagamentos.Business;


namespace Store.Pagamentos.Data.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        public readonly PagamentoContext _context;

        public PagamentoRepository(PagamentoContext context)
        {
            _context = context;
        }

        public IUnitOfwork UnitOfwork => _context;

        public void Adicionar(Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
        }

        public void AdicionarTransacao(Transacao transacao)
        {
            _context.Transacoes.Add(transacao);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
