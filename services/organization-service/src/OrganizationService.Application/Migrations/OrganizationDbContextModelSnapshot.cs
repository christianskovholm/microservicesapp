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
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("Relational:Sequence:.organizationseq", "'organizationseq', '', '1', '5', '', '', 'Int32', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OrganizationService.Domain.Aggregates.Organization.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

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
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

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
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("OrganizationService.Domain.Aggregates.Organization.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("DateTimeOffset(0)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

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
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnName("event_type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrganizationId")
                        .HasColumnName("organization_id")
                        .HasColumnType("int");

                    b.Property<string>("Payload")
                        .IsRequired()
                        .HasColumnName("payload")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnName("timestamp")
                        .HasColumnType("datetime2(0)");

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
                });

            modelBuilder.Entity("OrganizationService.Domain.Aggregates.Organization.Role", b =>
                {
                    b.HasOne("OrganizationService.Domain.Aggregates.Organization.Organization", null)
                        .WithMany("Roles")
                        .HasForeignKey("OrganizationId");
                });
#pragma warning restore 612, 618
        }
    }
}
