using FluentValidation;
using Store.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Vendas.Application.Commands
{
    public class AplicarVoucherPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
       // public Guid PedidoId { get; private set; }
        public string VoucherCodigo { get; private set; }

        public AplicarVoucherPedidoCommand(Guid clienteId, string voucher)
        {
            ClienteId = clienteId;
            //PedidoId = pedidoId;
            VoucherCodigo = voucher;
        }

        public override bool IsValido()
        {
            ValidationResult = new AplicarVoucherPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AplicarVoucherPedidoValidation : AbstractValidator<AplicarVoucherPedidoCommand>
    {
        public AplicarVoucherPedidoValidation()
        {
            RuleFor(c => c.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do Cliente inválido. Deve estar logado.");

            //RuleFor(c => c.PedidoId)
            //    .NotEqual(Guid.Empty)
            //    .WithMessage("Id do Pedido inválido.");

            RuleFor(c => c.VoucherCodigo)
                .NotEmpty()
                .WithMessage("Voucher não pode ser vazio.");
        }
    }
}
