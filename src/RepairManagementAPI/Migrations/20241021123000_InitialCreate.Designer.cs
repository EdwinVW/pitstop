﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pitstop.RepairManagementAPI.DataAccess;

#nullable disable

namespace Pitstop.RepairManagementAPI.Migrations
{
    [DbContext(typeof(RepairManagementContext))]
    [Migration("20241021123000_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Pitstop.RepairManagementAPI.Model.Customer", b =>
                {
                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelephoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("Pitstop.RepairManagementAPI.Model.RepairOrders", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<string>("LaborCost")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicenseNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RejectReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("VehiclePartId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RepairOrders", (string)null);
                });

            modelBuilder.Entity("Pitstop.RepairManagementAPI.Model.Vehicle", b =>
                {
                    b.Property<string>("LicenseNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Brand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LicenseNumber");

                    b.ToTable("Vehicle", (string)null);
                });

            modelBuilder.Entity("Pitstop.RepairManagementAPI.Model.VehicleParts", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VehicleParts", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("e831584d-b318-4b57-b436-aa934a06e8d6"),
                            Cost = 1200.00m,
                            Name = "Engine"
                        },
                        new
                        {
                            Id = new Guid("ab99586f-a421-48f7-83b5-f9ec47aeac7b"),
                            Cost = 300.00m,
                            Name = "Brake Pads"
                        },
                        new
                        {
                            Id = new Guid("a2bc1f12-c90d-4780-b9a2-6eb14f955610"),
                            Cost = 2400.00m,
                            Name = "Transmission"
                        },
                        new
                        {
                            Id = new Guid("f9284ca5-f36b-4e57-90dd-417598d7ae86"),
                            Cost = 600.00m,
                            Name = "Fuel Pump"
                        },
                        new
                        {
                            Id = new Guid("bff55876-b954-495a-8871-56887f547032"),
                            Cost = 450.00m,
                            Name = "Alternator"
                        },
                        new
                        {
                            Id = new Guid("8549b861-ef1b-448e-b791-e6176cd95386"),
                            Cost = 250.00m,
                            Name = "Radiator"
                        },
                        new
                        {
                            Id = new Guid("486e09f9-da8b-4577-ba57-fe75990d6f84"),
                            Cost = 500.00m,
                            Name = "Exhaust"
                        },
                        new
                        {
                            Id = new Guid("6df99364-466b-4e88-9b9e-dfddc572f06b"),
                            Cost = 800.00m,
                            Name = "Suspension"
                        },
                        new
                        {
                            Id = new Guid("a81c2242-1340-48a7-81f3-2700114fa520"),
                            Cost = 200.00m,
                            Name = "Battery"
                        },
                        new
                        {
                            Id = new Guid("bfd00e1b-e2a9-4c1b-ae3e-837f8c7b1c9a"),
                            Cost = 400.00m,
                            Name = "Starter Motor"
                        },
                        new
                        {
                            Id = new Guid("e6d590cd-d4d8-4cd8-9a08-7a754d12a7de"),
                            Cost = 150.00m,
                            Name = "Headlights"
                        },
                        new
                        {
                            Id = new Guid("e2efd403-fe51-46d4-966e-90b0430c0b4e"),
                            Cost = 120.00m,
                            Name = "Taillights"
                        },
                        new
                        {
                            Id = new Guid("56947ee4-b977-4bcc-85f4-9eba1ed63940"),
                            Cost = 75.00m,
                            Name = "Spark Plugs"
                        },
                        new
                        {
                            Id = new Guid("5280ec2b-c0c1-45c9-8916-e439b8f557d4"),
                            Cost = 850.00m,
                            Name = "Clutch"
                        },
                        new
                        {
                            Id = new Guid("ee01a952-6f23-4479-a76b-8cc92e5ea26f"),
                            Cost = 35.00m,
                            Name = "Air Filter"
                        },
                        new
                        {
                            Id = new Guid("0d66b7e2-c228-4379-87e0-25196049e6fd"),
                            Cost = 25.00m,
                            Name = "Oil Filter"
                        },
                        new
                        {
                            Id = new Guid("bc37223d-2fad-4eec-b8fc-134e00da097a"),
                            Cost = 320.00m,
                            Name = "Timing Belt"
                        },
                        new
                        {
                            Id = new Guid("810dcdf3-fd00-4d39-8132-8a0c393edd8a"),
                            Cost = 400.00m,
                            Name = "Water Pump"
                        },
                        new
                        {
                            Id = new Guid("65ba4299-05dd-4f72-9ff0-2d7ee6695990"),
                            Cost = 550.00m,
                            Name = "Fuel Injector"
                        },
                        new
                        {
                            Id = new Guid("2c099b35-2282-4bb3-946e-3cae913abee2"),
                            Cost = 650.00m,
                            Name = "Dashboard"
                        },
                        new
                        {
                            Id = new Guid("6537d296-1888-41d4-a6f8-8096eea77da0"),
                            Cost = 220.00m,
                            Name = "Steering Wheel"
                        },
                        new
                        {
                            Id = new Guid("b60a9a1a-195d-4f8f-8624-c51a69a4991d"),
                            Cost = 300.00m,
                            Name = "Shock Absorbers"
                        },
                        new
                        {
                            Id = new Guid("8248a366-fe21-41ac-acc4-bfc46867916a"),
                            Cost = 280.00m,
                            Name = "Brake Calipers"
                        },
                        new
                        {
                            Id = new Guid("702f8668-296c-4c6a-8e48-b6b94be8136d"),
                            Cost = 700.00m,
                            Name = "Catalytic Converter"
                        },
                        new
                        {
                            Id = new Guid("1f364a5c-6d62-4664-8691-bed60c692572"),
                            Cost = 350.00m,
                            Name = "Muffler"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
