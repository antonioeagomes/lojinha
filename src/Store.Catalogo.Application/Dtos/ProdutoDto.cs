using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store.Catalogo.Application.Dtos
{
    public class ProdutoDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get;  set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Descricao { get;  set; }
        
        public bool Ativo { get;  set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Valor { get;  set; }
        
        public string Imagem { get;  set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime DataCadastro { get;  set; }
        
        public int QuantidadeEstoque { get;  set; }
        
        public Guid CategoriaId { get;  set; }
        
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public CategoriaDto Categoria { get;  set; }

        public IEnumerable<CategoriaDto> Categorias { get; set; }
        public decimal Altura { get;  set; }
        public decimal Largura { get; set; }
        public decimal Profundidade { get; set; }
    }
}
