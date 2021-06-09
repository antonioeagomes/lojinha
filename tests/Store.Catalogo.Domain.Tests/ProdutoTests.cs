using AutoMapper;
using Moq;
using Moq.AutoMock;
using Store.Catalogo.Application.AutoMapper;
using Store.Catalogo.Application.Dtos;
using Store.Catalogo.Application.Services;
using Store.Core.Data;
using Store.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Store.Catalogo.Domain.Tests
{
    [Collection(nameof(ProdutoTestsCollection))]
    public class ProdutoTests
    {
        readonly ProdutoTestsFixture _produtoTestsFixture;

        private static IMapper _mapper;
        public ProdutoTests(ProdutoTestsFixture produtoTestsFixture)
        {
            _produtoTestsFixture = produtoTestsFixture;
            
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact (DisplayName ="Validações devem retornar exceptions")]
        [Trait("Catálogo", "Testes na entidade Produto")]
        public void Produto_Validar_VilidacoesDevemRetornarExceptions()
        {
            // Arrange & Act & Assert

            var ex = Assert.Throws<DomainException>(() =>
                new Produto(string.Empty, "Descricao", false, 100, Guid.NewGuid(), "Imagem", DateTime.Now, new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Nome do produto não pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", string.Empty, false, 100, Guid.NewGuid(), "Imagem", DateTime.Now, new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Descricao do produto não pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 100, Guid.Empty, "Imagem", DateTime.Now, new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo CategoriaId do produto não pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 100, Guid.NewGuid(), string.Empty, DateTime.Now, new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Imagem do produto não pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 100, Guid.NewGuid(), "Imagem", DateTime.Now, new Dimensoes(0, 1, 1))
            );

            Assert.Equal("A altura não pode ser menor ou igual a 0", ex.Message);
        }
    
        [Fact(DisplayName ="Adicionar Produto com sucesso")]
        [Trait("Catálogo", "ProdutoService Mock")]
        public async Task Produto_Adicionar_DeveAdicionarComSucessoAsync()
        {
            //Arrange
            var produto = _produtoTestsFixture.CriarProdutoUsandoBogus();
            var produtoRepository = new Mock<IProdutoRepository>();
            var estoqueService = new Mock<IEstoqueService>();

            produtoRepository.Setup(p => p.UnitOfwork).Returns(new Mock<IUnitOfwork>().Object);

            var produtoAppService = new ProdutoAppService(produtoRepository.Object, estoqueService.Object, _mapper);

            //Act
            await produtoAppService.Adicionar(_mapper.Map<ProdutoDto>(produto));

            // Assert
            Assert.True(produto.IsValido());
            produtoRepository.Verify(r => r.Adicionar(produto), Times.Once); // Verifica se este método foi chamado
            

        }


        [Fact(DisplayName = "Adicionar Produto com sucesso")]
        [Trait("Catálogo", "ProdutoService Auto Mock")]
        public void Produto_ObterTodos_DeveRetornarTodosProdutos()
        {
            var produtos = _produtoTestsFixture.CriarProdutosDiversificados();
            var mocker = new AutoMocker();

            mocker.GetMock<IProdutoRepository>().Setup(p => p.ObterTodos().Result)
                    .Returns(produtos);

            var produtoService = mocker.CreateInstance<ProdutoAppService>();

            var result = produtoService.ObterTodos().Result;

            Assert.IsAssignableFrom<IEnumerable<ProdutoDto>>(result);

        }
    }
}
