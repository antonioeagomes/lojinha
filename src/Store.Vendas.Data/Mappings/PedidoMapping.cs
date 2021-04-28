using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Vendas.Data.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ValorTotal)
                .HasColumnType("decimal(19,2)");

            builder.Property(c => c.Desconto)
                .HasColumnType("decimal(19,2)");

            builder.Property(c => c.Codigo)
                .HasDefaultValueSql("NEXT VALUE FOR Sequencia");

            // 1 : N => Pedido : PedidoItems
            builder.HasMany(c => c.PedidoItems)
                .WithOne(c => c.Pedido)
                .HasForeignKey(c => c.PedidoId);

            builder.ToTable("Pedidos");
        }
    }
}
