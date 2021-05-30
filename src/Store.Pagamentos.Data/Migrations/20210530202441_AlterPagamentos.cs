using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Pagamentos.Data.Migrations
{
    public partial class AlterPagamentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CvvCartao",
                table: "Pagamentos",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpiracaoCartao",
                table: "Pagamentos",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroCartao",
                table: "Pagamentos",
                type: "varchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvvCartao",
                table: "Pagamentos");

            migrationBuilder.DropColumn(
                name: "ExpiracaoCartao",
                table: "Pagamentos");

            migrationBuilder.DropColumn(
                name: "NumeroCartao",
                table: "Pagamentos");
        }
    }
}
