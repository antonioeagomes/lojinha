using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Store.Catalogo.Domain.Tests
{
    [Collection(nameof(ProdutoTestsCollection))]
    public class ProdutoInvalidoTests
    {
        readonly ProdutoTestsFixture _produtoTestsFixture;
        public ProdutoInvalidoTests(ProdutoTestsFixture produtoTestsFixture)
        {
            _produtoTestsFixture = produtoTestsFixture;
        }

        [Fact(DisplayName = "Novo Produto deve ser inválido")]
        [Trait("Catálogo", "Testes na entidade Produto")]
        public void Produto_NovoProduto_DeveSerInvalido()
        {
            // Arrange
            var produto = _produtoTestsFixture.CriarProdutoInvalido();
            // Act
            var result = produto.IsValido();

            //Assert
            Assert.False(result);
            Assert.NotEmpty(produto.ValidationResult.Errors);
        }
    }
}
