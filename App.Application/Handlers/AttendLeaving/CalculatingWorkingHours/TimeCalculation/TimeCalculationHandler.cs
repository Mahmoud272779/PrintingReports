using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.TimeCalculationHelper;
using App.Application.Services.HelperService.SecurityIntegrationServices;
using App.Application.SignalRHub;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Shift;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Domain.Entities.Process.General;
using App.Infrastructure.settings;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Caching;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation
{
    public class TimeCalculationHandler : IRequestHandler<TimeCalculationRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryQuery<MachineTransactions> _MachineTransactionsQuery;
        private readonly IRepositoryQuery<MoviedTransactions> _MoviedTransactionsQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings> _AttendLeaving_SettingsQuery;
        private readonly IRepositoryQuery<ChangefulTimeGroupsEmployees> _ChangefulTimeGroupsEmployeesQuery;
        private readonly IRepositoryQuery<signalR> _signalRQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.AttendancPermission> _AttendancPermissionQuery;

        private readonly IRepositoryCommand<MachineTransactions> _MachineTransactionsCommand;
        private readonly IRepositoryCommand<MoviedTransactions> _MoviedTransactionsCommand;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IRepositoryQuery<RamadanDate> _RamadanDateQuery;
        private readonly IMemoryCache _memoryCache;
        private readonly ISecurityIntegrationService _securityIntegrationService;

        private readonly IConfiguration _configuration;
        public TimeCalculationHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<MachineTransactions> machineTransactionsQuery, IRepositoryQuery<MoviedTransactions> moviedTransactionsQuery, IRepositoryCommand<MachineTransactions> machineTransactionsCommand, IRepositoryCommand<MoviedTransactions> moviedTransactionsCommand, IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings> attendLeaving_SettingsQuery, IRepositoryQuery<ChangefulTimeGroupsEmployees> changefulTimeGroupsEmployeesQuery, IConfiguration configuration, IHubContext<NotificationHub> hub, IRepositoryQuery<signalR> signalRQuery, IRepositoryQuery<RamadanDate> ramadanDateQuery, IMemoryCache memoryCache, ISecurityIntegrationService securityIntegrationService, IRepositoryQuery<AttendancPermission> attendancPermissionQuery)
        {
            _InvEmployeesQuery = invEmployeesQuery;
            _MachineTransactionsQuery = machineTransactionsQuery;
            _MoviedTransactionsQuery = moviedTransactionsQuery;
            _MachineTransactionsCommand = machineTransactionsCommand;
            _MoviedTransactionsCommand = moviedTransactionsCommand;
            _AttendLeaving_SettingsQuery = attendLeaving_SettingsQuery;
            _ChangefulTimeGroupsEmployeesQuery = changefulTimeGroupsEmployeesQuery;
            _configuration = configuration;
            _hub = hub;
            _signalRQuery = signalRQuery;
            _RamadanDateQuery = ramadanDateQuery;
            _memoryCache = memoryCache;
            _securityIntegrationService = securityIntegrationService;
            _AttendancPermissionQuery = attendancPermissionQuery;
        }
        public async Task<ResponseResult> Handle(TimeCalculationRequest request, CancellationToken cancellationToken)
        {
            var zeroTime = new TimeSpan();
            var company = await _securityIntegrationService.getCompanyInformation();
            var settings = _AttendLeaving_SettingsQuery.TableNoTracking.FirstOrDefault();
            var connectionStriong = ConnectionString.connectionString(_configuration, _MoviedTransactionsQuery.databaseName());
            SqlConnection con = new SqlConnection(connectionStriong);
            var employees = _InvEmployeesQuery.TableNoTracking
                            .Include(c => c.shiftsMaster)
                            .ThenInclude(c => c.normalShiftDetalies)
                            .Include(c => c.shiftsMaster.changefulTimeGroups)
                            .ThenInclude(c => c.changefulTimeDays)
                            .Where(c => c.shiftsMaster != null)
                            .Where(c => request.employeesCode != null ? request.employeesCode.Contains(c.Code) : true)
                            .Where(c => c.shiftsMaster.shiftType == (int)Enums.shiftTypes.openShift || c.shiftsMaster.shiftType == (int)Enums.shiftTypes.normal ? c.shiftsMaster.normalShiftDetalies.Any(c => c.TotalDayHours > zeroTime) : true)
                            .Where(c => c.shiftsMaster.shiftType == (int)Enums.shiftTypes.ChangefulTime ? c.shiftsMaster.changefulTimeGroups.First().changefulTimeDays.Any() : true)
                            .ToList();


            var MachinesTransaction = _MachineTransactionsQuery
                .TableNoTracking
                .Include(c => c.machine)
                .Where(c => !c.IsMoved && employees.Select(x => x.Code).Contains(c.EmployeeCode))
                .OrderBy(c => c.TransactionDate.Date)
                .ToList();

            List<int> empCodes = new List<int>();


            var totalCount = MachinesTransaction.GroupBy(c => new { c.EmployeeCode, c.TransactionDate.Date }).Count();
            int counter = 0;


            if (totalCount == 0) return new ResponseResult { Result = Result.Success };
            var listOfUpdated_MoviedTransactions = new List<MoviedTransactions>();
            //var usersSignalRConnectionId = _signalRQuery.TableNoTracking.Select(c => c.connectionId);
            con.Open();


            var companiesInProcess = _memoryCache.Get<List<string>>(defultData.AttendCalcProcess);

            try
            {
                if (companiesInProcess == null)
                {
                    List<string> co = new List<string>();
                    co.Add(company.companyLogin);
                    _memoryCache.Set(defultData.AttendCalcProcess, co, DateTimeOffset.Now.AddMinutes(15));

                }
                else if (!companiesInProcess.Where(c => c == company.companyLogin).Any())
                {
                    List<string> co = new List<string>();
                    co.Add(company.companyLogin);
                    _memoryCache.Set(defultData.AttendCalcProcess, co);
                }
                else
                {
                    if (companiesInProcess.Any(c => c == company.companyLogin))
                        return new ResponseResult
                        {
                            Result = Result.Failed,
                            Alart = new Alart
                            {
                                AlartType = AlartType.information,
                                type = AlartShow.popup,
                                titleAr = "تنبية",
                                titleEn = "Alart",
                                MessageAr = "يرجى أخذ العلم بأن عملية حساب الحضور جارية حالياً. يُرجى الانتظار حتى الانتهاء من العملية وتأكيد النتائج. شكرًا لصبركم وتفهمكم.",
                                MessageEn = "Please be informed that attendance calculation is currently in progress. Kindly await confirmation once the process is completed."

                            }
                        };
                }
                while (employees.Any())
                {
                    //Thread.Sleep(100);
                    var emp = employees.FirstOrDefault();
                    //usersSignalRConnectionId = _signalRQuery.TableNoTracking.Select(c => c.connectionId);
                    if (emp.shiftsMaster == null)
                    {
                        employees.Remove(employees.FirstOrDefault());
                        continue;
                    }
                    var employee_MachineTranscation = MachinesTransaction.Where(c => c.EmployeeCode == emp.Code);
                    if (!employee_MachineTranscation.Any())
                    {
                        employees.Remove(employees.FirstOrDefault());
                        continue;
                    }
                    var currentEmployeeTransactions = employee_MachineTranscation.GroupBy(c => !c.IsEdited ? c.TransactionDate.Date : c.EditedTransactionDate.Date).Select(c => c.FirstOrDefault()).ToList();
                    try
                    {
                        while (currentEmployeeTransactions.Any())
                        {
                            //Thread.Sleep(100);

                            var item = currentEmployeeTransactions.FirstOrDefault();




                            var Nextday = !item.IsEdited ? item.TransactionDate.Date : item.EditedTransactionDate;
                            if (!MachinesTransaction.Any(c => c.EmployeeCode == emp.Code && !c.IsEdited ? c.TransactionDate.Date == Nextday.Date : c.EditedTransactionDate.Date == Nextday.Date))
                            {
                                continue;
                            }
                            var currentDay = _MoviedTransactionsQuery.TableNoTracking.Where(c => c.day.Date == Nextday.Date && c.EmployeesId == emp.Id).ToList().FirstOrDefault();

                            if (currentDay == null)
                            {

                                var qurey = $"INSERT INTO [dbo].[MoviedTransactions] ([EmployeesId],[day],[cDate]) VALUES ({emp.Id},'{Nextday.Date.ToString(defultData.datetimeFormat)}',GETDATE())";
                                await con.ExecuteAsync(qurey);
                                currentDay = _MoviedTransactionsQuery.TableNoTracking.Where(c => c.day.Date == Nextday.Date && c.EmployeesId == emp.Id).ToList().FirstOrDefault();
                            }

                            var day = await CalcExistEmployeeDay.calcExistEmployeeDay(emp, currentDay, MachinesTransaction.Where(c => c.EmployeeCode == emp.Code).ToList(), settings, _ChangefulTimeGroupsEmployeesQuery, _RamadanDateQuery, _AttendancPermissionQuery);
                            if (day == null)
                            {
                                currentEmployeeTransactions.Remove(currentEmployeeTransactions.FirstOrDefault());
                                continue;
                            }
                            listOfUpdated_MoviedTransactions.Add(day);
                            counter++;
                            try
                            {
                                await Notify(company.companyLogin, counter, totalCount, employees.FirstOrDefault());

                            }
                            catch (Exception)
                            {

                            }
                            currentEmployeeTransactions.Remove(currentEmployeeTransactions.FirstOrDefault());
                            Thread.Sleep(200);
                        }

                    }
                    catch (Exception ex)
                    {
                        logger(ex.ToString());
                    }

                    empCodes.Add(employees.FirstOrDefault().Code);
                    employees.Remove(employees.FirstOrDefault());
                }

                await Notify(company.companyLogin, counter, totalCount, null, false,true);

                var moved = await _MoviedTransactionsCommand.UpdateAsyn(listOfUpdated_MoviedTransactions);

                if (moved)
                {
                    try
                    {
                        if (MachinesTransaction.Any())
                        {
                            var query = $"update MachineTransactions set IsMoved = 1 where EmployeeCode in ({string.Join(',', empCodes)});";
                            con.Execute(query);
                        }

                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                    }

                }
            }
            catch (Exception ex)
            {
                logger(ex.ToString());

                throw;
            }
            finally
            {
                if (companiesInProcess != null)
                {

                    companiesInProcess.Remove(company.companyLogin);
                    _memoryCache.Set(defultData.AttendCalcProcess, companiesInProcess);
                }
                con.Close();
            }

            if (companiesInProcess != null)
            {

                companiesInProcess.Remove(company.companyLogin);
                _memoryCache.Set(defultData.AttendCalcProcess, companiesInProcess);
            }


            await Notify(company.companyLogin, counter, totalCount,null, true);

            return new ResponseResult
            {
                Result = Result.Success
            };
        }
        private void logger(string ex)
        {

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "errorFolder");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, DateTime.Now.ToString("yyyyMMddTHHmmss"));
            File.Create(filePath).Close();
            File.AppendAllText(filePath, ex);
        }
        private async Task Notify(string companyLogin, int counter, int totalCount, InvEmployees emp, bool isFinshed = false,bool isSavingData = false)
        {
            var _signalRCash = _memoryCache.Get<List<SignalRCash>>(defultData.SignalRKey);
            if (_signalRCash == null)
                return;
            if (_signalRCash.Any())
            {
                var usersSignalRConnectionId = _signalRCash.Where(c => c.CompanyLogin == companyLogin).Select(c => c.connectionId).ToArray();
                if (usersSignalRConnectionId.Any())
                {
                    var percntage = (Convert.ToDouble(counter) / Convert.ToDouble(totalCount)) * 100;
                    await _hub.Clients
                        .Clients(usersSignalRConnectionId)
                        .SendAsync(defultData.attandanceCalc,
                        new porgressData
                        {
                            percentage = percntage,
                            noteAr = isFinshed ?"تم الانتهاء": (isSavingData ? "جاري حفظ البيانات" : $"جاري ترحيل حركات الموظف :{emp.ArabicName}  {counter}/{totalCount}"),
                            noteEn = isFinshed ?"Finished"  : (isSavingData ? "Saving data" : $"Attendance Progress of employee :{emp.LatinName}  {totalCount}/{counter}"),
                            Count = counter,
                            totalCount = totalCount,
                            status = isFinshed ? Aliases.ProgressStatus.ProgressFinshed : Aliases.ProgressStatus.InProgress
                        });
                }
            }
        }
    }
}
