using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace financeControl.Migrations
{
    /// <inheritdoc />
    public partial class att : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormaPagamento",
                table: "FinancialObligations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorCOFINS",
                table: "FinancialObligations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorICMS",
                table: "FinancialObligations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorIPI",
                table: "FinancialObligations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorPIS",
                table: "FinancialObligations",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormaPagamento",
                table: "FinancialObligations");

            migrationBuilder.DropColumn(
                name: "ValorCOFINS",
                table: "FinancialObligations");

            migrationBuilder.DropColumn(
                name: "ValorICMS",
                table: "FinancialObligations");

            migrationBuilder.DropColumn(
                name: "ValorIPI",
                table: "FinancialObligations");

            migrationBuilder.DropColumn(
                name: "ValorPIS",
                table: "FinancialObligations");
        }
    }
}
