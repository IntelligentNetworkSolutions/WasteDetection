using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WasteDetection.Migrations
{
    /// <inheritdoc />
    public partial class AddPolygonizeRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContourRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InpLayerPath = table.Column<string>(type: "TEXT", nullable: false),
                    OutContoursPath = table.Column<string>(type: "TEXT", nullable: false),
                    Suceeded = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContourRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PolygonizeRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InpLayerPath = table.Column<string>(type: "TEXT", nullable: false),
                    OutVectorizedPath = table.Column<string>(type: "TEXT", nullable: false),
                    Suceeded = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolygonizeRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RasterCalculatorRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InpLayerAPath = table.Column<string>(type: "TEXT", nullable: false),
                    InpLayerBPath = table.Column<string>(type: "TEXT", nullable: false),
                    OutLayerPath = table.Column<string>(type: "TEXT", nullable: false),
                    Suceeded = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RasterCalculatorRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SieveRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InpLayerPath = table.Column<string>(type: "TEXT", nullable: false),
                    OutSievedPath = table.Column<string>(type: "TEXT", nullable: false),
                    Suceeded = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SieveRequests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContourRequests");

            migrationBuilder.DropTable(
                name: "PolygonizeRequests");

            migrationBuilder.DropTable(
                name: "RasterCalculatorRequests");

            migrationBuilder.DropTable(
                name: "SieveRequests");
        }
    }
}
