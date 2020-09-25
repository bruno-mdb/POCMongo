using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo.Migrations
{
    public partial class v100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoPedidos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AspNetUserInsertId = table.Column<Guid>(nullable: false),
                    Inserted = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<DateTime>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true),
                    Grupo = table.Column<short>(nullable: false),
                    RevendaNaoRevenda = table.Column<short>(nullable: false),
                    EntradaSaida = table.Column<short>(nullable: false),
                    Financeiro = table.Column<bool>(nullable: false),
                    EmiteNF = table.Column<bool>(nullable: false),
                    Estoque = table.Column<bool>(nullable: false),
                    Contabiliza = table.Column<bool>(nullable: false),
                    PortalAdm = table.Column<bool>(nullable: false),
                    PortalLoja = table.Column<bool>(nullable: false),
                    PortalFornecedor = table.Column<bool>(nullable: false),
                    PortalLog = table.Column<bool>(nullable: false),
                    Logistica = table.Column<bool>(nullable: false),
                    PDV = table.Column<bool>(nullable: false),
                    Precifica = table.Column<bool>(nullable: false),
                    PedidoSaida = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoPedidos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoPedidos");
        }
    }
}
