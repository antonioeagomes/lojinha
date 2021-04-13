using Store.Core.DomainObjects;
using System;

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
        public Dimensoes Dimensoes { get; private set; }

        protected Produto()
        {

        }

        public Produto(string nome, string descricao, bool ativo, decimal valor, 
            Guid categoriaId, string imagem, DateTime dataCadastro, Dimensoes dimensoes)
        {
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
            Valor = valor;
            Imagem = imagem;
            DataCadastro = dataCadastro;
            CategoriaId = categoriaId;
            Dimensoes = dimensoes;

            Validar();
        }

        /* AD hoc setters */
        public void Ativar() => Ativo = true;

        public void Desativar() => Ativo = false;

        public void AlterarCategoria(Categoria categoria)
        {
            CategoriaId = categoria.Id;
            Categoria = categoria;
        }

        public void AlterarDescricao(string descricao)
        {
            AssertionConcerns.ValidarSeVazio(descricao, "O campo Descricao do produto não pode estar vazio");
            Descricao = descricao;
        }

        public void AlterarDimensoes(Dimensoes dimensoes)
        {
            AssertionConcerns.ValidarSeIgual(dimensoes, null, "Não pode ser nulo");
            Dimensoes = dimensoes;
        }

        public void RemoverDimensoes()
        {
            Dimensoes = null;
        }

        public void DebitarEstoque(int quantidade) {
            
            if (!PossuiEstoque(quantidade)) throw new DomainException("Estoque insuficiente");

            QuantidadeEstoque -= Math.Abs(quantidade); 
        }

        public void ReporEstoque(int quantidade) => QuantidadeEstoque += Math.Abs(quantidade);        

        public bool PossuiEstoque(int quantidade) => QuantidadeEstoque >= Math.Abs(quantidade);

        public void Validar()
        {
            AssertionConcerns.ValidarSeVazio(Nome, "O campo Nome do produto não pode estar vazio");
            AssertionConcerns.ValidarSeVazio(Descricao, "O campo Descricao do produto não pode estar vazio");
            AssertionConcerns.ValidarSeIgual(CategoriaId, Guid.Empty, "O campo CategoriaId do produto não pode estar vazio");
            AssertionConcerns.ValidarSeMenorQue(Valor, 0.1m, "O campo Valor do produto não pode se menor igual a 0");
            AssertionConcerns.ValidarSeVazio(Imagem, "O campo Imagem do produto não pode estar vazio");
        }
    }
}
