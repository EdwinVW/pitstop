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
                    { new Guid("03997cb1-57ca-4597-9a23-f43ab4f8c517"), 280.00m, "Brake Calipers" },
                    { new Guid("0db3b891-1e79-423d-94ed-b640268ec570"), 350.00m, "Muffler" },
                    { new Guid("108dece9-44e9-4fe6-84cf-52faa345c283"), 35.00m, "Air Filter" },
                    { new Guid("26713241-e87f-48ca-8ded-5648700805b7"), 400.00m, "Water Pump" },
                    { new Guid("27285fe4-8729-463d-9f6e-39518a5b7826"), 150.00m, "Headlights" },
                    { new Guid("4c18c931-3d0e-45e1-b1fa-ba28a527fc7c"), 300.00m, "Shock Absorbers" },
                    { new Guid("5076b669-9e74-4245-962f-74ef0b4e6b3d"), 650.00m, "Dashboard" },
                    { new Guid("5b17530d-71a0-4258-b70a-9d4e7e2e5b38"), 450.00m, "Alternator" },
                    { new Guid("6be1fcbd-929c-4657-84e3-01b17ae0096e"), 2400.00m, "Transmission" },
                    { new Guid("7a8930fa-f4d2-463e-b274-ec4d4e1fc70b"), 300.00m, "Brake Pads" },
                    { new Guid("891c8dcf-b50d-4c0e-9951-cc949d3c4e4b"), 220.00m, "Steering Wheel" },
                    { new Guid("8ad8c9a1-9bb7-460f-a986-471dd0864d73"), 850.00m, "Clutch" },
                    { new Guid("9c1dc79e-6e98-4126-828f-696ffe7e325c"), 250.00m, "Radiator" },
                    { new Guid("9f3f455c-ea81-4a28-a968-0c6d34f310f7"), 75.00m, "Spark Plugs" },
                    { new Guid("a0fd9950-ad0e-485c-b607-8a83149fe78f"), 550.00m, "Fuel Injector" },
                    { new Guid("a224187e-169e-45bf-8b91-3986f3bb9fd0"), 200.00m, "Battery" },
                    { new Guid("ab29e382-54be-46b9-98ce-76084c638130"), 25.00m, "Oil Filter" },
                    { new Guid("b3335655-937f-47a2-a6e9-96cff0050ac2"), 500.00m, "Exhaust" },
                    { new Guid("c601f130-56f9-480b-8dc7-fe272c98c3a6"), 600.00m, "Fuel Pump" },
                    { new Guid("d3136793-6ca6-41c1-b5bf-7b90466183f2"), 800.00m, "Suspension" },
                    { new Guid("d35f5cbc-61a2-4015-8218-f6a23a63ea21"), 320.00m, "Timing Belt" },
                    { new Guid("dd96438c-f40c-4ad8-88a4-06b978511149"), 120.00m, "Taillights" },
                    { new Guid("df93f1d0-5d36-44da-8d24-d880590c40f4"), 700.00m, "Catalytic Converter" },
                    { new Guid("e263ae28-6cc7-4fb0-a4ca-1241bf821433"), 400.00m, "Starter Motor" },
                    { new Guid("e6a6ea35-2535-4f57-be93-bb721b409af3"), 1200.00m, "Engine" }
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
