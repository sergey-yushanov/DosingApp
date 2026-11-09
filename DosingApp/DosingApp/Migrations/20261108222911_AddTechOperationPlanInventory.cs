using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class AddTechOperationPlanInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanInventories");

            migrationBuilder.DropTable(
                name: "TechOperations");
        }
    }
}
