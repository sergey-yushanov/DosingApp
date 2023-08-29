﻿// <auto-generated />
using System;
using DosingApp.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DosingApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230829103113_AddMixerVolumeProperty")]
    partial class AddMixerVolumeProperty
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("DosingApp.Models.AgrYear", b =>
                {
                    b.Property<int>("AgrYearId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FinishDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("AgrYearId");

                    b.ToTable("AgrYears");
                });

            modelBuilder.Entity("DosingApp.Models.Applicator", b =>
                {
                    b.Property<int>("ApplicatorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("ApplicatorId");

                    b.ToTable("Applicators");
                });

            modelBuilder.Entity("DosingApp.Models.ApplicatorTank", b =>
                {
                    b.Property<int>("ApplicatorTankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ApplicatorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Volume")
                        .HasColumnType("REAL");

                    b.HasKey("ApplicatorTankId");

                    b.HasIndex("ApplicatorId");

                    b.ToTable("ApplicatorTanks");
                });

            modelBuilder.Entity("DosingApp.Models.Assignment", b =>
                {
                    b.Property<int>("AssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AgrYearId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DestApplicatorId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DestApplicatorTankId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DestFacilityId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DestFacilityTankId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DestTransportId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DestTransportTankId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DestType")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FieldId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDestApplicator")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDestFacility")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDestTransport")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSourceApplicator")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSourceFacility")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSourceTransport")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<string>("Place")
                        .HasColumnType("TEXT");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Size")
                        .HasColumnType("REAL");

                    b.Property<int?>("SourceApplicatorId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SourceApplicatorTankId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SourceFacilityId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SourceFacilityTankId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SourceTransportId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SourceTransportTankId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SourceType")
                        .HasColumnType("TEXT");

                    b.Property<string>("Unit")
                        .HasColumnType("TEXT");

                    b.Property<double?>("VolumeRate")
                        .HasColumnType("REAL");

                    b.HasKey("AssignmentId");

                    b.HasIndex("AgrYearId");

                    b.HasIndex("DestApplicatorId");

                    b.HasIndex("DestApplicatorTankId");

                    b.HasIndex("DestFacilityId");

                    b.HasIndex("DestFacilityTankId");

                    b.HasIndex("DestTransportId");

                    b.HasIndex("DestTransportTankId");

                    b.HasIndex("FieldId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("SourceApplicatorId");

                    b.HasIndex("SourceApplicatorTankId");

                    b.HasIndex("SourceFacilityId");

                    b.HasIndex("SourceFacilityTankId");

                    b.HasIndex("SourceTransportId");

                    b.HasIndex("SourceTransportTankId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("DosingApp.Models.Component", b =>
                {
                    b.Property<int>("ComponentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Consistency")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Density")
                        .HasColumnType("REAL");

                    b.Property<int?>("ManufacturerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Packing")
                        .HasColumnType("TEXT");

                    b.HasKey("ComponentId");

                    b.HasIndex("ManufacturerId");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("DosingApp.Models.Crop", b =>
                {
                    b.Property<int>("CropId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("CropId");

                    b.ToTable("Crops");
                });

            modelBuilder.Entity("DosingApp.Models.Facility", b =>
                {
                    b.Property<int>("FacilityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("FacilityId");

                    b.ToTable("Facilities");
                });

            modelBuilder.Entity("DosingApp.Models.FacilityTank", b =>
                {
                    b.Property<int>("FacilityTankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FacilityId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Volume")
                        .HasColumnType("REAL");

                    b.HasKey("FacilityTankId");

                    b.HasIndex("FacilityId");

                    b.ToTable("FacilityTanks");
                });

            modelBuilder.Entity("DosingApp.Models.Field", b =>
                {
                    b.Property<int>("FieldId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double?>("Size")
                        .HasColumnType("REAL");

                    b.HasKey("FieldId");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("DosingApp.Models.Job", b =>
                {
                    b.Property<int>("JobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AgrYearId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AssignmentId")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AssignmentRemainSize")
                        .HasColumnType("REAL");

                    b.Property<double?>("AssignmentSize")
                        .HasColumnType("REAL");

                    b.Property<int?>("DestApplicatorId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DestApplicatorTankId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DestFacilityId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DestFacilityTankId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DestTransportId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DestTransportTankId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DestType")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FieldId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDestApplicator")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDestFacility")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDestTransport")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSourceApplicator")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSourceFacility")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSourceTransport")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PartyCount")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PartyRemainCount")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("PartySize")
                        .HasColumnType("REAL");

                    b.Property<double?>("PartyVolume")
                        .HasColumnType("REAL");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SourceApplicatorId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SourceApplicatorTankId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SourceFacilityId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SourceFacilityTankId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SourceTransportId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SourceTransportTankId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SourceType")
                        .HasColumnType("TEXT");

                    b.Property<string>("Unit")
                        .HasColumnType("TEXT");

                    b.Property<double?>("VolumeRate")
                        .HasColumnType("REAL");

                    b.HasKey("JobId");

                    b.HasIndex("AgrYearId");

                    b.HasIndex("AssignmentId");

                    b.HasIndex("DestApplicatorId");

                    b.HasIndex("DestApplicatorTankId");

                    b.HasIndex("DestFacilityId");

                    b.HasIndex("DestFacilityTankId");

                    b.HasIndex("DestTransportId");

                    b.HasIndex("DestTransportTankId");

                    b.HasIndex("FieldId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("SourceApplicatorId");

                    b.HasIndex("SourceApplicatorTankId");

                    b.HasIndex("SourceFacilityId");

                    b.HasIndex("SourceFacilityTankId");

                    b.HasIndex("SourceTransportId");

                    b.HasIndex("SourceTransportTankId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("DosingApp.Models.JobComponent", b =>
                {
                    b.Property<int>("JobComponentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ComponentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Dispenser")
                        .HasColumnType("TEXT");

                    b.Property<int?>("JobId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Volume")
                        .HasColumnType("REAL");

                    b.Property<double?>("VolumeRate")
                        .HasColumnType("REAL");

                    b.Property<string>("VolumeRateUnit")
                        .HasColumnType("TEXT");

                    b.Property<string>("VolumeUnit")
                        .HasColumnType("TEXT");

                    b.HasKey("JobComponentId");

                    b.HasIndex("ComponentId");

                    b.HasIndex("JobId");

                    b.ToTable("JobComponents");
                });

            modelBuilder.Entity("DosingApp.Models.Manufacturer", b =>
                {
                    b.Property<int>("ManufacturerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("ManufacturerId");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("DosingApp.Models.Mixer", b =>
                {
                    b.Property<int>("MixerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Collector")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsUsedMixer")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Single")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ThreeWay")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Volume")
                        .HasColumnType("INTEGER");

                    b.HasKey("MixerId");

                    b.ToTable("Mixers");
                });

            modelBuilder.Entity("DosingApp.Models.ProcessingType", b =>
                {
                    b.Property<int>("ProcessingTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("ProcessingTypeId");

                    b.ToTable("ProcessingTypes");
                });

            modelBuilder.Entity("DosingApp.Models.Recipe", b =>
                {
                    b.Property<int>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CarrierId")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("CarrierReserve")
                        .HasColumnType("REAL");

                    b.Property<int?>("CropId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ProcessingTypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RecipeId");

                    b.HasIndex("CarrierId");

                    b.HasIndex("CropId");

                    b.HasIndex("ProcessingTypeId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("DosingApp.Models.RecipeComponent", b =>
                {
                    b.Property<int>("RecipeComponentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ComponentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Dispenser")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("VolumeRate")
                        .HasColumnType("REAL");

                    b.Property<string>("VolumeRateUnit")
                        .HasColumnType("TEXT");

                    b.HasKey("RecipeComponentId");

                    b.HasIndex("ComponentId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeComponents");
                });

            modelBuilder.Entity("DosingApp.Models.Report", b =>
                {
                    b.Property<int>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AssignmentName")
                        .HasColumnType("TEXT");

                    b.Property<string>("AssignmentNote")
                        .HasColumnType("TEXT");

                    b.Property<string>("AssignmentPlace")
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("DriverName")
                        .HasColumnType("TEXT");

                    b.Property<string>("OperatorName")
                        .HasColumnType("TEXT");

                    b.Property<string>("RecipeName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ReportDateTime")
                        .HasColumnType("TEXT");

                    b.HasKey("ReportId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("DosingApp.Models.ReportComponent", b =>
                {
                    b.Property<int>("ReportComponentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Dispenser")
                        .HasColumnType("TEXT");

                    b.Property<double?>("DosedVolume")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ReportId")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("RequiredVolume")
                        .HasColumnType("REAL");

                    b.HasKey("ReportComponentId");

                    b.HasIndex("ReportId");

                    b.ToTable("ReportComponents");
                });

            modelBuilder.Entity("DosingApp.Models.Transport", b =>
                {
                    b.Property<int>("TransportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Number")
                        .HasColumnType("TEXT");

                    b.HasKey("TransportId");

                    b.ToTable("Transports");
                });

            modelBuilder.Entity("DosingApp.Models.TransportTank", b =>
                {
                    b.Property<int>("TransportTankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TransportId")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Volume")
                        .HasColumnType("REAL");

                    b.HasKey("TransportTankId");

                    b.HasIndex("TransportId");

                    b.ToTable("TransportTanks");
                });

            modelBuilder.Entity("DosingApp.Models.ApplicatorTank", b =>
                {
                    b.HasOne("DosingApp.Models.Applicator", "Applicator")
                        .WithMany("ApplicatorTanks")
                        .HasForeignKey("ApplicatorId");
                });

            modelBuilder.Entity("DosingApp.Models.Assignment", b =>
                {
                    b.HasOne("DosingApp.Models.AgrYear", "AgrYear")
                        .WithMany()
                        .HasForeignKey("AgrYearId");

                    b.HasOne("DosingApp.Models.Applicator", "DestApplicator")
                        .WithMany()
                        .HasForeignKey("DestApplicatorId");

                    b.HasOne("DosingApp.Models.ApplicatorTank", "DestApplicatorTank")
                        .WithMany()
                        .HasForeignKey("DestApplicatorTankId");

                    b.HasOne("DosingApp.Models.Facility", "DestFacility")
                        .WithMany()
                        .HasForeignKey("DestFacilityId");

                    b.HasOne("DosingApp.Models.FacilityTank", "DestFacilityTank")
                        .WithMany()
                        .HasForeignKey("DestFacilityTankId");

                    b.HasOne("DosingApp.Models.Transport", "DestTransport")
                        .WithMany()
                        .HasForeignKey("DestTransportId");

                    b.HasOne("DosingApp.Models.TransportTank", "DestTransportTank")
                        .WithMany()
                        .HasForeignKey("DestTransportTankId");

                    b.HasOne("DosingApp.Models.Field", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId");

                    b.HasOne("DosingApp.Models.Recipe", "Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeId");

                    b.HasOne("DosingApp.Models.Applicator", "SourceApplicator")
                        .WithMany()
                        .HasForeignKey("SourceApplicatorId");

                    b.HasOne("DosingApp.Models.ApplicatorTank", "SourceApplicatorTank")
                        .WithMany()
                        .HasForeignKey("SourceApplicatorTankId");

                    b.HasOne("DosingApp.Models.Facility", "SourceFacility")
                        .WithMany()
                        .HasForeignKey("SourceFacilityId");

                    b.HasOne("DosingApp.Models.FacilityTank", "SourceFacilityTank")
                        .WithMany()
                        .HasForeignKey("SourceFacilityTankId");

                    b.HasOne("DosingApp.Models.Transport", "SourceTransport")
                        .WithMany()
                        .HasForeignKey("SourceTransportId");

                    b.HasOne("DosingApp.Models.TransportTank", "SourceTransportTank")
                        .WithMany()
                        .HasForeignKey("SourceTransportTankId");
                });

            modelBuilder.Entity("DosingApp.Models.Component", b =>
                {
                    b.HasOne("DosingApp.Models.Manufacturer", "Manufacturer")
                        .WithMany("Components")
                        .HasForeignKey("ManufacturerId");
                });

            modelBuilder.Entity("DosingApp.Models.FacilityTank", b =>
                {
                    b.HasOne("DosingApp.Models.Facility", "Facility")
                        .WithMany("FacilityTanks")
                        .HasForeignKey("FacilityId");
                });

            modelBuilder.Entity("DosingApp.Models.Job", b =>
                {
                    b.HasOne("DosingApp.Models.AgrYear", "AgrYear")
                        .WithMany()
                        .HasForeignKey("AgrYearId");

                    b.HasOne("DosingApp.Models.Assignment", "Assignment")
                        .WithMany()
                        .HasForeignKey("AssignmentId");

                    b.HasOne("DosingApp.Models.Applicator", "DestApplicator")
                        .WithMany()
                        .HasForeignKey("DestApplicatorId");

                    b.HasOne("DosingApp.Models.ApplicatorTank", "DestApplicatorTank")
                        .WithMany()
                        .HasForeignKey("DestApplicatorTankId");

                    b.HasOne("DosingApp.Models.Facility", "DestFacility")
                        .WithMany()
                        .HasForeignKey("DestFacilityId");

                    b.HasOne("DosingApp.Models.FacilityTank", "DestFacilityTank")
                        .WithMany()
                        .HasForeignKey("DestFacilityTankId");

                    b.HasOne("DosingApp.Models.Transport", "DestTransport")
                        .WithMany()
                        .HasForeignKey("DestTransportId");

                    b.HasOne("DosingApp.Models.TransportTank", "DestTransportTank")
                        .WithMany()
                        .HasForeignKey("DestTransportTankId");

                    b.HasOne("DosingApp.Models.Field", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId");

                    b.HasOne("DosingApp.Models.Recipe", "Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeId");

                    b.HasOne("DosingApp.Models.Applicator", "SourceApplicator")
                        .WithMany()
                        .HasForeignKey("SourceApplicatorId");

                    b.HasOne("DosingApp.Models.ApplicatorTank", "SourceApplicatorTank")
                        .WithMany()
                        .HasForeignKey("SourceApplicatorTankId");

                    b.HasOne("DosingApp.Models.Facility", "SourceFacility")
                        .WithMany()
                        .HasForeignKey("SourceFacilityId");

                    b.HasOne("DosingApp.Models.FacilityTank", "SourceFacilityTank")
                        .WithMany()
                        .HasForeignKey("SourceFacilityTankId");

                    b.HasOne("DosingApp.Models.Transport", "SourceTransport")
                        .WithMany()
                        .HasForeignKey("SourceTransportId");

                    b.HasOne("DosingApp.Models.TransportTank", "SourceTransportTank")
                        .WithMany()
                        .HasForeignKey("SourceTransportTankId");
                });

            modelBuilder.Entity("DosingApp.Models.JobComponent", b =>
                {
                    b.HasOne("DosingApp.Models.Component", "Component")
                        .WithMany()
                        .HasForeignKey("ComponentId");

                    b.HasOne("DosingApp.Models.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobId");
                });

            modelBuilder.Entity("DosingApp.Models.Recipe", b =>
                {
                    b.HasOne("DosingApp.Models.Component", "Carrier")
                        .WithMany()
                        .HasForeignKey("CarrierId");

                    b.HasOne("DosingApp.Models.Crop", "Crop")
                        .WithMany("Recipes")
                        .HasForeignKey("CropId");

                    b.HasOne("DosingApp.Models.ProcessingType", "ProcessingType")
                        .WithMany()
                        .HasForeignKey("ProcessingTypeId");
                });

            modelBuilder.Entity("DosingApp.Models.RecipeComponent", b =>
                {
                    b.HasOne("DosingApp.Models.Component", "Component")
                        .WithMany()
                        .HasForeignKey("ComponentId");

                    b.HasOne("DosingApp.Models.Recipe", "Recipe")
                        .WithMany("RecipeComponents")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("DosingApp.Models.ReportComponent", b =>
                {
                    b.HasOne("DosingApp.Models.Report", "Report")
                        .WithMany("ReportComponents")
                        .HasForeignKey("ReportId");
                });

            modelBuilder.Entity("DosingApp.Models.TransportTank", b =>
                {
                    b.HasOne("DosingApp.Models.Transport", "Transport")
                        .WithMany("TransportTanks")
                        .HasForeignKey("TransportId");
                });
#pragma warning restore 612, 618
        }
    }
}
