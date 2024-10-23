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
                name: "RepairOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CutomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LaborCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrders", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "RepairOrderVehicleParts",
                columns: table => new
                {
                    RepairOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehiclePartsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderVehicleParts", x => new { x.RepairOrderId, x.VehiclePartsId });
                    table.ForeignKey(
                        name: "FK_RepairOrderVehicleParts_RepairOrders_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalTable: "RepairOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepairOrderVehicleParts_VehicleParts_VehiclePartsId",
                        column: x => x.VehiclePartsId,
                        principalTable: "VehicleParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "VehicleParts",
                columns: new[] { "Id", "Cost", "Name" },
                values: new object[,]
                {
                    { new Guid("03fd0223-a8ef-44c3-819f-f704e9bf4b19"), 2400.00m, "Transmission" },
                    { new Guid("0dcce8a1-24fe-4b78-8cd9-0d517d6210cf"), 300.00m, "Brake Pads" },
                    { new Guid("149a46cc-1a6d-4c2b-a744-9537e7140a5c"), 320.00m, "Timing Belt" },
                    { new Guid("1a030c72-c058-430e-8e09-fe19d782ca9f"), 650.00m, "Dashboard" },
                    { new Guid("1a38a640-5aff-4ccb-b90b-408e58bcfe1b"), 400.00m, "Water Pump" },
                    { new Guid("21a60a25-dd00-4e85-b92f-6b50af9b9da8"), 220.00m, "Steering Wheel" },
                    { new Guid("261b7ad0-d769-4ef0-a1d6-d1713cb8f950"), 25.00m, "Oil Filter" },
                    { new Guid("298e3e56-be49-453f-8988-6d34a0ed2f9f"), 350.00m, "Muffler" },
                    { new Guid("2a2ff87b-ff7b-469b-be5c-559b69019bdf"), 300.00m, "Shock Absorbers" },
                    { new Guid("3cee969b-7561-4ba2-a3e2-17bcef8f0fc8"), 400.00m, "Starter Motor" },
                    { new Guid("3e9753cd-7031-48d5-9767-b96eed95ade9"), 550.00m, "Fuel Injector" },
                    { new Guid("4532dd87-8207-4ed0-8e03-0cb31cac6319"), 800.00m, "Suspension" },
                    { new Guid("496b0ac1-6ff0-4256-b845-e40a50e44464"), 200.00m, "Battery" },
                    { new Guid("62b8cb27-6654-42db-9eb9-6ca192a18819"), 1200.00m, "Engine" },
                    { new Guid("76cf53e4-0968-408d-88d0-8f3421e600b9"), 150.00m, "Headlights" },
                    { new Guid("7a4f3ccc-985f-484c-bc6e-598e7e16981e"), 35.00m, "Air Filter" },
                    { new Guid("88c05295-0c58-4d0a-b5ed-85c5ef505334"), 500.00m, "Exhaust" },
                    { new Guid("a45e3037-e1ac-489c-b707-448bd8760799"), 450.00m, "Alternator" },
                    { new Guid("ab912724-4b12-4ddc-8ecc-9d694c17675f"), 250.00m, "Radiator" },
                    { new Guid("c9a2e5d9-241d-4667-8abe-9905ea041181"), 850.00m, "Clutch" },
                    { new Guid("d1afd464-480b-4101-9052-9c94687b3d72"), 75.00m, "Spark Plugs" },
                    { new Guid("dbbc0244-6d50-40cb-86cd-fdfd3345a07b"), 600.00m, "Fuel Pump" },
                    { new Guid("e44efe1f-12e1-423e-9bcd-f516a38a4c7e"), 700.00m, "Catalytic Converter" },
                    { new Guid("ec1066ca-245a-42db-b4ea-b1910637e343"), 120.00m, "Taillights" },
                    { new Guid("f90e7acc-c05e-4b9f-9edd-3a92e725491f"), 280.00m, "Brake Calipers" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderVehicleParts_VehiclePartsId",
                table: "RepairOrderVehicleParts",
                column: "VehiclePartsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepairOrderVehicleParts");

            migrationBuilder.DropTable(
                name: "RepairOrders");

            migrationBuilder.DropTable(
                name: "VehicleParts");
        }
    }
}
