using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class AddJobComponentsFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Consistency",
                table: "JobComponents",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Density",
                table: "JobComponents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "JobComponents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Consistency",
                table: "JobComponents");

            migrationBuilder.DropColumn(
                name: "Density",
                table: "JobComponents");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "JobComponents");
        }
    }
}
