using FluentValidation;
using Store.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Vendas.Application.Commands
{
    public class AtualizarItemPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }
        // public Guid PedidoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public AtualizarItemPedidoCommand(Guid clienteId, Guid produtoId, int quantidade)
        {
            ClienteId = clienteId;
            // PedidoId = pedidoId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }

        public override bool IsValido()
        {
            ValidationResult = new AtualizarItemPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AtualizarItemPedidoValidation : AbstractValidator<AtualizarItemPedidoCommand>
    {
        public AtualizarItemPedidoValidation()
        {
            RuleFor(c => c.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do Cliente inválido. Deve estar logado");

            //RuleFor(c => c.PedidoId)
            //    .NotEqual(Guid.Empty)
            //    .WithMessage("Pedido Inválido");

            RuleFor(c => c.ProdutoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do Produto inválido.");

            RuleFor(c => c.Quantidade)
                .GreaterThan(0)
                .WithMessage("Deve adicionar pelo menos uma unidade");

            RuleFor(c => c.Quantidade)
                .LessThan(16)
                .WithMessage("Não vendemos no atacado. Quantidade máxima 15 itens");
        }
    }
}
