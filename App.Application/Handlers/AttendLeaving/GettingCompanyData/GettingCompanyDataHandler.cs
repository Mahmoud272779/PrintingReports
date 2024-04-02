using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.Models;
using App.Application.Handlers.AttendLeaving.GettingCompanyData.GettingCompanyDataDTOS;
using App.Application.Services.HelperService.SecurityIntegrationServices;
using App.Domain.Entities.Process;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Domain.Entities.Process.Store;
using App.Domain.Models.Response.HR.AttendLeaving;
using Dapper;
using DocumentFormat.OpenXml.ExtendedProperties;
using MediatR;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.GettingCompanyData
{
    public class GettingCompanyDataHandler : IRequestHandler<GettingCompanyDataRequest, ResponseResult>
    {
        private readonly IConfiguration _configuration;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Vaccation> _VaccationCommand;
        private readonly IRepositoryCommand<Machines> _MachinesCommand;
        private readonly ISecurityIntegrationService _securityIntegrationService;
        private readonly IRepositoryQuery<InvJobs> _InvJobsQuery;
        private readonly IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Projects> _ProjectsQuery;
        private readonly IRepositoryQuery<NormalShiftDetalies> _NormalShiftDetaliesQuery;
        private readonly IRepositoryCommand<NormalShiftDetalies> _NormalShiftDetaliesCommand;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryCommand<InvEmployees> _InvEmployeesCommand;
        private readonly IRepositoryCommand<InvEmployeeBranch> _InvEmployeeBranchCommand;
        private readonly IRepositoryCommand<MachineTransactions> _MachineTransactionsCommand;
        private readonly IRepositoryQuery<Machines> _MachinesQuery;

        public GettingCompanyDataHandler(IConfiguration configuration, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Vaccation> vaccationCommand, ISecurityIntegrationService securityIntegrationService, IRepositoryCommand<Machines> machinesCommand, IRepositoryQuery<InvJobs> invJobsQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Projects> projectsQuery, IRepositoryQuery<NormalShiftDetalies> normalShiftDetaliesQuery, IRepositoryCommand<NormalShiftDetalies> normalShiftDetaliesCommand, IRepositoryCommand<InvEmployees> invEmployeesCommand, IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryCommand<MachineTransactions> machineTransactionsCommand, IRepositoryQuery<Machines> machinesQuery, IRepositoryCommand<InvEmployeeBranch> invEmployeeBranchCommand)
        {
            _configuration = configuration;
            _VaccationCommand = vaccationCommand;
            _securityIntegrationService = securityIntegrationService;
            _MachinesCommand = machinesCommand;
            _InvJobsQuery = invJobsQuery;
            _ProjectsQuery = projectsQuery;
            _NormalShiftDetaliesQuery = normalShiftDetaliesQuery;
            _NormalShiftDetaliesCommand = normalShiftDetaliesCommand;
            _InvEmployeesCommand = invEmployeesCommand;
            _InvEmployeesQuery = invEmployeesQuery;
            _MachineTransactionsCommand = machineTransactionsCommand;
            _MachinesQuery = machinesQuery;
            _InvEmployeeBranchCommand = invEmployeeBranchCommand;
        }
        private GettingDataDTO GettingData(string companyUniqueName)
        {
            var con = new SqlConnection(_configuration["ConnectionStrings:AttendLeaving"]);
            try
            {
                con.Open();
                var company = con.Query<companyEntity>($"select * from Companies where UniqueName = '{companyUniqueName}'").FirstOrDefault();
                var branchs = con.Query<BranshesEntity>($"select * from Branshes where CompanyID = {company.ID}").ToList();
                var BranchMachines = con.Query<BranchMachinesEntity>($"select * from BranchMachines where BranchId in ({string.Join(',', branchs.Select(x => x.Id).ToArray())})").ToList();
                var jobs = con.Query<jobsEntity>($"select * from Jobs where CompanyID = {company.ID}").ToList();
                var Projects = con.Query<ProjectsEntity>($"select * from Projects where CompanyID = {company.ID}").ToList();
                var Nationalities = con.Query<NationalitiesEntity>($"select * from Nationalities where CompanyID = {company.ID}").ToList();
                var missions = con.Query<WorkTasksEntity>($"select * from WorkTasks where CompanyID = {company.ID}").ToList();
                var employeesGroups = con.Query<EmployeeGroupsEntity>($"select * from EmployeeGroups where CompanyID = {company.ID}").ToList();
                var ShiftMaster = con.Query<ShiftsEntity>($"select * from Shifts where CompanyID = {company.ID}").ToList();
                var shiftDetalies = con.Query<shiftDetaliesEntity>($"select " +
                    $"t1.Id," +
                    $"t1.TotalDayHours," +
                    $"t1.IsVacation," +
                    $"t1.DayId," +
                    $"t2.StartIn," +
                    $"t2.StartOut," +
                    $"t2.EndIn," +
                    $"t2.EndOut," +
                    $"t2.AvailableEarlyTime," +
                    $"t2.AvailableLateTime," +
                    $"t1.ShiftType," +
                    $"t1.ShiftId," +
                    $"t2.Seq " +
                    $"from ShiftDays t1 join PeriodShiftTimes t2 on t2.ShiftDayId = t1.Id where t1.ShiftId in ({string.Join(',', ShiftMaster.Select(c => c.Id).ToArray())})").ToList();
                var Employees = con.Query<EmployeesEntity>($"select * from Employee where BranchId in ({string.Join(',', branchs.Select(x => x.Id).ToArray())})").ToList();
                var VacationDays = con.Query<VacationDaysEntity>($"select * from VacationDays where CompanyID = {company.ID}").ToList();
                var MachineTransactionsQuery = $"select * from MachineTransactions where MachineSN in ({string.Join(',', BranchMachines.Select(c => "'" + c.MachineSN + "'").ToArray())}) " +
                    $"union select * from Timmy_MachineTransactions where MachineSN in ({string.Join(',', BranchMachines.Select(c => "'" + c.MachineSN + "'").ToArray())})";
                var MachineTransactions = con.Query<MachineTransactionsEntity>(MachineTransactionsQuery).ToList();
                GettingDataDTO res = new GettingDataDTO
                {
                    companyEntity = company,
                    BranshesEntity = branchs,
                    BranchMachinesEntity = BranchMachines,
                    EmployeeGroupsEntity = employeesGroups,
                    EmployeesEntity = Employees,
                    jobsEntity = jobs,
                    MachineTransactionsEntity = MachineTransactions,
                    missionsEntity = missions,
                    NationalitiesEntity = Nationalities,
                    ProjectsEntity = Projects,
                    shiftDetaliesEntity = shiftDetalies,
                    ShiftsEntity = ShiftMaster,
                    VacationDaysEntity = VacationDays

                };
                return res;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }
        private async Task<string[]> addMachines(List<BranchMachinesEntity> BranchMachinesEntity)
        {
            var con = new SqlConnection(_configuration["ConnectionStrings:UserManagerConnection"]);
            try
            {
                con.Open();
                var company = await _securityIntegrationService.getCompanyInformation();
                var addingQuery = string.Empty;
                foreach (var SN in BranchMachinesEntity.Select(c => c.MachineSN).ToArray())
                {
                    addingQuery += "IF NOT EXISTS (" +
                        $"SELECT 1 " +
                        $"FROM [dbo].[AttendanceLeavingMachines] " +
                        $"WHERE [machineSN] = '{SN}') " +
                        $"BEGIN " +
                        $"INSERT INTO [dbo].[AttendanceLeavingMachines] ([machineSN], [userApplicationId]) " +
                        $"VALUES ('{SN}', {company.companyId}) " +
                        $"END;";
                }
                var IsAdded = con.Execute(addingQuery) > 0 ? true : false;

                var addedMachines = con.Query<string>($"select machineSN from AttendanceLeavingMachines where userApplicationId = {company.companyId}").ToArray();
                var Machines = new List<Machines>();
                foreach (var item in addedMachines)
                {
                    if (!BranchMachinesEntity.Any(c => c.MachineSN == item))
                        continue;
                    Machines.Add(new Domain.Entities.Process.AttendLeaving.Machines
                    {
                        branchId = 1,
                        arabicName = BranchMachinesEntity.FirstOrDefault(c => c.MachineSN == item).MachineNameAr,
                        latinName = BranchMachinesEntity.FirstOrDefault(c => c.MachineSN == item).MachineNameEn,
                        MachineSN = item
                    });
                }
                await _MachinesCommand.AddAsync(Machines);
                return addedMachines;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

        }
        private async Task InsertJobs(List<jobsEntity> jobsEntity)
        {
            var insertQuery = $"SET IDENTITY_INSERT {nameof(InvJobs)} ON;";
            var maxCode = _InvJobsQuery.GetMaxCode(c => c.Code);

            foreach (var item in jobsEntity)
            {
                maxCode++;
                insertQuery +=
                    $"MERGE INTO InvJobs AS target " +
                    $"USING (VALUES ({item.Id},{maxCode}, '{item.NameAr}', '{(!string.IsNullOrEmpty(item.NameEn) ? item.NameEn : item.NameAr)}',1,'',0))    " +
                    $"AS source ([Id],[Code], [ArabicName], [LatinName],[Status],[Notes],[CanDelete])   " +
                    $"ON target.Id = source.Id " +
                    $" WHEN MATCHED THEN   " +
                    $"UPDATE SET   " +
                    $"target.Code = source.Code,  " +
                    $"target.arabicName = source.arabicName,  " +
                    $"target.latinName = source.latinName,  " +
                    $"target.[Status] = source.[Status],  " +
                    $"target.[Notes] = source.[Notes],  " +
                    $"target.[CanDelete] = source.[CanDelete]  " +
                    $"WHEN NOT MATCHED BY TARGET THEN   " +
                    $"INSERT (Id,Code, arabicName, latinName,Status,Notes,CanDelete)   " +
                    $"VALUES (source.Id,source.Code, source.arabicName, source.latinName,source.Status,source.Notes,source.CanDelete);";
            }
            insertQuery += $"SET IDENTITY_INSERT {nameof(InvJobs)} OFF;";
            var con = new SqlConnection(_InvJobsQuery.ConnectionString());
            try
            {
                con.Execute(insertQuery);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
        }
        private async Task insertProjects(List<ProjectsEntity> ProjectsEntity)
        {
            var insertQuery = $"SET IDENTITY_INSERT Projects ON;";
            foreach (var item in ProjectsEntity)
            {
                insertQuery += $"MERGE INTO Projects AS target " +
                    $"USING (VALUES ({item.Id}, '{item.NameAr}', '{(!string.IsNullOrEmpty(item.NameEn) ? item.NameEn : item.NameAr)}')) AS source (Id, arabicName, latinName)  " +
                    $"ON target.Id = source.Id WHEN MATCHED THEN  UPDATE SET  target.arabicName = source.arabicName, target.latinName = source.latinName WHEN NOT MATCHED BY TARGET THEN  INSERT (Id, arabicName, latinName) VALUES (source.Id, source.arabicName, source.latinName);";
            }
            insertQuery += $"SET IDENTITY_INSERT Projects OFF;";
            var con = new SqlConnection(_ProjectsQuery.ConnectionString());
            try
            {
                con.Execute(insertQuery);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public async Task InsertNationalities(List<NationalitiesEntity> NationalitiesEntity)
        {
            var insertQuery = $"SET IDENTITY_INSERT {nameof(App.Domain.Entities.Process.AttendLeaving.Nationality)} ON;";
            foreach (var item in NationalitiesEntity)
            {
                insertQuery +=
                    $"MERGE INTO [dbo].{nameof(App.Domain.Entities.Process.AttendLeaving.Nationality)} AS target " +
                    $"USING (VALUES ({item.Id},'{item.NameAr}','{(!string.IsNullOrEmpty(item.NameEn) ? item.NameEn : item.NameAr)}')) AS source" +
                    $" (Id, arabicName,latinName) ON target.Id = source.Id " +
                    $"WHEN MATCHED THEN " +
                    $"UPDATE SET target.arabicName = source.arabicName, target.latinName = source.latinName " +
                    $"WHEN NOT MATCHED BY TARGET THEN INSERT (Id,arabicName, latinName) VALUES (source.Id,source.arabicName, source.latinName);";
            }

            insertQuery += $"SET IDENTITY_INSERT {nameof(App.Domain.Entities.Process.AttendLeaving.Nationality)} OFF;";
            var con = new SqlConnection(_ProjectsQuery.ConnectionString());
            try
            {
                con.Execute(insertQuery);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public async Task Insertmissions(List<WorkTasksEntity> missionsEntity)
        {
            var insertQuery = $"SET IDENTITY_INSERT {nameof(App.Domain.Entities.Process.AttendLeaving.Missions)} ON;";
            foreach (var item in missionsEntity)
            {
                insertQuery +=
                    $"MERGE INTO [dbo].{nameof(App.Domain.Entities.Process.AttendLeaving.Missions)} AS target " +
                    $"USING (VALUES ({item.Id},'{item.NameAr}','{(!string.IsNullOrEmpty(item.NameEn) ? item.NameEn : item.NameAr)}')) AS source" +
                    $" (Id, arabicName,latinName) ON target.Id = source.Id " +
                    $"WHEN MATCHED THEN " +
                    $"UPDATE SET target.arabicName = source.arabicName, target.latinName = source.latinName " +
                    $"WHEN NOT MATCHED BY TARGET THEN INSERT (Id,arabicName, latinName) VALUES (source.Id,source.arabicName, source.latinName);";
            }

            insertQuery += $"SET IDENTITY_INSERT {nameof(App.Domain.Entities.Process.AttendLeaving.Missions)} OFF;";
            var con = new SqlConnection(_ProjectsQuery.ConnectionString());
            try
            {
                con.Execute(insertQuery);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public async Task InsertEmployeesGroups(List<EmployeeGroupsEntity> EmployeeGroupsEntity)
        {
            var insertQuery = $"SET IDENTITY_INSERT {nameof(App.Domain.Entities.Process.AttendLeaving.EmployeesGroup)} ON;";
            foreach (var item in EmployeeGroupsEntity)
            {
                insertQuery +=
                    $"MERGE INTO [dbo].{nameof(App.Domain.Entities.Process.AttendLeaving.EmployeesGroup)} AS target " +
                    $"USING (VALUES ({item.Id},'{item.NameAr}','{item.NameEn}')) AS source" +
                    $" (Id, arabicName,latinName) ON target.Id = source.Id " +
                    $"WHEN MATCHED THEN " +
                    $"UPDATE SET target.arabicName = source.arabicName, target.latinName = source.latinName " +
                    $"WHEN NOT MATCHED BY TARGET THEN INSERT (Id,arabicName, latinName) VALUES (source.Id,source.arabicName, source.latinName);";
            }

            insertQuery += $"SET IDENTITY_INSERT {nameof(App.Domain.Entities.Process.AttendLeaving.EmployeesGroup)} OFF;";
            var con = new SqlConnection(_ProjectsQuery.ConnectionString());
            try
            {
                con.Execute(insertQuery);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public async Task InsertShiftMaster(List<ShiftsEntity> ShiftsEntity)
        {
            var insertQuery = $"SET IDENTITY_INSERT {nameof(App.Domain.Entities.Process.AttendLeaving.ShiftsMaster)} ON;";
            foreach (var item in ShiftsEntity)
            {
                insertQuery +=
                    $"MERGE INTO [dbo].{nameof(App.Domain.Entities.Process.AttendLeaving.ShiftsMaster)} AS target " +
                    $"USING (VALUES ({item.Id},'{item.NameAr}','{(!string.IsNullOrEmpty(item.NameEn) ? item.NameEn : item.NameAr)}','{item.DayEndTime}',{(item.IsShiftOpen ? (int)shiftTypes.openShift : (int)shiftTypes.normal)})) AS source" +
                    $" (Id, arabicName,latinName,dayEndTime,shiftType) " +
                    $"ON target.Id = source.Id " +
                    $"WHEN MATCHED THEN " +
                    $"UPDATE SET " +
                    $"target.arabicName = source.arabicName, " +
                    $"target.latinName = source.latinName, " +
                    $"target.dayEndTime = source.dayEndTime, " +
                    $"target.shiftType = source.shiftType " +
                    $"WHEN NOT MATCHED BY TARGET THEN " +
                    $"INSERT (Id,arabicName, latinName,dayEndTime,shiftType) " +
                    $"VALUES (source.Id,source.arabicName, source.latinName,source.dayEndTime,source.shiftType);";
            }

            insertQuery += $"SET IDENTITY_INSERT {nameof(App.Domain.Entities.Process.AttendLeaving.ShiftsMaster)} OFF;";
            var con = new SqlConnection(_ProjectsQuery.ConnectionString());
            try
            {
                con.Execute(insertQuery);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public async Task insertNormalAndOpenShiftDays(List<ShiftsEntity> ShiftsEntity)
        {
            await _NormalShiftDetaliesCommand.DeleteAsync(cx => ShiftsEntity.Select(c => c.Id).ToArray().Contains(cx.ShiftId));
            var listOf_NormalShiftDetalies = new List<NormalShiftDetalies>();
            foreach (var item in ShiftsEntity)
            {
                for (int i = 0; i < 7; i++)
                {
                    listOf_NormalShiftDetalies.Add(new NormalShiftDetalies
                    {
                        DayId = i + 1,
                        IsRamadan = false,
                        ShiftId = item.Id

                    });
                    listOf_NormalShiftDetalies.Add(new NormalShiftDetalies
                    {
                        DayId = i + 1,
                        IsRamadan = true,
                        ShiftId = item.Id
                    });
                }

            }
            await _NormalShiftDetaliesCommand.AddAsync(listOf_NormalShiftDetalies);
        }
        public async Task insertEmployees(List<EmployeesEntity> EmployeesEntity)
        {
            var listOf_InvEmployees = new List<InvEmployees>();
            var employees = _InvEmployeesQuery.TableNoTracking.ToList();

            foreach (var item in EmployeesEntity)
            {
                if (employees.Any(c => c.Code == item.EmployeeCode))
                {
                    var currentEmployee = employees.Find(c => c.Code == item.EmployeeCode);
                    currentEmployee.Status = (int)Enums.Status.Active;
                    currentEmployee.ArabicName = item.NameAr;
                    currentEmployee.LatinName = !string.IsNullOrEmpty(item.NameEn) ? item.NameEn : item.NameAr;
                    currentEmployee.JobId = item.JobId;
                    currentEmployee.shiftsMasterId = item.ShiftId;
                    currentEmployee.projectsId = item.ProjectId;
                    currentEmployee.missionsId = item.WorkTasksId;
                    currentEmployee.nationalityId = item.NationalitiesId;
                    currentEmployee.IDNumber = item.NationalityNumber;
                    currentEmployee.phone = item.PhoneNumber;
                    currentEmployee.email = item.Email;
                    currentEmployee.Deduction_of_delay_from_additional_time = item.ProvideDelayDiscountingOvertime;
                    currentEmployee.Calculating_extra_time_before_work = item.ExtraTimeBeforeWorking;
                    currentEmployee.Calculating_extra_time_after_work = item.ExtraTimeAfterWorking;
                    currentEmployee.Adding_working_hours_on_vacations = item.AddWorkingHoursOnHoliday;
                    currentEmployee.Auto_Dismissal_registration = item.LogOutWithoutFingerprint;
                    continue;
                }

                listOf_InvEmployees.Add(new InvEmployees
                {
                    Code = item.EmployeeCode,
                    Status = (int)Enums.Status.Active,
                    ArabicName = item.NameAr,
                    LatinName = item.NameEn,
                    JobId = item.JobId,
                    gLBranchId = 1,
                    shiftsMasterId = item.ShiftId,
                    projectsId = item.ProjectId,
                    missionsId = item.WorkTasksId,
                    nationalityId = item.NationalitiesId,
                    IDNumber = item.NationalityNumber,
                    phone = item.PhoneNumber,
                    email = item.Email,
                    Deduction_of_delay_from_additional_time = item.ProvideDelayDiscountingOvertime,
                    Calculating_extra_time_before_work = item.ExtraTimeBeforeWorking,
                    Calculating_extra_time_after_work = item.ExtraTimeAfterWorking,
                    Adding_working_hours_on_vacations = item.AddWorkingHoursOnHoliday,
                    Auto_Dismissal_registration = item.LogOutWithoutFingerprint
                });
            }
            await _InvEmployeesCommand.UpdateAsyn(employees);
            if (listOf_InvEmployees.Any())
            {
                await _InvEmployeesCommand.AddAsync(listOf_InvEmployees);
                var con = new SqlConnection(_InvJobsQuery.ConnectionString());
                try
                {
                    con.Open();
                    var branchInsert = "INSERT INTO InvEmployeesBranches (EmployeeId,BranchId,[current]) " +
                                        "(SELECT Id, 1,0 " +
                                        "FROM InvEmployees " +
                                        "WHERE NOT EXISTS ( " +
                                        "    SELECT 1 " +
                                        "    FROM InvEmployeesBranches " +
                                        "    WHERE InvEmployeesBranches.EmployeeId = InvEmployees.Id" +
                                        "))";
                    con.Execute(branchInsert);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }
            }
        }
        public async Task insertMachineTransaction(List<MachineTransactionsEntity> MachineTransactionsEntity)
        {
            var machines = _MachinesQuery.TableNoTracking;
            var data = MachineTransactionsEntity.Select(c => new MachineTransactions
            {
                isAuto = true,
                IsEdited = false,
                IsMoved = false,
                machineId = machines.FirstOrDefault(x => x.MachineSN == c.MachineSN)?.Id ?? machines.FirstOrDefault().Id,
                TransactionDate = c.TransactionDate,
                PushTime = c.PushTime,
                EmployeeCode = c.EmployeeID
            });
            await _MachineTransactionsCommand.AddAsync(data);
        }
        public async Task InsertVaccation(List<VacationDaysEntity> VacationDaysEntity)
        {
            var insertQuery = $"SET IDENTITY_INSERT {nameof(App.Domain.Entities.Process.AttendLeaving.Vaccation)} ON;";
            foreach (var item in VacationDaysEntity)
            {
                insertQuery +=
                    $"MERGE INTO [dbo].{nameof(App.Domain.Entities.Process.AttendLeaving.Vaccation)} AS target " +
                    $"USING (VALUES ({item.Id},'{item.NameAr}','{item.NameEn}')) AS source" +
                    $" (Id, arabicName,latinName) " +
                    $"ON target.Id = source.Id " +
                    $"WHEN MATCHED THEN " +
                    $"UPDATE SET " +
                    $"target.arabicName = source.arabicName, " +
                    $"target.latinName = source.latinName " +
                    $"WHEN NOT MATCHED BY TARGET THEN " +
                    $"INSERT (Id,arabicName, latinName) " +
                    $"VALUES (source.Id,source.arabicName, source.latinName);";
            }

            insertQuery += $"SET IDENTITY_INSERT {nameof(App.Domain.Entities.Process.AttendLeaving.Vaccation)} OFF;";
            var con = new SqlConnection(_ProjectsQuery.ConnectionString());
            try
            {
                con.Execute(insertQuery);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public async Task<ResponseResult> Handle(GettingCompanyDataRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var data = GettingData(request.companyUniqueName);
                await InsertVaccation(data.VacationDaysEntity);
                await addMachines(data.BranchMachinesEntity);
                await InsertJobs(data.jobsEntity);
                await insertProjects(data.ProjectsEntity);
                await InsertNationalities(data.NationalitiesEntity);
                await Insertmissions(data.missionsEntity);
                await InsertEmployeesGroups(data.EmployeeGroupsEntity);
                await InsertShiftMaster(data.ShiftsEntity);
                await insertNormalAndOpenShiftDays(data.ShiftsEntity);
                await insertEmployees(data.EmployeesEntity);
                await insertMachineTransaction(data.MachineTransactionsEntity);
                return new ResponseResult
                {
                    Result = Result.Success
                };

            }
            catch (Exception ex)
            {

                return new ResponseResult
                {
                    Result = Result.Failed,
                    Note = ex.ToString()
                };
            }



        }
    }
}
