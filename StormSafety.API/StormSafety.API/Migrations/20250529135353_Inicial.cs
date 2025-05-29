using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StormSafety.API.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StormDatabase_TipoOcorrencia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NomeTipo = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StormDatabase_TipoOcorrencia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StormDatabase_Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Localizacao = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StormDatabase_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StormDatabase_Ocorrencia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Descricao = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    DataHora = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UsuarioId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TipoOcorrenciaId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StormDatabase_Ocorrencia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StormDatabase_Ocorrencia_StormDatabase_TipoOcorrencia_TipoOcorrenciaId",
                        column: x => x.TipoOcorrenciaId,
                        principalTable: "StormDatabase_TipoOcorrencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StormDatabase_Ocorrencia_StormDatabase_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "StormDatabase_Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StormDatabase_Ocorrencia_TipoOcorrenciaId",
                table: "StormDatabase_Ocorrencia",
                column: "TipoOcorrenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_StormDatabase_Ocorrencia_UsuarioId",
                table: "StormDatabase_Ocorrencia",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StormDatabase_Ocorrencia");

            migrationBuilder.DropTable(
                name: "StormDatabase_TipoOcorrencia");

            migrationBuilder.DropTable(
                name: "StormDatabase_Usuario");
        }
    }
}
