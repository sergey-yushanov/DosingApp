using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class CopyComponentsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Copies column values from Components to JobComponents
            migrationBuilder.Sql(@"
                UPDATE JobComponents
                SET 
                    Consistency = (
                        SELECT Consistency 
                        FROM Components 
                        WHERE Components.ComponentId = JobComponents.ComponentId
                    ),
                    Density = (
                        SELECT Density 
                        FROM Components 
                        WHERE Components.ComponentId = JobComponents.ComponentId
                    ),
                    Name = (
                        SELECT Name 
                        FROM Components 
                        WHERE Components.ComponentId = JobComponents.ComponentId
                    )
                WHERE EXISTS (
                    SELECT 1 
                    FROM Components 
                    WHERE Components.ComponentId = JobComponents.ComponentId
                );
            ");

            migrationBuilder.Sql(@"
                UPDATE JobComponents
                SET 
                    ComponentId = NULL;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE JobComponents
                SET
                    Consistency = NULL,
                    Density = NULL,
                    Name = NULL;
            ");
        }
    }
}
