using FluentValidation;
using Store.Core.Messages;
using Store.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Vendas.Application.Commands
{
    public class AdicionarItemPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public AdicionarItemPedidoCommand(Guid clienteId, Guid produtoId, 
            string produtoNome, int quantidade, decimal valorUnitario)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public override bool IsValido()
        {
            ValidationResult = new AdicionarItemPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AdicionarItemPedidoValidation : AbstractValidator<AdicionarItemPedidoCommand>
    {
        public AdicionarItemPedidoValidation()
        {
            RuleFor(c => c.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do Cliente inválido. Deve estar logado");

            RuleFor(c => c.ProdutoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do Produto inválido.");

            RuleFor(c => c.ProdutoNome)
                .NotEmpty()
                .WithMessage("Produto deve ter uma descrição.");

            RuleFor(c => c.Quantidade)
                .GreaterThan(0)
                .WithMessage("Deve adicionar pelo menos uma unidade");
            
            RuleFor(c => c.Quantidade)
                .LessThanOrEqualTo(Pedido.MAX_UNIDADES_ITEM)
                .WithMessage($"Não vendemos no atacado. Quantidade máxima {Pedido.MAX_UNIDADES_ITEM} itens");

            RuleFor(c => c.ValorUnitario)
                .GreaterThan(0)
                .WithMessage("Valor inválido");
        }
    }
}
