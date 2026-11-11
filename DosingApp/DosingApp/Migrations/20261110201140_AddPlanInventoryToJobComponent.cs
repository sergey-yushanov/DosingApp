using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class AddPlanInventoryToJobComponent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PlanInventoryGuid",
                table: "JobComponents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanInventoryGuid",
                table: "JobComponents");
        }
    }
}
