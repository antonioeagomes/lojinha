using FluentValidation;
using Store.Core.Messages;
using System;

namespace Store.Vendas.Application.Commands
{
    public class IniciarPedidoCommand : Command
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }
        public decimal Total { get; private set; }
        public string NomeCartao { get; private set; }
        public string NumeroCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CvvCartao { get; private set; }
        
        public IniciarPedidoCommand(Guid clienteId, Guid pedidoId, decimal total, 
            string nomeCartao, string numeroCartao, string expiracaoCartao, string cvvCartao)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            Total = total;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CvvCartao = cvvCartao;
        }

        public override bool IsValido()
        {
            ValidationResult = new IniciarPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class IniciarPedidoValidation : AbstractValidator<IniciarPedidoCommand>
    {
        public IniciarPedidoValidation()
        {
            RuleFor(c => c.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do Cliente inválido. Deve estar logado");

            RuleFor(c => c.PedidoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do Produto inválido.");

            RuleFor(c => c.NumeroCartao)
                .CreditCard()
                .WithMessage("Número de cartão inválido");

            RuleFor(c => c.CvvCartao)
                .Length(3, 4)
                .WithMessage("CVV inválido");

            RuleFor(c => c.ExpiracaoCartao)
                .NotEmpty()
                .WithMessage("Necessário data de validade");
        }
    }
}
