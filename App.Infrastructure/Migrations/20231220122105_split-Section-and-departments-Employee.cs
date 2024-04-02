using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    public partial class splitSectionanddepartmentsEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvEmployees_SectionsAndDepartments_SectionsAndDepartmentsId",
                table: "InvEmployees");

            migrationBuilder.RenameColumn(
                name: "SectionsAndDepartmentsId",
                table: "InvEmployees",
                newName: "religionsId");

            migrationBuilder.RenameIndex(
                name: "IX_InvEmployees_SectionsAndDepartmentsId",
                table: "InvEmployees",
                newName: "IX_InvEmployees_religionsId");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentsId",
                table: "InvEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IDNumber",
                table: "InvEmployees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "InvEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectionsId",
                table: "InvEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "birthday",
                table: "InvEmployees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "InvEmployees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "InvEmployees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "religions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    arabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    latinName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_religions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvEmployees_DepartmentsId",
                table: "InvEmployees",
                column: "DepartmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_InvEmployees_SectionsId",
                table: "InvEmployees",
                column: "SectionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvEmployees_religions_religionsId",
                table: "InvEmployees",
                column: "religionsId",
                principalTable: "religions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvEmployees_SectionsAndDepartments_DepartmentsId",
                table: "InvEmployees",
                column: "DepartmentsId",
                principalTable: "SectionsAndDepartments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvEmployees_SectionsAndDepartments_SectionsId",
                table: "InvEmployees",
                column: "SectionsId",
                principalTable: "SectionsAndDepartments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvEmployees_religions_religionsId",
                table: "InvEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_InvEmployees_SectionsAndDepartments_DepartmentsId",
                table: "InvEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_InvEmployees_SectionsAndDepartments_SectionsId",
                table: "InvEmployees");

            migrationBuilder.DropTable(
                name: "religions");

            migrationBuilder.DropIndex(
                name: "IX_InvEmployees_DepartmentsId",
                table: "InvEmployees");

            migrationBuilder.DropIndex(
                name: "IX_InvEmployees_SectionsId",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "DepartmentsId",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "IDNumber",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "SectionsId",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "birthday",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "email",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "InvEmployees");

            migrationBuilder.RenameColumn(
                name: "religionsId",
                table: "InvEmployees",
                newName: "SectionsAndDepartmentsId");

            migrationBuilder.RenameIndex(
                name: "IX_InvEmployees_religionsId",
                table: "InvEmployees",
                newName: "IX_InvEmployees_SectionsAndDepartmentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvEmployees_SectionsAndDepartments_SectionsAndDepartmentsId",
                table: "InvEmployees",
                column: "SectionsAndDepartmentsId",
                principalTable: "SectionsAndDepartments",
                principalColumn: "Id");
        }
    }
}
