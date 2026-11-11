using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class AddWithdrawals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Withdrawals",
                columns: table => new
                {
                    WithdrawalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WithdrawalGuid = table.Column<Guid>(nullable: true),
                    WithdrawalExtId = table.Column<string>(nullable: true),
                    TechoperationGuid = table.Column<Guid>(nullable: false),
                    StructureGuid = table.Column<Guid>(nullable: true),
                    FieldGuid = table.Column<Guid>(nullable: true),
                    EntityGuid = table.Column<Guid>(nullable: false),
                    EntityTypeId = table.Column<int>(nullable: false),
                    Total = table.Column<double>(nullable: true),
                    FactAreaHectare = table.Column<double>(nullable: true),
                    UnitId = table.Column<int>(nullable: true),
                    SyncState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Withdrawals", x => x.WithdrawalId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Withdrawals");
        }
    }
}
