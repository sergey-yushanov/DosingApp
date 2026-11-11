using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class AddPlanInventoryToRecipeComponent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PlanInventoryGuid",
                table: "RecipeComponents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanInventoryGuid",
                table: "RecipeComponents");
        }
    }
}
