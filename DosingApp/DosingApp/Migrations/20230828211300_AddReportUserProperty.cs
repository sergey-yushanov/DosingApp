using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class AddReportUserProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatorName",
                table: "Reports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "OperatorName",
                table: "Reports");
        }
    }
}
