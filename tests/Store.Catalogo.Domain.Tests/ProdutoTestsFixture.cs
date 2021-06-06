﻿using Bogus;
using System;
using System.Collections.Generic;
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
            return new Produto("Camiseta stah uoh",
                "Camiseta com o Darth Vader falando: Está o ó",
                true,
                75.90m,
                Guid.NewGuid(),
                "picture01.jpg",
                DateTime.Now,
                new Dimensoes(0.65m, 0.35m, 0.03m));
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

        public void Dispose()
        {
        }
    }
}
