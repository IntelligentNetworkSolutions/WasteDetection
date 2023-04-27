using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WasteDetection.Migrations
{
    /// <inheritdoc />
    public partial class AddTranslateAndPyramidRequestsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuildPyramidsRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InpLayerPath = table.Column<string>(type: "TEXT", nullable: false),
                    Suceeded = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildPyramidsRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TranslateLayerRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InpLayerPath = table.Column<string>(type: "TEXT", nullable: false),
                    OutLayerPath = table.Column<string>(type: "TEXT", nullable: false),
                    Suceeded = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslateLayerRequests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuildPyramidsRequests");

            migrationBuilder.DropTable(
                name: "TranslateLayerRequests");
        }
    }
}
