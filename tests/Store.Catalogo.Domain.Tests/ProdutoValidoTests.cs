using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Store.Catalogo.Domain.Tests
{
    [Collection(nameof(ProdutoTestsCollection))]
    public class ProdutoValidoTests
    {
        readonly ProdutoTestsFixture _produtoTestsFixture;
        
        public ProdutoValidoTests(ProdutoTestsFixture produtoTestsFixture)
        {
            _produtoTestsFixture = produtoTestsFixture;
        }

        [Fact(DisplayName = "Novo Produto deve ser válido")]
        [Trait("Catálogo", "Testes na entidade Produto")]
        public void Produto_NovoProduto_DeveSerValido()
        {
            // Arrange
            var produto = _produtoTestsFixture.CriarProdutoUsandoBogus();
            // Act
            var result = produto.IsValido();

            //Assert
            result.Should().BeTrue();
            produto.ValidationResult.Errors.Should().BeEmpty();
            Assert.True(result);
            Assert.Empty(produto.ValidationResult.Errors);
        }
    }
}
