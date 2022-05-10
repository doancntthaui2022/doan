using Microsoft.EntityFrameworkCore.Migrations;

namespace HoangNV.HotelBooking.Web.Migrations
{
    public partial class update_hotelbranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "HotelBranchs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "HotelBranchs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6ea013dd-40ea-4a0e-9d0e-a675c0e9498b",
                column: "PassWord",
                value: "AQAAAAEAACcQAAAAEE3JSs1lR0uh5HsO5IDP+TdWvbZhDgHEORTM2r0psCrUL3rs3hM0Or7QV2GewVM3xA==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "HotelBranchs");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "HotelBranchs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6ea013dd-40ea-4a0e-9d0e-a675c0e9498b",
                column: "PassWord",
                value: "AQAAAAEAACcQAAAAEEs7Zo3FMgFwsGOsNmwqm/eFFcqfJIPOFblxUPAoTLW0qbyGgqRZ0sXTcNpyf9b7SQ==");
        }
    }
}
