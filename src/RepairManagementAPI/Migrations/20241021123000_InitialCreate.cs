using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pitstop.RepairManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VehiclePartId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LaborCost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RejectReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    LicenseNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.LicenseNumber);
                });

            migrationBuilder.CreateTable(
                name: "VehicleParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleParts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "VehicleParts",
                columns: new[] { "Id", "Cost", "Name" },
                values: new object[,]
                {
                    { new Guid("0d66b7e2-c228-4379-87e0-25196049e6fd"), 25.00m, "Oil Filter" },
                    { new Guid("1f364a5c-6d62-4664-8691-bed60c692572"), 350.00m, "Muffler" },
                    { new Guid("2c099b35-2282-4bb3-946e-3cae913abee2"), 650.00m, "Dashboard" },
                    { new Guid("486e09f9-da8b-4577-ba57-fe75990d6f84"), 500.00m, "Exhaust" },
                    { new Guid("5280ec2b-c0c1-45c9-8916-e439b8f557d4"), 850.00m, "Clutch" },
                    { new Guid("56947ee4-b977-4bcc-85f4-9eba1ed63940"), 75.00m, "Spark Plugs" },
                    { new Guid("6537d296-1888-41d4-a6f8-8096eea77da0"), 220.00m, "Steering Wheel" },
                    { new Guid("65ba4299-05dd-4f72-9ff0-2d7ee6695990"), 550.00m, "Fuel Injector" },
                    { new Guid("6df99364-466b-4e88-9b9e-dfddc572f06b"), 800.00m, "Suspension" },
                    { new Guid("702f8668-296c-4c6a-8e48-b6b94be8136d"), 700.00m, "Catalytic Converter" },
                    { new Guid("810dcdf3-fd00-4d39-8132-8a0c393edd8a"), 400.00m, "Water Pump" },
                    { new Guid("8248a366-fe21-41ac-acc4-bfc46867916a"), 280.00m, "Brake Calipers" },
                    { new Guid("8549b861-ef1b-448e-b791-e6176cd95386"), 250.00m, "Radiator" },
                    { new Guid("a2bc1f12-c90d-4780-b9a2-6eb14f955610"), 2400.00m, "Transmission" },
                    { new Guid("a81c2242-1340-48a7-81f3-2700114fa520"), 200.00m, "Battery" },
                    { new Guid("ab99586f-a421-48f7-83b5-f9ec47aeac7b"), 300.00m, "Brake Pads" },
                    { new Guid("b60a9a1a-195d-4f8f-8624-c51a69a4991d"), 300.00m, "Shock Absorbers" },
                    { new Guid("bc37223d-2fad-4eec-b8fc-134e00da097a"), 320.00m, "Timing Belt" },
                    { new Guid("bfd00e1b-e2a9-4c1b-ae3e-837f8c7b1c9a"), 400.00m, "Starter Motor" },
                    { new Guid("bff55876-b954-495a-8871-56887f547032"), 450.00m, "Alternator" },
                    { new Guid("e2efd403-fe51-46d4-966e-90b0430c0b4e"), 120.00m, "Taillights" },
                    { new Guid("e6d590cd-d4d8-4cd8-9a08-7a754d12a7de"), 150.00m, "Headlights" },
                    { new Guid("e831584d-b318-4b57-b436-aa934a06e8d6"), 1200.00m, "Engine" },
                    { new Guid("ee01a952-6f23-4479-a76b-8cc92e5ea26f"), 35.00m, "Air Filter" },
                    { new Guid("f9284ca5-f36b-4e57-90dd-417598d7ae86"), 600.00m, "Fuel Pump" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "RepairOrders");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "VehicleParts");
        }
    }
}
