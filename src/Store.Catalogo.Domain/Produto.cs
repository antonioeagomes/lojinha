using Store.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Catalogo.Domain
{
    public class Produto : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
        public decimal Valor { get; private set; }
        public string Imagem { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public int QuantidadeEstoque { get; private set; }
        public Guid CategoriaId { get; private set; }
        public Categoria Categoria { get; private set; }

        public Produto(string nome, string descricao, bool ativo, decimal valor, Guid categoriaId, string imagem, DateTime dataCadastro)
        {
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
            Valor = valor;
            Imagem = imagem;
            DataCadastro = dataCadastro;
            CategoriaId = categoriaId;
        }

        /* AD hoc setters */
        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;

        public void AlterarCategoria(Categoria categoria)
        {
            CategoriaId = categoria.Id;
            Categoria = categoria;
        }

        public void AlterarDescricao(string descricao) => Descricao = descricao;

        public void DebitarEstoque(int quantidade) => QuantidadeEstoque -= Math.Abs(quantidade);

        public void ReporEstoque(int quantidade) => QuantidadeEstoque += Math.Abs(quantidade);        

        public bool PossuiEstoque(int quantidade) => QuantidadeEstoque >= Math.Abs(quantidade);

        public void Validar()
        {

        }
    }
}
