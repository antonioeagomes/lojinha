using System.ComponentModel.DataAnnotations;

namespace Store.Vendas.Application.Queries.DTO
{
    public class CarrinhoPagamentoDTO
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string NomeCartao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string NumeroCartao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string ExpiracaoCartao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string CvvCartao { get; set; }
    }
}
