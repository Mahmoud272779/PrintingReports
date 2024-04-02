using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    public partial class settingsandpermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "AttendLeaving_Settings",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "isShift1_extended",
                table: "AttendancPermission",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isShift2_extended",
                table: "AttendancPermission",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isShift3_extended",
                table: "AttendancPermission",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isShift4_extended",
                table: "AttendancPermission",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AttendLeaving_Settings_BranchId",
                table: "AttendLeaving_Settings",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendLeaving_Settings_GLBranch_BranchId",
                table: "AttendLeaving_Settings",
                column: "BranchId",
                principalTable: "GLBranch",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendLeaving_Settings_GLBranch_BranchId",
                table: "AttendLeaving_Settings");

            migrationBuilder.DropIndex(
                name: "IX_AttendLeaving_Settings_BranchId",
                table: "AttendLeaving_Settings");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "AttendLeaving_Settings");

            migrationBuilder.DropColumn(
                name: "isShift1_extended",
                table: "AttendancPermission");

            migrationBuilder.DropColumn(
                name: "isShift2_extended",
                table: "AttendancPermission");

            migrationBuilder.DropColumn(
                name: "isShift3_extended",
                table: "AttendancPermission");

            migrationBuilder.DropColumn(
                name: "isShift4_extended",
                table: "AttendancPermission");
        }
    }
}
