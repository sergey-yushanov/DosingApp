using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class AddPropertiesToReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VolumeRate",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessedArea",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DosingTime",
                table: "Reports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VolumeRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ProcessedArea",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "DosingTime",
                table: "Reports");
        }
    }
}
