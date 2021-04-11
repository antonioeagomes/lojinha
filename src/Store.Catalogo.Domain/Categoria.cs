using Store.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Catalogo.Domain
{
    public class Categoria : Entity
    {
        public string Nome { get; private set; }
        public int Codigo { get; private set; }

        public ICollection<Produto> Produtos { get; set; }

        protected Categoria() { }

        public Categoria(string nome, int codigo)
        {
            Nome = nome;
            Codigo = codigo;

            Validar();
        }

        public override string ToString()
        {
            return $"{Nome} - {Codigo}";
        }

        public void Validar()
        {
            AssertionConcerns.ValidarSeVazio(Nome, "Nome da categoria não pode ser vazio.");
            AssertionConcerns.ValidarSeMenorQue(Codigo, 0, "Código deve ser positivo.");
        }
    }
}
