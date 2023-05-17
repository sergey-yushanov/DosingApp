using Microsoft.EntityFrameworkCore.Migrations;

namespace DosingApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgrYears",
                columns: table => new
                {
                    AgrYearId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    FinishDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgrYears", x => x.AgrYearId);
                });

            migrationBuilder.CreateTable(
                name: "Applicators",
                columns: table => new
                {
                    ApplicatorId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicators", x => x.ApplicatorId);
                });

            migrationBuilder.CreateTable(
                name: "Crops",
                columns: table => new
                {
                    CropId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crops", x => x.CropId);
                });

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    FacilityId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.FacilityId);
                });

            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    FieldId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Size = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.FieldId);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    ManufacturerId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.ManufacturerId);
                });

            migrationBuilder.CreateTable(
                name: "Mixers",
                columns: table => new
                {
                    MixerId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Collector = table.Column<int>(nullable: true),
                    Single = table.Column<int>(nullable: true),
                    ThreeWay = table.Column<int>(nullable: true),
                    IsUsedMixer = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mixers", x => x.MixerId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingTypes",
                columns: table => new
                {
                    ProcessingTypeId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingTypes", x => x.ProcessingTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Transports",
                columns: table => new
                {
                    TransportId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transports", x => x.TransportId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicatorTanks",
                columns: table => new
                {
                    ApplicatorTankId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Volume = table.Column<double>(nullable: true),
                    ApplicatorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicatorTanks", x => x.ApplicatorTankId);
                    table.ForeignKey(
                        name: "FK_ApplicatorTanks_Applicators_ApplicatorId",
                        column: x => x.ApplicatorId,
                        principalTable: "Applicators",
                        principalColumn: "ApplicatorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FacilityTanks",
                columns: table => new
                {
                    FacilityTankId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Volume = table.Column<double>(nullable: true),
                    FacilityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityTanks", x => x.FacilityTankId);
                    table.ForeignKey(
                        name: "FK_FacilityTanks_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    ComponentId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ManufacturerId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Consistency = table.Column<string>(nullable: true),
                    Density = table.Column<double>(nullable: true),
                    Packing = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Components_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "ManufacturerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportTanks",
                columns: table => new
                {
                    TransportTankId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Volume = table.Column<double>(nullable: true),
                    TransportId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportTanks", x => x.TransportTankId);
                    table.ForeignKey(
                        name: "FK_TransportTanks_Transports_TransportId",
                        column: x => x.TransportId,
                        principalTable: "Transports",
                        principalColumn: "TransportId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    CropId = table.Column<int>(nullable: true),
                    ProcessingTypeId = table.Column<int>(nullable: true),
                    CarrierId = table.Column<int>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    CarrierReserve = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_Recipes_Components_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "Components",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recipes_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "CropId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recipes_ProcessingTypes_ProcessingTypeId",
                        column: x => x.ProcessingTypeId,
                        principalTable: "ProcessingTypes",
                        principalColumn: "ProcessingTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    RecipeId = table.Column<int>(nullable: true),
                    SourceType = table.Column<string>(nullable: true),
                    DestType = table.Column<string>(nullable: true),
                    IsSourceFacility = table.Column<bool>(nullable: false),
                    SourceFacilityId = table.Column<int>(nullable: true),
                    SourceFacilityTankId = table.Column<int>(nullable: true),
                    IsSourceTransport = table.Column<bool>(nullable: false),
                    SourceTransportId = table.Column<int>(nullable: true),
                    SourceTransportTankId = table.Column<int>(nullable: true),
                    IsSourceApplicator = table.Column<bool>(nullable: false),
                    SourceApplicatorId = table.Column<int>(nullable: true),
                    SourceApplicatorTankId = table.Column<int>(nullable: true),
                    IsDestFacility = table.Column<bool>(nullable: false),
                    DestFacilityId = table.Column<int>(nullable: true),
                    DestFacilityTankId = table.Column<int>(nullable: true),
                    IsDestTransport = table.Column<bool>(nullable: false),
                    DestTransportId = table.Column<int>(nullable: true),
                    DestTransportTankId = table.Column<int>(nullable: true),
                    IsDestApplicator = table.Column<bool>(nullable: false),
                    DestApplicatorId = table.Column<int>(nullable: true),
                    DestApplicatorTankId = table.Column<int>(nullable: true),
                    AgrYearId = table.Column<int>(nullable: true),
                    FieldId = table.Column<int>(nullable: true),
                    Place = table.Column<string>(nullable: true),
                    VolumeRate = table.Column<double>(nullable: true),
                    Size = table.Column<double>(nullable: true),
                    Unit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_Assignments_AgrYears_AgrYearId",
                        column: x => x.AgrYearId,
                        principalTable: "AgrYears",
                        principalColumn: "AgrYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Applicators_DestApplicatorId",
                        column: x => x.DestApplicatorId,
                        principalTable: "Applicators",
                        principalColumn: "ApplicatorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_ApplicatorTanks_DestApplicatorTankId",
                        column: x => x.DestApplicatorTankId,
                        principalTable: "ApplicatorTanks",
                        principalColumn: "ApplicatorTankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Facilities_DestFacilityId",
                        column: x => x.DestFacilityId,
                        principalTable: "Facilities",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_FacilityTanks_DestFacilityTankId",
                        column: x => x.DestFacilityTankId,
                        principalTable: "FacilityTanks",
                        principalColumn: "FacilityTankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Transports_DestTransportId",
                        column: x => x.DestTransportId,
                        principalTable: "Transports",
                        principalColumn: "TransportId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_TransportTanks_DestTransportTankId",
                        column: x => x.DestTransportTankId,
                        principalTable: "TransportTanks",
                        principalColumn: "TransportTankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "FieldId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Applicators_SourceApplicatorId",
                        column: x => x.SourceApplicatorId,
                        principalTable: "Applicators",
                        principalColumn: "ApplicatorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_ApplicatorTanks_SourceApplicatorTankId",
                        column: x => x.SourceApplicatorTankId,
                        principalTable: "ApplicatorTanks",
                        principalColumn: "ApplicatorTankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Facilities_SourceFacilityId",
                        column: x => x.SourceFacilityId,
                        principalTable: "Facilities",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_FacilityTanks_SourceFacilityTankId",
                        column: x => x.SourceFacilityTankId,
                        principalTable: "FacilityTanks",
                        principalColumn: "FacilityTankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Transports_SourceTransportId",
                        column: x => x.SourceTransportId,
                        principalTable: "Transports",
                        principalColumn: "TransportId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_TransportTanks_SourceTransportTankId",
                        column: x => x.SourceTransportTankId,
                        principalTable: "TransportTanks",
                        principalColumn: "TransportTankId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeComponents",
                columns: table => new
                {
                    RecipeComponentId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<int>(nullable: true),
                    ComponentId = table.Column<int>(nullable: true),
                    Order = table.Column<int>(nullable: true),
                    VolumeRate = table.Column<double>(nullable: true),
                    VolumeRateUnit = table.Column<string>(nullable: true),
                    Dispenser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeComponents", x => x.RecipeComponentId);
                    table.ForeignKey(
                        name: "FK_RecipeComponents_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeComponents_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssignmentId = table.Column<int>(nullable: true),
                    AssignmentSize = table.Column<double>(nullable: true),
                    AssignmentRemainSize = table.Column<double>(nullable: true),
                    PartySize = table.Column<double>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    PartyCount = table.Column<int>(nullable: true),
                    PartyRemainCount = table.Column<int>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    RecipeId = table.Column<int>(nullable: true),
                    SourceType = table.Column<string>(nullable: true),
                    DestType = table.Column<string>(nullable: true),
                    IsSourceFacility = table.Column<bool>(nullable: false),
                    SourceFacilityId = table.Column<int>(nullable: true),
                    SourceFacilityTankId = table.Column<int>(nullable: true),
                    IsSourceTransport = table.Column<bool>(nullable: false),
                    SourceTransportId = table.Column<int>(nullable: true),
                    SourceTransportTankId = table.Column<int>(nullable: true),
                    IsSourceApplicator = table.Column<bool>(nullable: false),
                    SourceApplicatorId = table.Column<int>(nullable: true),
                    SourceApplicatorTankId = table.Column<int>(nullable: true),
                    IsDestFacility = table.Column<bool>(nullable: false),
                    DestFacilityId = table.Column<int>(nullable: true),
                    DestFacilityTankId = table.Column<int>(nullable: true),
                    IsDestTransport = table.Column<bool>(nullable: false),
                    DestTransportId = table.Column<int>(nullable: true),
                    DestTransportTankId = table.Column<int>(nullable: true),
                    IsDestApplicator = table.Column<bool>(nullable: false),
                    DestApplicatorId = table.Column<int>(nullable: true),
                    DestApplicatorTankId = table.Column<int>(nullable: true),
                    AgrYearId = table.Column<int>(nullable: true),
                    FieldId = table.Column<int>(nullable: true),
                    PartyVolume = table.Column<double>(nullable: true),
                    VolumeRate = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_Jobs_AgrYears_AgrYearId",
                        column: x => x.AgrYearId,
                        principalTable: "AgrYears",
                        principalColumn: "AgrYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_Applicators_DestApplicatorId",
                        column: x => x.DestApplicatorId,
                        principalTable: "Applicators",
                        principalColumn: "ApplicatorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_ApplicatorTanks_DestApplicatorTankId",
                        column: x => x.DestApplicatorTankId,
                        principalTable: "ApplicatorTanks",
                        principalColumn: "ApplicatorTankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_Facilities_DestFacilityId",
                        column: x => x.DestFacilityId,
                        principalTable: "Facilities",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_FacilityTanks_DestFacilityTankId",
                        column: x => x.DestFacilityTankId,
                        principalTable: "FacilityTanks",
                        principalColumn: "FacilityTankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_Transports_DestTransportId",
                        column: x => x.DestTransportId,
                        principalTable: "Transports",
                        principalColumn: "TransportId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_TransportTanks_DestTransportTankId",
                        column: x => x.DestTransportTankId,
                        principalTable: "TransportTanks",
                        principalColumn: "TransportTankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "FieldId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_Applicators_SourceApplicatorId",
                        column: x => x.SourceApplicatorId,
                        principalTable: "Applicators",
                        principalColumn: "ApplicatorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_ApplicatorTanks_SourceApplicatorTankId",
                        column: x => x.SourceApplicatorTankId,
                        principalTable: "ApplicatorTanks",
                        principalColumn: "ApplicatorTankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_Facilities_SourceFacilityId",
                        column: x => x.SourceFacilityId,
                        principalTable: "Facilities",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_FacilityTanks_SourceFacilityTankId",
                        column: x => x.SourceFacilityTankId,
                        principalTable: "FacilityTanks",
                        principalColumn: "FacilityTankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_Transports_SourceTransportId",
                        column: x => x.SourceTransportId,
                        principalTable: "Transports",
                        principalColumn: "TransportId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_TransportTanks_SourceTransportTankId",
                        column: x => x.SourceTransportTankId,
                        principalTable: "TransportTanks",
                        principalColumn: "TransportTankId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobComponents",
                columns: table => new
                {
                    JobComponentId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobId = table.Column<int>(nullable: true),
                    ComponentId = table.Column<int>(nullable: true),
                    Order = table.Column<int>(nullable: true),
                    Volume = table.Column<double>(nullable: true),
                    VolumeRate = table.Column<double>(nullable: true),
                    VolumeUnit = table.Column<string>(nullable: true),
                    VolumeRateUnit = table.Column<string>(nullable: true),
                    Dispenser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobComponents", x => x.JobComponentId);
                    table.ForeignKey(
                        name: "FK_JobComponents_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobComponents_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicatorTanks_ApplicatorId",
                table: "ApplicatorTanks",
                column: "ApplicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AgrYearId",
                table: "Assignments",
                column: "AgrYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_DestApplicatorId",
                table: "Assignments",
                column: "DestApplicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_DestApplicatorTankId",
                table: "Assignments",
                column: "DestApplicatorTankId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_DestFacilityId",
                table: "Assignments",
                column: "DestFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_DestFacilityTankId",
                table: "Assignments",
                column: "DestFacilityTankId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_DestTransportId",
                table: "Assignments",
                column: "DestTransportId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_DestTransportTankId",
                table: "Assignments",
                column: "DestTransportTankId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_FieldId",
                table: "Assignments",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_RecipeId",
                table: "Assignments",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SourceApplicatorId",
                table: "Assignments",
                column: "SourceApplicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SourceApplicatorTankId",
                table: "Assignments",
                column: "SourceApplicatorTankId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SourceFacilityId",
                table: "Assignments",
                column: "SourceFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SourceFacilityTankId",
                table: "Assignments",
                column: "SourceFacilityTankId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SourceTransportId",
                table: "Assignments",
                column: "SourceTransportId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SourceTransportTankId",
                table: "Assignments",
                column: "SourceTransportTankId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_ManufacturerId",
                table: "Components",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityTanks_FacilityId",
                table: "FacilityTanks",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_JobComponents_ComponentId",
                table: "JobComponents",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_JobComponents_JobId",
                table: "JobComponents",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_AgrYearId",
                table: "Jobs",
                column: "AgrYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_AssignmentId",
                table: "Jobs",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_DestApplicatorId",
                table: "Jobs",
                column: "DestApplicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_DestApplicatorTankId",
                table: "Jobs",
                column: "DestApplicatorTankId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_DestFacilityId",
                table: "Jobs",
                column: "DestFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_DestFacilityTankId",
                table: "Jobs",
                column: "DestFacilityTankId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_DestTransportId",
                table: "Jobs",
                column: "DestTransportId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_DestTransportTankId",
                table: "Jobs",
                column: "DestTransportTankId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_FieldId",
                table: "Jobs",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_RecipeId",
                table: "Jobs",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_SourceApplicatorId",
                table: "Jobs",
                column: "SourceApplicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_SourceApplicatorTankId",
                table: "Jobs",
                column: "SourceApplicatorTankId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_SourceFacilityId",
                table: "Jobs",
                column: "SourceFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_SourceFacilityTankId",
                table: "Jobs",
                column: "SourceFacilityTankId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_SourceTransportId",
                table: "Jobs",
                column: "SourceTransportId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_SourceTransportTankId",
                table: "Jobs",
                column: "SourceTransportTankId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeComponents_ComponentId",
                table: "RecipeComponents",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeComponents_RecipeId",
                table: "RecipeComponents",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CarrierId",
                table: "Recipes",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CropId",
                table: "Recipes",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ProcessingTypeId",
                table: "Recipes",
                column: "ProcessingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportTanks_TransportId",
                table: "TransportTanks",
                column: "TransportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobComponents");

            migrationBuilder.DropTable(
                name: "Mixers");

            migrationBuilder.DropTable(
                name: "RecipeComponents");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "AgrYears");

            migrationBuilder.DropTable(
                name: "ApplicatorTanks");

            migrationBuilder.DropTable(
                name: "FacilityTanks");

            migrationBuilder.DropTable(
                name: "TransportTanks");

            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Applicators");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "Transports");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "Crops");

            migrationBuilder.DropTable(
                name: "ProcessingTypes");

            migrationBuilder.DropTable(
                name: "Manufacturers");
        }
    }
}
