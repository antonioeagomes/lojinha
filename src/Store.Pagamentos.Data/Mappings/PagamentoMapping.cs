using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Pagamentos.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Pagamentos.Data.Mappings
{
    public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.NomeCartao)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.HasOne(p => p.Transacao)
                .WithOne(p => p.Pagamento);

            builder.ToTable("Pagamentos");

        }
    }
}
