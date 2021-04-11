using Store.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Catalogo.Domain
{
    /* Objeto de valor */
    public class Dimensoes
    {
        public decimal Altura { get; private set; }
        public decimal Largura { get; private set; }
        public decimal Profundidade { get; private set; }

        public Dimensoes(decimal altura, decimal largura, decimal profundidade)
        {
            AssertionConcerns.ValidarSeMenorQue(altura, 0.1m, "A altura não pode ser menor ou igual a 0");
            AssertionConcerns.ValidarSeMenorQue(largura, 0.1m, "A altura não pode ser menor ou igual a 0");
            AssertionConcerns.ValidarSeMenorQue(profundidade, 0.1m, "A altura não pode ser menor ou igual a 0");

            Altura = altura;
            Largura = largura;
            Profundidade = profundidade;
        }

        public string DescricaoFormatada() => $"LxAxP: {Largura} x {Altura} x {Profundidade}";        

        public override string ToString()
        {
            return DescricaoFormatada();
        }
    }
}
