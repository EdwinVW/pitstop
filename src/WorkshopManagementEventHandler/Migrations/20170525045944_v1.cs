using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pitstop.WorkshopManagementEventHandler.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TelephoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    LicenseNumber = table.Column<string>(nullable: false),
                    Brand = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.LicenseNumber);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceJob",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActualEndTime = table.Column<DateTime>(nullable: true),
                    ActualStartTime = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    VehicleLicenseNumber = table.Column<string>(nullable: true),
                    WorkshopPlanningDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceJob", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceJob_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceJob_Vehicle_VehicleLicenseNumber",
                        column: x => x.VehicleLicenseNumber,
                        principalTable: "Vehicle",
                        principalColumn: "LicenseNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJob_CustomerId",
                table: "MaintenanceJob",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJob_VehicleLicenseNumber",
                table: "MaintenanceJob",
                column: "VehicleLicenseNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceJob");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Vehicle");
        }
    }
}
