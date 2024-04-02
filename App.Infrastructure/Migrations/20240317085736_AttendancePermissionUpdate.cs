using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    public partial class AttendancePermissionUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "haveShift2",
                table: "AttendancPermission",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "haveShift3",
                table: "AttendancPermission",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "haveShift4",
                table: "AttendancPermission",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "haveShift2",
                table: "AttendancPermission");

            migrationBuilder.DropColumn(
                name: "haveShift3",
                table: "AttendancPermission");

            migrationBuilder.DropColumn(
                name: "haveShift4",
                table: "AttendancPermission");
        }
    }
}
