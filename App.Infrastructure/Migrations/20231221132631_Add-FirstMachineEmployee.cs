using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    public partial class AddFirstMachineEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "arabicName",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "latinName",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FirstLogmachineId",
                table: "InvEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvEmployees_FirstLogmachineId",
                table: "InvEmployees",
                column: "FirstLogmachineId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvEmployees_Machines_FirstLogmachineId",
                table: "InvEmployees",
                column: "FirstLogmachineId",
                principalTable: "Machines",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvEmployees_Machines_FirstLogmachineId",
                table: "InvEmployees");

            migrationBuilder.DropIndex(
                name: "IX_InvEmployees_FirstLogmachineId",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "arabicName",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "latinName",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "FirstLogmachineId",
                table: "InvEmployees");
        }
    }
}
