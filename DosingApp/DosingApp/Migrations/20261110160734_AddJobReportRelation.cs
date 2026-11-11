using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class AddJobReportRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUploaded",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Reports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUploaded",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Reports");
        }
    }
}
