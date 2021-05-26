using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Pagamentos.Business
{
    public class Pedido
    {
        public Guid Id { get; set; }
        public decimal Valor { get; set; }

        public List<Produto> Produtos { get; set; }

    }

    public class Produto
    {
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
    }
}
