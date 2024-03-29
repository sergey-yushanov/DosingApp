﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class AddReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReportDateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                });

            migrationBuilder.CreateTable(
                name: "ReportComponents",
                columns: table => new
                {
                    ReportComponentId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReportId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Dispenser = table.Column<string>(nullable: true),
                    RequiredVolume = table.Column<double>(nullable: true),
                    DosedVolume = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportComponents", x => x.ReportComponentId);
                    table.ForeignKey(
                        name: "FK_ReportComponents_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportComponents_ReportId",
                table: "ReportComponents",
                column: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportComponents");

            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
