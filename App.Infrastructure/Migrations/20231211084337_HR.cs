using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    public partial class HR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationSeen_NotificationsMaster_NotificationsMasterId",
                table: "NotificationSeen");

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "SystemHistoryLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "POSSessionHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CollectionReceipts",
                table: "otherSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvUnitsHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvStpItemCardHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvStoresHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvStorePlacesHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvSizesHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvSalesManHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvPurchasesAdditionalCostsHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvPersonsHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvPaymentMethodsHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CollectionMainCode",
                table: "InvoiceMasterHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvoiceMasterHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "eInvoiceCode",
                table: "InvoiceMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "eInvoiceReported",
                table: "InvoiceMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvJobsHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActiveElectronicInvoice",
                table: "InvGeneralSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OTP",
                table: "InvGeneralSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SystemUpdateNumber",
                table: "InvGeneralSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvGeneralSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvFundsBanksSafesHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvEmployeesHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Adding_working_hours_on_vacations",
                table: "InvEmployees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Auto_Dismissal_registration",
                table: "InvEmployees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Calculating_extra_time_after_work",
                table: "InvEmployees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Calculating_extra_time_before_work",
                table: "InvEmployees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deduction_of_delay_from_additional_time",
                table: "InvEmployees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SectionsAndDepartmentsId",
                table: "InvEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "employeesGroupId",
                table: "InvEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "gLBranchId",
                table: "InvEmployees",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "missionsId",
                table: "InvEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "nationalityId",
                table: "InvEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "projectsId",
                table: "InvEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "shiftsMasterId",
                table: "InvEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvDiscount_A_P_History",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvCommissionListHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvColorsHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvCategoriesHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CategoryType",
                table: "InvCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "kitchenId",
                table: "InvCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "InvBarcodeHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLSafeHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CollectionMainCode",
                table: "GLRecieptsHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubTypeId",
                table: "GLRecieptsHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLRecieptsHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CollectionMainCode",
                table: "GlReciepts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLRecHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLOtherAuthoritiesHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLJournalEntryDraft",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReceiptsMainCode",
                table: "GLJournalEntryDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLJournalEntry",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLGeneralSetting",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLFinancialSetting",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLFinancialAccountHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLFinancialAccountForOpeningBalance",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLFinancialAccount",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLCurrencyHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLCostCenterHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLCostCenter",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLBranchHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLBanksHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTechnicalSupport",
                table: "GLBalanceForLastPeriod",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AttendancPermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsMoved = table.Column<bool>(type: "bit", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    totalHoursForOpenShift = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift1_start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift1_end = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift2_start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift2_end = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift3_start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift3_end = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift4_start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift4_end = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendancPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendancPermission_InvEmployees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "InvEmployees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttendLeaving_Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    is_TimeOfNeglectingMovementsInMinutes = table.Column<bool>(type: "bit", nullable: false),
                    TimeOfNeglectingMovementsInMinutes = table.Column<TimeSpan>(type: "time", nullable: false),
                    is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift = table.Column<bool>(type: "bit", nullable: false),
                    The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift = table.Column<TimeSpan>(type: "time", nullable: false),
                    is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift = table.Column<bool>(type: "bit", nullable: false),
                    The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift = table.Column<TimeSpan>(type: "time", nullable: false),
                    is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift = table.Column<bool>(type: "bit", nullable: false),
                    The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift = table.Column<TimeSpan>(type: "time", nullable: false),
                    is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift = table.Column<bool>(type: "bit", nullable: false),
                    The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift = table.Column<TimeSpan>(type: "time", nullable: false),
                    is_The_maximum_delay_in_minutes = table.Column<bool>(type: "bit", nullable: false),
                    The_maximum_delay_in_minutes = table.Column<TimeSpan>(type: "time", nullable: false),
                    is_The_Maximum_limit_for_early_dismissal_in_minutes = table.Column<bool>(type: "bit", nullable: false),
                    The_Maximum_limit_for_early_dismissal_in_minutes = table.Column<TimeSpan>(type: "time", nullable: false),
                    SetLastMoveAsLeave = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendLeaving_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeesGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    arabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    latinName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GLPrinterHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employeesId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    LastTransactionAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddTransactionUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddTransactionDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastTransactionUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastTransactionDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteTransactionUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteTransactionDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastTransactionUserAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isTechnicalSupport = table.Column<bool>(type: "bit", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    LatinName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrowserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLPrinterHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GLPrinterHistory_InvEmployees_employeesId",
                        column: x => x.employeesId,
                        principalTable: "InvEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    arabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    latinName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    startdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    enddate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kitchens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    LatinName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArabicName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false),
                    UTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kitchens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KitchensHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employeesId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    LastTransactionAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddTransactionUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddTransactionDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastTransactionUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastTransactionDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteTransactionUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteTransactionDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastTransactionUserAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isTechnicalSupport = table.Column<bool>(type: "bit", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    LatinName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrowserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchensHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KitchensHistory_InvEmployees_employeesId",
                        column: x => x.employeesId,
                        principalTable: "InvEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineSN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    branchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Machines_GLBranch_branchId",
                        column: x => x.branchId,
                        principalTable: "GLBranch",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Missions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    arabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    latinName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoviedTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeesId = table.Column<int>(type: "int", nullable: false),
                    day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    shift1_BranchIdIn = table.Column<int>(type: "int", nullable: true),
                    shift1_BranchIdOut = table.Column<int>(type: "int", nullable: true),
                    shift1_TimeIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift1_TimeOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift1_ExtraTimeBefore = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift1_ExtraTimeAfter = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift1_LateTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift1_LeaveEarly = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift1_TotalShiftHours = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift1_TotalWorkHours = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift2_BranchIdIn = table.Column<int>(type: "int", nullable: true),
                    shift2_BranchIdOut = table.Column<int>(type: "int", nullable: true),
                    shift2_TimeIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift2_TimeOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift2_ExtraTimeBefore = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift2_ExtraTimeAfter = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift2_LateTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift2_LeaveEarly = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift2_TotalShiftHours = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift2_TotalWorkHours = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsHaveShift2 = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    shift3_BranchIdIn = table.Column<int>(type: "int", nullable: true),
                    shift3_BranchIdOut = table.Column<int>(type: "int", nullable: true),
                    shift3_TimeIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift3_TimeOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift3_ExtraTimeBefore = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift3_ExtraTimeAfter = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift3_LateTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift3_LeaveEarly = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift3_TotalShiftHours = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift3_TotalWorkHours = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsHaveShift3 = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    shift4_BranchIdIn = table.Column<int>(type: "int", nullable: true),
                    shift4_BranchIdOut = table.Column<int>(type: "int", nullable: true),
                    shift4_TimeIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift4_TimeOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    shift4_ExtraTimeBefore = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift4_ExtraTimeAfter = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift4_LateTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift4_LeaveEarly = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift4_TotalShiftHours = table.Column<TimeSpan>(type: "time", nullable: true),
                    shift4_TotalWorkHours = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsHaveShift4 = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    cDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsAbsance = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsHoliday = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviedTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoviedTransactions_InvEmployees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "InvEmployees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Nationality",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    arabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    latinName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationality", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    arabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    latinName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RstCategoriesPrinters",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PrinterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RstCategoriesPrinters", x => new { x.CategoryId, x.PrinterId });
                    table.ForeignKey(
                        name: "FK_RstCategoriesPrinters_GLPrinter_PrinterId",
                        column: x => x.PrinterId,
                        principalTable: "GLPrinter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RstCategoriesPrinters_InvCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "InvCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectionsAndDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<int>(type: "int", nullable: false),
                    arabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    latinName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    empId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    parentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionsAndDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectionsAndDepartments_InvEmployees_empId",
                        column: x => x.empId,
                        principalTable: "InvEmployees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShiftsMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    arabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    latinName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dayEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    shiftType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftsMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vaccation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LatinName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HolidaysEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidaysId = table.Column<int>(type: "int", nullable: false),
                    EmployeesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidaysEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolidaysEmployees_Holidays_HolidaysId",
                        column: x => x.HolidaysId,
                        principalTable: "Holidays",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HolidaysEmployees_InvEmployees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "InvEmployees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MachineTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeCode = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedTransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    machineId = table.Column<int>(type: "int", nullable: false),
                    IsMoved = table.Column<bool>(type: "bit", nullable: false),
                    isAuto = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    PushTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineTransactions_Machines_machineId",
                        column: x => x.machineId,
                        principalTable: "Machines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChangefulTimeGroupsMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    arabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    latinName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isRamadan = table.Column<bool>(type: "bit", nullable: false),
                    shiftsMasterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangefulTimeGroupsMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangefulTimeGroupsMaster_ShiftsMaster_shiftsMasterId",
                        column: x => x.shiftsMasterId,
                        principalTable: "ShiftsMaster",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NormalShiftDetalies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsVacation = table.Column<bool>(type: "bit", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    DayId = table.Column<int>(type: "int", nullable: false),
                    TotalDayHours = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsRamadan = table.Column<bool>(type: "bit", nullable: false),
                    shift1_startIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_endIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_startOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_endOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_End = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_IsExtended = table.Column<bool>(type: "bit", nullable: false),
                    shift1_lateBefore = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_lateAfter = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_startIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_endIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_startOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_endOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_End = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsHaveShift2 = table.Column<bool>(type: "bit", nullable: false),
                    shift2_IsExtended = table.Column<bool>(type: "bit", nullable: false),
                    shift2_lateBefore = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_lateAfter = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_startIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_endIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_startOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_endOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_End = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsHaveShift3 = table.Column<bool>(type: "bit", nullable: false),
                    shift3_IsExtended = table.Column<bool>(type: "bit", nullable: false),
                    shift3_lateBefore = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_lateAfter = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_startIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_endIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_startOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_endOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_End = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsHaveShift4 = table.Column<bool>(type: "bit", nullable: false),
                    shift4_IsExtended = table.Column<bool>(type: "bit", nullable: false),
                    shift4_lateBefore = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_lateAfter = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NormalShiftDetalies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NormalShiftDetalies_ShiftsMaster_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "ShiftsMaster",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VaccationEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VaccationId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccationEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaccationEmployees_InvEmployees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "InvEmployees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VaccationEmployees_Vaccation_VaccationId",
                        column: x => x.VaccationId,
                        principalTable: "Vaccation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChangefulTimeDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsRamadan = table.Column<bool>(type: "bit", nullable: false),
                    IsVacation = table.Column<bool>(type: "bit", nullable: false),
                    changefulTimeGroupsId = table.Column<int>(type: "int", nullable: false),
                    day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    shift1_startIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_endIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_startOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_endOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_End = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_IsExtended = table.Column<bool>(type: "bit", nullable: false),
                    shift1_lateBefore = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_lateAfter = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_startIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_endIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_startOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_endOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_End = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsHaveShift2 = table.Column<bool>(type: "bit", nullable: false),
                    shift2_IsExtended = table.Column<bool>(type: "bit", nullable: false),
                    shift2_lateBefore = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_lateAfter = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_startIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_endIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_startOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_endOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_End = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsHaveShift3 = table.Column<bool>(type: "bit", nullable: false),
                    shift3_IsExtended = table.Column<bool>(type: "bit", nullable: false),
                    shift3_lateBefore = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_lateAfter = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_startIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_endIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_startOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_endOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_End = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsHaveShift4 = table.Column<bool>(type: "bit", nullable: false),
                    shift4_IsExtended = table.Column<bool>(type: "bit", nullable: false),
                    shift4_lateBefore = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_lateAfter = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangefulTimeDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangefulTimeDays_ChangefulTimeGroupsMaster_changefulTimeGroupsId",
                        column: x => x.changefulTimeGroupsId,
                        principalTable: "ChangefulTimeGroupsMaster",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChangefulTimeGroupsDetalies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    workDaysNumber = table.Column<int>(type: "int", nullable: false),
                    weekendNumber = table.Column<int>(type: "int", nullable: false),
                    IsRamadan = table.Column<bool>(type: "bit", nullable: false),
                    changefulTimeGroupsId = table.Column<int>(type: "int", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    shift1_startIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_endIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_startOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_endOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_End = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_IsExtended = table.Column<bool>(type: "bit", nullable: false),
                    shift1_lateBefore = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift1_lateAfter = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_startIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_endIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_startOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_endOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_End = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsHaveShift2 = table.Column<bool>(type: "bit", nullable: false),
                    shift2_IsExtended = table.Column<bool>(type: "bit", nullable: false),
                    shift2_lateBefore = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift2_lateAfter = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_startIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_endIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_startOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_endOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_End = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsHaveShift3 = table.Column<bool>(type: "bit", nullable: false),
                    shift3_IsExtended = table.Column<bool>(type: "bit", nullable: false),
                    shift3_lateBefore = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift3_lateAfter = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_startIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_endIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_startOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_endOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_End = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsHaveShift4 = table.Column<bool>(type: "bit", nullable: false),
                    shift4_IsExtended = table.Column<bool>(type: "bit", nullable: false),
                    shift4_lateBefore = table.Column<TimeSpan>(type: "time", nullable: false),
                    shift4_lateAfter = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangefulTimeGroupsDetalies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangefulTimeGroupsDetalies_ChangefulTimeGroupsMaster_changefulTimeGroupsId",
                        column: x => x.changefulTimeGroupsId,
                        principalTable: "ChangefulTimeGroupsMaster",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChangefulTimeGroupsEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    changefulTimeGroupsMasterId = table.Column<int>(type: "int", nullable: false),
                    invEmployeesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangefulTimeGroupsEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangefulTimeGroupsEmployees_ChangefulTimeGroupsMaster_changefulTimeGroupsMasterId",
                        column: x => x.changefulTimeGroupsMasterId,
                        principalTable: "ChangefulTimeGroupsMaster",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChangefulTimeGroupsEmployees_InvEmployees_invEmployeesId",
                        column: x => x.invEmployeesId,
                        principalTable: "InvEmployees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvEmployees_employeesGroupId",
                table: "InvEmployees",
                column: "employeesGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_InvEmployees_gLBranchId",
                table: "InvEmployees",
                column: "gLBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_InvEmployees_missionsId",
                table: "InvEmployees",
                column: "missionsId");

            migrationBuilder.CreateIndex(
                name: "IX_InvEmployees_nationalityId",
                table: "InvEmployees",
                column: "nationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_InvEmployees_projectsId",
                table: "InvEmployees",
                column: "projectsId");

            migrationBuilder.CreateIndex(
                name: "IX_InvEmployees_SectionsAndDepartmentsId",
                table: "InvEmployees",
                column: "SectionsAndDepartmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_InvEmployees_shiftsMasterId",
                table: "InvEmployees",
                column: "shiftsMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_InvCategories_kitchenId",
                table: "InvCategories",
                column: "kitchenId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendancPermission_EmpId",
                table: "AttendancPermission",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangefulTimeDays_changefulTimeGroupsId",
                table: "ChangefulTimeDays",
                column: "changefulTimeGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangefulTimeGroupsDetalies_changefulTimeGroupsId",
                table: "ChangefulTimeGroupsDetalies",
                column: "changefulTimeGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangefulTimeGroupsEmployees_changefulTimeGroupsMasterId",
                table: "ChangefulTimeGroupsEmployees",
                column: "changefulTimeGroupsMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangefulTimeGroupsEmployees_invEmployeesId",
                table: "ChangefulTimeGroupsEmployees",
                column: "invEmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangefulTimeGroupsMaster_shiftsMasterId",
                table: "ChangefulTimeGroupsMaster",
                column: "shiftsMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_GLPrinterHistory_employeesId",
                table: "GLPrinterHistory",
                column: "employeesId");

            migrationBuilder.CreateIndex(
                name: "IX_HolidaysEmployees_EmployeesId",
                table: "HolidaysEmployees",
                column: "EmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_HolidaysEmployees_HolidaysId",
                table: "HolidaysEmployees",
                column: "HolidaysId");

            migrationBuilder.CreateIndex(
                name: "IX_KitchensHistory_employeesId",
                table: "KitchensHistory",
                column: "employeesId");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_branchId",
                table: "Machines",
                column: "branchId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTransactions_machineId",
                table: "MachineTransactions",
                column: "machineId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviedTransactions_EmployeesId",
                table: "MoviedTransactions",
                column: "EmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_NormalShiftDetalies_ShiftId",
                table: "NormalShiftDetalies",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_RstCategoriesPrinters_PrinterId",
                table: "RstCategoriesPrinters",
                column: "PrinterId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionsAndDepartments_empId",
                table: "SectionsAndDepartments",
                column: "empId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccationEmployees_EmployeeId",
                table: "VaccationEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccationEmployees_VaccationId",
                table: "VaccationEmployees",
                column: "VaccationId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvCategories_Kitchens_kitchenId",
                table: "InvCategories",
                column: "kitchenId",
                principalTable: "Kitchens",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvEmployees_EmployeesGroup_employeesGroupId",
                table: "InvEmployees",
                column: "employeesGroupId",
                principalTable: "EmployeesGroup",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvEmployees_GLBranch_gLBranchId",
                table: "InvEmployees",
                column: "gLBranchId",
                principalTable: "GLBranch",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvEmployees_Missions_missionsId",
                table: "InvEmployees",
                column: "missionsId",
                principalTable: "Missions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvEmployees_Nationality_nationalityId",
                table: "InvEmployees",
                column: "nationalityId",
                principalTable: "Nationality",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvEmployees_Projects_projectsId",
                table: "InvEmployees",
                column: "projectsId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvEmployees_SectionsAndDepartments_SectionsAndDepartmentsId",
                table: "InvEmployees",
                column: "SectionsAndDepartmentsId",
                principalTable: "SectionsAndDepartments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvEmployees_ShiftsMaster_shiftsMasterId",
                table: "InvEmployees",
                column: "shiftsMasterId",
                principalTable: "ShiftsMaster",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationSeen_NotificationsMaster_NotificationsMasterId",
                table: "NotificationSeen",
                column: "NotificationsMasterId",
                principalTable: "NotificationsMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvCategories_Kitchens_kitchenId",
                table: "InvCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_InvEmployees_EmployeesGroup_employeesGroupId",
                table: "InvEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_InvEmployees_GLBranch_gLBranchId",
                table: "InvEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_InvEmployees_Missions_missionsId",
                table: "InvEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_InvEmployees_Nationality_nationalityId",
                table: "InvEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_InvEmployees_Projects_projectsId",
                table: "InvEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_InvEmployees_SectionsAndDepartments_SectionsAndDepartmentsId",
                table: "InvEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_InvEmployees_ShiftsMaster_shiftsMasterId",
                table: "InvEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationSeen_NotificationsMaster_NotificationsMasterId",
                table: "NotificationSeen");

            migrationBuilder.DropTable(
                name: "AttendancPermission");

            migrationBuilder.DropTable(
                name: "AttendLeaving_Settings");

            migrationBuilder.DropTable(
                name: "ChangefulTimeDays");

            migrationBuilder.DropTable(
                name: "ChangefulTimeGroupsDetalies");

            migrationBuilder.DropTable(
                name: "ChangefulTimeGroupsEmployees");

            migrationBuilder.DropTable(
                name: "EmployeesGroup");

            migrationBuilder.DropTable(
                name: "GLPrinterHistory");

            migrationBuilder.DropTable(
                name: "HolidaysEmployees");

            migrationBuilder.DropTable(
                name: "Kitchens");

            migrationBuilder.DropTable(
                name: "KitchensHistory");

            migrationBuilder.DropTable(
                name: "MachineTransactions");

            migrationBuilder.DropTable(
                name: "Missions");

            migrationBuilder.DropTable(
                name: "MoviedTransactions");

            migrationBuilder.DropTable(
                name: "Nationality");

            migrationBuilder.DropTable(
                name: "NormalShiftDetalies");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "RstCategoriesPrinters");

            migrationBuilder.DropTable(
                name: "SectionsAndDepartments");

            migrationBuilder.DropTable(
                name: "VaccationEmployees");

            migrationBuilder.DropTable(
                name: "ChangefulTimeGroupsMaster");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.DropTable(
                name: "Vaccation");

            migrationBuilder.DropTable(
                name: "ShiftsMaster");

            migrationBuilder.DropIndex(
                name: "IX_InvEmployees_employeesGroupId",
                table: "InvEmployees");

            migrationBuilder.DropIndex(
                name: "IX_InvEmployees_gLBranchId",
                table: "InvEmployees");

            migrationBuilder.DropIndex(
                name: "IX_InvEmployees_missionsId",
                table: "InvEmployees");

            migrationBuilder.DropIndex(
                name: "IX_InvEmployees_nationalityId",
                table: "InvEmployees");

            migrationBuilder.DropIndex(
                name: "IX_InvEmployees_projectsId",
                table: "InvEmployees");

            migrationBuilder.DropIndex(
                name: "IX_InvEmployees_SectionsAndDepartmentsId",
                table: "InvEmployees");

            migrationBuilder.DropIndex(
                name: "IX_InvEmployees_shiftsMasterId",
                table: "InvEmployees");

            migrationBuilder.DropIndex(
                name: "IX_InvCategories_kitchenId",
                table: "InvCategories");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "SystemHistoryLogs");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "POSSessionHistory");

            migrationBuilder.DropColumn(
                name: "CollectionReceipts",
                table: "otherSettings");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvUnitsHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvStpItemCardHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvStoresHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvStorePlacesHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvSizesHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvSalesManHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvPurchasesAdditionalCostsHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvPersonsHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvPaymentMethodsHistory");

            migrationBuilder.DropColumn(
                name: "CollectionMainCode",
                table: "InvoiceMasterHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvoiceMasterHistory");

            migrationBuilder.DropColumn(
                name: "eInvoiceCode",
                table: "InvoiceMaster");

            migrationBuilder.DropColumn(
                name: "eInvoiceReported",
                table: "InvoiceMaster");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvJobsHistory");

            migrationBuilder.DropColumn(
                name: "ActiveElectronicInvoice",
                table: "InvGeneralSettings");

            migrationBuilder.DropColumn(
                name: "OTP",
                table: "InvGeneralSettings");

            migrationBuilder.DropColumn(
                name: "SystemUpdateNumber",
                table: "InvGeneralSettings");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvGeneralSettings");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvFundsBanksSafesHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvEmployeesHistory");

            migrationBuilder.DropColumn(
                name: "Adding_working_hours_on_vacations",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "Auto_Dismissal_registration",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "Calculating_extra_time_after_work",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "Calculating_extra_time_before_work",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "Deduction_of_delay_from_additional_time",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "SectionsAndDepartmentsId",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "employeesGroupId",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "gLBranchId",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "missionsId",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "nationalityId",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "projectsId",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "shiftsMasterId",
                table: "InvEmployees");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvDiscount_A_P_History");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvCommissionListHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvColorsHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvCategoriesHistory");

            migrationBuilder.DropColumn(
                name: "CategoryType",
                table: "InvCategories");

            migrationBuilder.DropColumn(
                name: "kitchenId",
                table: "InvCategories");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "InvBarcodeHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLSafeHistory");

            migrationBuilder.DropColumn(
                name: "CollectionMainCode",
                table: "GLRecieptsHistory");

            migrationBuilder.DropColumn(
                name: "SubTypeId",
                table: "GLRecieptsHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLRecieptsHistory");

            migrationBuilder.DropColumn(
                name: "CollectionMainCode",
                table: "GlReciepts");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLRecHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLOtherAuthoritiesHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLJournalEntryDraft");

            migrationBuilder.DropColumn(
                name: "ReceiptsMainCode",
                table: "GLJournalEntryDetails");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLJournalEntry");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLGeneralSetting");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLFinancialSetting");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLFinancialAccountHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLFinancialAccountForOpeningBalance");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLFinancialAccount");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLCurrencyHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLCostCenterHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLCostCenter");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLBranchHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLBanksHistory");

            migrationBuilder.DropColumn(
                name: "isTechnicalSupport",
                table: "GLBalanceForLastPeriod");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationSeen_NotificationsMaster_NotificationsMasterId",
                table: "NotificationSeen",
                column: "NotificationsMasterId",
                principalTable: "NotificationsMaster",
                principalColumn: "Id");
        }
    }
}
