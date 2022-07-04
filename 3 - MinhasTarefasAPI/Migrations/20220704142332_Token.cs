using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _3___MinhasTarefasAPI.Migrations
{
    public partial class Token : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Utilizado = table.Column<bool>(type: "bit", nullable: false),
                    ExpirationToken = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationRefreshToken = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Criado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Atualizado = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Token_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Token_UsuarioId",
                table: "Token",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Token");
        }
    }
}
