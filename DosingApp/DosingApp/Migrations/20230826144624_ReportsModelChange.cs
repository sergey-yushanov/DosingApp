using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class ReportsModelChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Reports",
                newName: "AssignmentName");

            migrationBuilder.AddColumn<string>(
                name: "AssignmentNote",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignmentPlace",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipeName",
                table: "Reports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignmentName",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AssignmentNote",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AssignmentPlace",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "RecipeName",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Reports",
                type: "TEXT",
                nullable: true);
        }
    }
}
