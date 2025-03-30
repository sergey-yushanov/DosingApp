using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class RenameCollectorValves : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE JobComponents SET Dispenser = (substr(Dispenser, 1, 1) || 'К' || substr(Dispenser, 5, 1) || 'Кл') WHERE Dispenser LIKE '%Кол%';\n" +
                "UPDATE RecipeComponents SET Dispenser = (substr(Dispenser, 1, 1) || 'К' || substr(Dispenser, 5, 1) || 'Кл') WHERE Dispenser LIKE '%Кол%';\n" +
                "UPDATE ReportComponents SET Dispenser = (substr(Dispenser, 1, 1) || 'К' || substr(Dispenser, 5, 1) || 'Кл') WHERE Dispenser LIKE '%Кол%';"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
