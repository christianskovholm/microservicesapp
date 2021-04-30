﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrganizationService.Infrastructure;

namespace OrganizationService.Application.Migrations
{
    [DbContext(typeof(OrganizationDbContext))]
    partial class OrganizationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.HasSequence<int>("organizationseq")
                .IncrementsBy(5);

            modelBuilder.Entity("OrganizationService.Domain.Aggregates.Organization.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("OrganizationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("OrganizationService.Domain.Aggregates.Organization.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<string>("Name")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("RoleId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("OrganizationService.Domain.Aggregates.Organization.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("OrganizationService.Domain.Aggregates.Organization.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("OrganizationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("OrganizationService.Infrastructure.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .UseIdentityColumn();

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("event_type");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int")
                        .HasColumnName("organization_id");

                    b.Property<string>("Payload")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("payload");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2(0)")
                        .HasColumnName("timestamp");

                    b.HasKey("Id");

                    b.ToTable("events");
                });

            modelBuilder.Entity("OrganizationService.Domain.Aggregates.Organization.Department", b =>
                {
                    b.HasOne("OrganizationService.Domain.Aggregates.Organization.Organization", null)
                        .WithMany("Departments")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("OrganizationService.Domain.Aggregates.Organization.Member", b =>
                {
                    b.HasOne("OrganizationService.Domain.Aggregates.Organization.Department", null)
                        .WithMany("Members")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("OrganizationService.Domain.Aggregates.Organization.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("OrganizationService.Domain.Aggregates.Organization.Role", b =>
                {
                    b.HasOne("OrganizationService.Domain.Aggregates.Organization.Organization", null)
                        .WithMany("Roles")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("OrganizationService.Domain.Aggregates.Organization.Department", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("OrganizationService.Domain.Aggregates.Organization.Organization", b =>
                {
                    b.Navigation("Departments");

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
