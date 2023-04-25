using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WasteDetection.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComputeImageStatisticsRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InpImgPath = table.Column<string>(type: "TEXT", nullable: false),
                    OutXmlPath = table.Column<string>(type: "TEXT", nullable: false),
                    Suceeded = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputeImageStatisticsRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageClassificationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InpImgPath = table.Column<string>(type: "TEXT", nullable: false),
                    InpModelPath = table.Column<string>(type: "TEXT", nullable: false),
                    InpXmlStatisticsPath = table.Column<string>(type: "TEXT", nullable: false),
                    OutRasterPath = table.Column<string>(type: "TEXT", nullable: false),
                    OutConfidenceMapPath = table.Column<string>(type: "TEXT", nullable: false),
                    Suceeded = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageClassificationRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainImageClassificatierRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InpImgPath = table.Column<string>(type: "TEXT", nullable: false),
                    InpVectorPath = table.Column<string>(type: "TEXT", nullable: false),
                    ValidationVectorPath = table.Column<string>(type: "TEXT", nullable: false),
                    InpXmlStatisticsPath = table.Column<string>(type: "TEXT", nullable: false),
                    OutModelPath = table.Column<string>(type: "TEXT", nullable: false),
                    OutConfusionMatrixPath = table.Column<string>(type: "TEXT", nullable: false),
                    LabelField = table.Column<string>(type: "TEXT", nullable: false),
                    TrainingClassifierName = table.Column<string>(type: "TEXT", nullable: false),
                    Suceeded = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainImageClassificatierRequests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComputeImageStatisticsRequests");

            migrationBuilder.DropTable(
                name: "ImageClassificationRequests");

            migrationBuilder.DropTable(
                name: "TrainImageClassificatierRequests");
        }
    }
}
