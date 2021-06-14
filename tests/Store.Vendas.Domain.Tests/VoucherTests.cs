using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Store.Vendas.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Voucher Tipo Valor Válido")]
        [Trait("Vendas", "Pedido / Voucher")]
        public void Voucher_ValidarTipoValor_DeveEstarValido()
        {
            //Arrange
            var voucher = new Voucher("PROMO10", null, 10, 1, 
                TipoDescontoVoucher.Valor, DateTime.Now.AddDays(1), true, false);

            //Act
            var result = voucher.ValidarAplicabilidade();

            //Assert
            Assert.True(result.IsValid) ;
        }

        [Fact(DisplayName = "Voucher Tipo Valor Invalido")]
        [Trait("Vendas", "Pedido / Voucher")]
        public void Voucher_ValidarTipoValor_DeveEstarInvalido()
        {
            //Arrange
            var voucher = new Voucher("", null, null, 0,
                TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), false, true);

            //Act

            var result = voucher.ValidarAplicabilidade();

            //Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherAplicavelValidation.CodigoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.QuantidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.DataValidadeVencido, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.AtivoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.UtilizadoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.ValorErroMsg, result.Errors.Select(c => c.ErrorMessage));
        }
    }
}
