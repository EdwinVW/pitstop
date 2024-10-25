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
                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
                columns: new[] { "Id", "PartCost", "PartName" },
                values: new object[,]
                {
                    { new Guid("10b53bff-9e67-483e-9f2c-ae36f3422bb1"), 800.00m, "Suspension" },
                    { new Guid("1d593592-0892-4b7e-9f20-3963c85146cb"), 400.00m, "Water Pump" },
                    { new Guid("2436b636-585b-49c3-a0cc-e9d7e00f1abc"), 150.00m, "Headlights" },
                    { new Guid("2c9470db-6bbf-4f90-a4b1-c95180e400bc"), 320.00m, "Timing Belt" },
                    { new Guid("305080ed-a8d5-41cd-9cb0-5a6375ce1615"), 250.00m, "Radiator" },
                    { new Guid("30b38c16-fabf-4229-a7f4-069aa068c8c0"), 700.00m, "Catalytic Converter" },
                    { new Guid("44717b24-eed4-434d-b784-cf7292ab3c07"), 350.00m, "Muffler" },
                    { new Guid("51211baa-9056-49f2-aa8a-9535d21c3e83"), 2400.00m, "Transmission" },
                    { new Guid("5ca923e8-e47f-488e-8438-f0d29bde8510"), 450.00m, "Alternator" },
                    { new Guid("694912c5-3841-41fc-bec7-4544a84e48f0"), 280.00m, "Brake Calipers" },
                    { new Guid("7189321d-5ed3-41db-a91b-7c9641135b23"), 220.00m, "Steering Wheel" },
                    { new Guid("74049415-7d35-4857-bfaf-a4eeec2e72da"), 200.00m, "Battery" },
                    { new Guid("78d25e89-7aa8-4e96-a14e-68f632d15540"), 300.00m, "Shock Absorbers" },
                    { new Guid("83aca8e8-29f9-4d2e-b5cb-7c6ee21f3aee"), 1200.00m, "Engine" },
                    { new Guid("8be4af84-25c2-4b58-8fa3-36982beb3dc4"), 600.00m, "Fuel Pump" },
                    { new Guid("8d525553-83e6-41cf-953d-71f53ff7f968"), 400.00m, "Starter Motor" },
                    { new Guid("ab3b2aa9-544a-4a41-a511-aa9253fa17d8"), 850.00m, "Clutch" },
                    { new Guid("b49b7c02-b6b1-4c57-832e-17b66e15ba53"), 75.00m, "Spark Plugs" },
                    { new Guid("c382b0da-c294-4302-aad6-e3c993aba5a3"), 35.00m, "Air Filter" },
                    { new Guid("c7da1541-4e93-44eb-bbf7-7512be5ca54b"), 120.00m, "Taillights" },
                    { new Guid("d756198d-250d-440f-a532-115cae2b653d"), 300.00m, "Brake Pads" },
                    { new Guid("da630a7e-51be-427f-9b76-eec798dd2825"), 25.00m, "Oil Filter" },
                    { new Guid("e52df18f-2116-4124-9850-dcf99a36ef02"), 500.00m, "Exhaust" },
                    { new Guid("ee14080e-ad18-4142-8ec2-a0732ea3d548"), 550.00m, "Fuel Injector" },
                    { new Guid("f293d48f-f841-4677-b461-fe78338433ce"), 650.00m, "Dashboard" }
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
