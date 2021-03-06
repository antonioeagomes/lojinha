// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Store.Pagamentos.Data;

namespace Store.Pagamentos.Data.Migrations
{
    [DbContext(typeof(PagamentoContext))]
    [Migration("20210530202441_AlterPagamentos")]
    partial class AlterPagamentos
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Store.Pagamentos.Business.Pagamento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CvvCartao")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ExpiracaoCartao")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NomeCartao")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<string>("NumeroCartao")
                        .HasColumnType("varchar(100)");

                    b.Property<Guid>("PedidoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(19,2)");

                    b.HasKey("Id");

                    b.ToTable("Pagamentos");
                });

            modelBuilder.Entity("Store.Pagamentos.Business.Transacao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PagamentoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PedidoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("StatusTransacao")
                        .HasColumnType("int");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(19,2)");

                    b.HasKey("Id");

                    b.HasIndex("PagamentoId")
                        .IsUnique();

                    b.ToTable("Tansacoes");
                });

            modelBuilder.Entity("Store.Pagamentos.Business.Transacao", b =>
                {
                    b.HasOne("Store.Pagamentos.Business.Pagamento", "Pagamento")
                        .WithOne("Transacao")
                        .HasForeignKey("Store.Pagamentos.Business.Transacao", "PagamentoId")
                        .IsRequired();

                    b.Navigation("Pagamento");
                });

            modelBuilder.Entity("Store.Pagamentos.Business.Pagamento", b =>
                {
                    b.Navigation("Transacao");
                });
#pragma warning restore 612, 618
        }
    }
}
