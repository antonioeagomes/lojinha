using Bogus;
using Store.Catalogo.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Store.Catalogo.Domain.Tests
{
    [CollectionDefinition(nameof(ProdutoTestsCollection))]
    public class ProdutoTestsCollection : ICollectionFixture<ProdutoTestsFixture>
    {

    }
    public class ProdutoTestsFixture : IDisposable
    {
        public Produto CriarProdutoValido()
        {
            return CriarProdutos(1, true).FirstOrDefault();
        }

        public IEnumerable<Produto> CriarProdutosDiversificados()
        {
            var produtos = new List<Produto>();
            produtos.AddRange(CriarProdutos(50, true));
            produtos.AddRange(CriarProdutos(50, false));

            return produtos;
        }

        public Produto CriarProdutoInvalido()
        {
            return new Produto("Camiseta stah uoh",
                "Camiseta com o Darth Vader falando: Está o ó",
                true,
                0,
                Guid.NewGuid(),
                "picture01.jpg",
                DateTime.Now,
                new Dimensoes(0.65m, 0.35m, 0.03m));
        }

        public Produto CriarProdutoUsandoBogus()
        {
            var prd = new Faker<Produto>("pt_BR")
                .CustomInstantiator(f => new Produto(
                    f.Commerce.Product(),
                    f.Commerce.ProductDescription(),
                    true,
                    Convert.ToDecimal(f.Commerce.Price(0.01m, 1000m, 2)),
                    Guid.NewGuid(),
                    f.Image.PicsumUrl(),
                    f.Date.Past(),
                    new Dimensoes(1,1,1)                    
                    ));
            
            return prd;
        }

        public ICollection<Produto> CriarProdutos(int quantidade, bool ativo)
        {
            var prd = new Faker<Produto>("pt_BR")
                .CustomInstantiator(f => new Produto(
                    f.Commerce.Product(),
                    f.Commerce.ProductDescription(),
                    ativo,
                    Convert.ToDecimal(f.Commerce.Price(0.01m, 1000m, 2)),
                    Guid.NewGuid(),
                    f.Image.PicsumUrl(),
                    f.Date.Past(),
                    new Dimensoes(f.Random.Decimal(0.1m, 100m), f.Random.Decimal(0.1m, 100m), f.Random.Decimal(0.1m, 100m))
                    ));

            return prd.Generate(quantidade);
        }

        public ProdutoDto CriarProdutoDtoUsandoBogus()
        {
            var prd = new Faker<ProdutoDto>("pt_BR")
                .RuleFor(p => p.Nome, f => f.Commerce.Product())
                .RuleFor(p => p.Descricao, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Ativo, f => f.Random.Bool())
                .RuleFor(p => p.Valor, f => Convert.ToDecimal(f.Commerce.Price(0.01m, 1000m, 2)))
                .RuleFor(p => p.CategoriaId, f => f.Random.Uuid())
                .RuleFor(p => p.Imagem, f => f.Image.PicsumUrl())
                .RuleFor(p => p.DataCadastro, f => f.Date.Past())
                .RuleFor(p => p.QuantidadeEstoque, f => f.Random.Int(1, 99))
                .RuleFor(p => p.Altura, f => f.Random.Decimal(0.1m, 100m))
                .RuleFor(p => p.Largura, f => f.Random.Decimal(0.1m, 100m))
                .RuleFor(p => p.Profundidade, f => f.Random.Decimal(0.1m, 100m));

            return prd;
        }

        public void Dispose()
        {
        }
    }
}
