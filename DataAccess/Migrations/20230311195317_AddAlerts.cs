using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.DataAccess.Migrations
{
    public partial class AddAlerts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    AlertId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationDate = table.Column<string>(type: "TEXT", nullable: false),
                    FirstSensorReadingDate = table.Column<string>(type: "TEXT", nullable: false),
                    LastSensorReadingDate = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    MinValue = table.Column<double>(type: "REAL", nullable: false),
                    MaxValue = table.Column<double>(type: "REAL", nullable: false),
                    DeviceSerialNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.AlertId);
                    table.ForeignKey(
                        name: "FK_Alerts_Devices_DeviceSerialNumber",
                        column: x => x.DeviceSerialNumber,
                        principalTable: "Devices",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_DeviceSerialNumber",
                table: "Alerts",
                column: "DeviceSerialNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");
        }
    }
}
