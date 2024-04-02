using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    public partial class editSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_TimeOfNeglectingMovementsInMinutes",
                table: "AttendLeaving_Settings");

            migrationBuilder.AddColumn<int>(
                name: "numberOfShiftsInReports",
                table: "AttendLeaving_Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numberOfShiftsInReports",
                table: "AttendLeaving_Settings");

            migrationBuilder.AddColumn<bool>(
                name: "is_TimeOfNeglectingMovementsInMinutes",
                table: "AttendLeaving_Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
