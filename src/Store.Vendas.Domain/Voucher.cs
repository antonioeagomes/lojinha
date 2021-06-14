using FluentValidation;
using FluentValidation.Results;
using Store.Core.DomainObjects;
using System;
using System.Collections.Generic;

namespace Store.Vendas.Domain
{
    public class Voucher : Entity
    {
        public string Codigo { get; private set; }
        public decimal? Percentual { get; private set; }
        public decimal? ValorDesconto { get; private set; }
        public int Quantidade { get; private set; }
        public TipoDescontoVoucher TipoDescontoVoucher { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataUtilizacao { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Utilizado { get; private set; }
        public ICollection<Pedido> Pedidos { get; set; }

        protected Voucher()
        {

        }

        public Voucher(string codigo, decimal? percentual, decimal? valorDesconto, int quantidade,
            TipoDescontoVoucher tipo, DateTime validade, bool ativo, bool utilizado)
        {
            Codigo = codigo;
            Percentual = percentual;
            ValorDesconto = valorDesconto;
            Quantidade = quantidade;
            TipoDescontoVoucher = tipo;
            DataCriacao = DateTime.Now;
            DataValidade = validade;
            Ativo = ativo;
            Utilizado = utilizado;
        }

        public override bool IsValido()
        {
            return base.IsValido();
        }
        
        public ValidationResult ValidarAplicabilidade()
        {
            return new VoucherAplicavelValidation().Validate(this);
        }

    }

    public class VoucherAplicavelValidation : AbstractValidator<Voucher>
    {
        public static string CodigoErroMsg => "Código é obrigatório";
        public static string DataValidadeVencido => "Este voucher está expirado.";
        public static string AtivoErroMsg => "Este voucher não está mais ativo.";
        public static string UtilizadoErroMsg => "Este voucher já foi utilizado.";
        public static string QuantidadeErroMsg => "Este voucher está expirado.";
        public static string PercentualErroMsg => "Voucher do tipo percentual deve possuir um valor em porcentagem";
        public static string ValorErroMsg => "Voucher do tipo Valor deve possuir um valor em reais";
        public VoucherAplicavelValidation()
        {
            RuleFor(c => c.Codigo)
                .NotEmpty()
                .WithMessage(CodigoErroMsg);

            RuleFor(c => c.DataValidade)
                .Must(DataVencimentoSuperiorAtual)
                .WithMessage(DataValidadeVencido);

            RuleFor(c => c.Ativo)
                .Equal(true)
                .WithMessage(AtivoErroMsg);

            RuleFor(c => c.Utilizado)
                .Equal(false)
                .WithMessage(UtilizadoErroMsg);

            RuleFor(c => c.Quantidade)
                .GreaterThan(0)
                .WithMessage(QuantidadeErroMsg);

            When(c => c.TipoDescontoVoucher == TipoDescontoVoucher.Porcentagem, () =>
            {
                RuleFor(c => c.Percentual)
                    .NotNull()
                    .WithMessage(PercentualErroMsg)
                    .GreaterThan(0)
                    .WithMessage(PercentualErroMsg);
            });

            When(c => c.TipoDescontoVoucher == TipoDescontoVoucher.Valor, () =>
            {
                RuleFor(c => c.ValorDesconto)
                    .NotNull()
                    .WithMessage(ValorErroMsg)
                    .GreaterThan(0)
                    .WithMessage(ValorErroMsg);
            });
        }

        protected static bool DataVencimentoSuperiorAtual(DateTime dataValidade)
        {
            return dataValidade >= DateTime.Now;
        }
    }
}
