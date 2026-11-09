using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class AddDictionariesAndAdditionalFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TechOperationGuid",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PesticideGuid",
                table: "Components",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pesticides",
                columns: table => new
                {
                    PesticideGuid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UnitId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pesticides", x => x.PesticideGuid);
                });

            migrationBuilder.CreateTable(
                name: "PlanInventories",
                columns: table => new
                {
                    PlanInventoryGuid = table.Column<Guid>(nullable: false),
                    TechOperationGuid = table.Column<Guid>(nullable: true),
                    EntityGuid = table.Column<Guid>(nullable: true),
                    EntityTypeId = table.Column<int>(nullable: true),
                    PlanAreaHectare = table.Column<double>(nullable: true),
                    Total = table.Column<double>(nullable: true),
                    UnitId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanInventories", x => x.PlanInventoryGuid);
                });

            migrationBuilder.CreateTable(
                name: "TechOperations",
                columns: table => new
                {
                    TechOperationGuid = table.Column<Guid>(nullable: false),
                    FieldGuid = table.Column<Guid>(nullable: true),
                    StructureGuid = table.Column<Guid>(nullable: true),
                    PlanAreaHectare = table.Column<double>(nullable: true),
                    FactAreaHectare = table.Column<double>(nullable: true),
                    SeqId = table.Column<int>(nullable: true),
                    SeasonId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechOperations", x => x.TechOperationGuid);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    UnitId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GroupId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pesticides");

            migrationBuilder.DropTable(
                name: "PlanInventories");

            migrationBuilder.DropTable(
                name: "TechOperations");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropColumn(
                name: "TechOperationGuid",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "PesticideGuid",
                table: "Components");
        }
    }
}
