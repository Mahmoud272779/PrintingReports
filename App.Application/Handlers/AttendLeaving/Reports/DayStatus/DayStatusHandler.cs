using App.Application.Handlers.AttendLeaving.AttendanceMachines.GetMachines;
using App.Application.Handlers.AttendLeaving.AttendLeaving_Helper;
using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation;
using App.Application.Handlers.AttendLeaving.Reports.ReportHelper;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Domain.Models.Response.HR.AttendLeaving.Reports;
using App.Infrastructure.Migrations;
using App.Infrastructure.settings;
using App.Infrastructure.UserManagementDB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static App.Domain.Enums.Enums;

namespace App.Application.Handlers.AttendLeaving.Reports.DayStatus
{
    public class DayStatusHandler : IRequestHandler<DayStatusRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryQuery<MoviedTransactions> _MoviedTransactionsQuery;
        private readonly IRepositoryQuery<GLBranch> _BranchQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployees;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery;
        private readonly IMediator _mediator;
        private readonly IRepositoryQuery<RamadanDate> _RamadanDateQuery;

        public DayStatusHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<MoviedTransactions> moviedTransactionsQuery, IRepositoryQuery<GLBranch> branchQuery, IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployees, IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesQuery, IMediator mediator, IRepositoryQuery<RamadanDate> ramadanDateQuery)
        {
            _InvEmployeesQuery = invEmployeesQuery;
            _MoviedTransactionsQuery = moviedTransactionsQuery;
            _BranchQuery = branchQuery;
            _HolidaysEmployees = holidaysEmployees;
            _VaccationEmployeesQuery = vaccationEmployeesQuery;
            _mediator = mediator;
            _RamadanDateQuery = ramadanDateQuery;
        }

        public async Task<ResponseResult> Handle(DayStatusRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var calcRes = await _mediator.Send(new TimeCalculationRequest());
                if (calcRes.Result != Result.Success)
                    return calcRes;

                var days = DatesService.GetDatesBetween(request.dateFrom, request.dateTo);


                int[] Departments = null;
                int[] Sections = null;
                if (!string.IsNullOrEmpty(request.debartmentId))
                    Departments = request.debartmentId.Split(',').Select(c => int.Parse(c)).ToArray();
                if (!string.IsNullOrEmpty(request.sectionId))
                    Sections = request.sectionId.Split(',').Select(c => int.Parse(c)).ToArray();

                List<DayStatues_Days_Response> DaysList = new List<DayStatues_Days_Response>();
                var EmployeesList = _InvEmployeesQuery
                                    .TableNoTracking
                                    .Include(c => c.shiftsMaster)
                                    .ThenInclude(c => c.changefulTimeGroups)
                                    .ThenInclude(c => c.changefulTimeDays)
                                    .Include(c => c.shiftsMaster.normalShiftDetalies)
                                    .Include(c => c.Job)
                                    .Include(c => c.Sections)
                                    .Include(c => c.Departments)
                                    .Where(c => c.Status != (int)Status.newElement)
                                    .Where(c=> request.jobId != null ? c.JobId == request.jobId : true)
                                    .Where(c=> request.ShiftId != null ? c.shiftsMasterId == request.ShiftId : true)
                                    .Where(c => request.empId != null ? c.Id == request.empId : true)
                                    .Where(c => Departments != null ? Departments.Contains(c.DepartmentsId.Value) : true)
                                    .Where(c => Sections != null ? Sections.Contains(c.SectionsId.Value) : true)
                                    .Where(c => request.projectId != null ? c.projectsId == request.projectId : true)
                                    .Where(c => request.empGroupId != null ? c.employeesGroupId == request.empGroupId : true);

                var _BranchsListQuery = _BranchQuery
                    .TableNoTracking
                    .Include(c => c.Employees);

                var _AttendanceTransactions = await _MoviedTransactionsQuery.TableNoTracking.Where(x => x.day.Date >= request.dateFrom.Date && x.day.Date <= request.dateTo.Date).ToListAsync();
                foreach (var day in days)
                {
                    var branches = await GetDayStatuesBranches(request, day, EmployeesList, _BranchsListQuery, _AttendanceTransactions);
                    if (!branches.Any())
                        continue;
                    DaysList.Add(new DayStatues_Days_Response
                    {
                        DayStatues_Branches = branches,
                        day = day.ToString(defultData.datetimeFormat),
                        dayAr = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().arabicName,
                        dayEn = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().latinName,
                    });
                }

                //var result = ProcessAttendanceTransactions(_AttendanceTransactions, BranchesList);
                //return result;
                return new ResponseResult { Result = Result.Success, Data = DaysList };
            }
            catch (Exception ex)
            {
                return new ResponseResult { Result = Result.Failed, ErrorMessageEn = ex.ToString() };
            }
        }

        private async Task<List<DayStatues_Branches>> GetDayStatuesBranches(DayStatusRequest request, DateTime Day,
            IQueryable<InvEmployees> EmployeesList, IQueryable<GLBranch> _BranchsListQuery, List<MoviedTransactions> _AttendanceTransactions)
        {
            int[] branches = null;
            if (!string.IsNullOrEmpty(request.branchId))
                branches = request.branchId.Split(',').Select(c => int.Parse(c)).ToArray();


            var BranchesList = await _BranchsListQuery.Where(b => branches != null ? branches.Contains(b.Id) : true).ToListAsync();

            var listof_DayStatues_Branches = new List<DayStatues_Branches>();


            foreach (var item in BranchesList.Where(c => c.Employees.Any()))
            {
                var DayStatues_employees = await GetDayStatuesEmployees(EmployeesList, Day, _AttendanceTransactions, EmployeesList.Where(c => c.gLBranchId == item.Id).ToList());
                if (DayStatues_employees == null)
                    continue;
                if (!DayStatues_employees.Any())
                    continue;
                listof_DayStatues_Branches.Add(new DayStatues_Branches
                {
                    DayStatues_employees = DayStatues_employees,
                    day = Day.ToString(defultData.datetimeFormat),
                    branchId = item.Id,
                    arabicName = item.ArabicName,
                    latinName = item.LatinName,
                });
            }

            return listof_DayStatues_Branches;
        }

        public async Task<List<DayStatues_employees>> GetDayStatuesEmployees(IQueryable<InvEmployees> EmployeesList, DateTime day, List<MoviedTransactions> _AttendanceTransactions, List<InvEmployees> _Employees)
        {
            List<DayStatues_employees> DayStatues_employees = new List<DayStatues_employees>();
            foreach (var emp in _Employees)
            {
                var currenTransaction = _AttendanceTransactions.Where(c => c.day.Date == day.Date && c.EmployeesId == emp.Id).FirstOrDefault();
                var currentEmp = EmployeesList.Where(c => c.Id == emp.Id).FirstOrDefault();

                var status = await _mediator.Send(new AttendLeaving_GetStatusRequest { day = day, employees = emp, transactions = currenTransaction });

                var additionalTimes = currenTransaction != null ? AdditionalTimes.GenerateAdditionalTimes(currenTransaction, emp)
                    : new GenerateAdditionalTimesDTO
                    {
                        extraTime = new TimeSpan(),
                        lateTime = new TimeSpan(),
                        totalShiftTime = new TimeSpan(),
                        workingTime = new TimeSpan()
                    };
                DayStatues_employees.Add(new DayStatues_employees
                {
                    code = currentEmp.Code,
                    day = day.ToString(defultData.datetimeFormat),
                    branchId = currentEmp.gLBranchId,
                    arabicName = currentEmp.ArabicName,
                    latinName = currentEmp.LatinName,
                    shiftAr = currentEmp.shiftsMaster?.arabicName ?? "",
                    shiftEn = currentEmp.shiftsMaster?.latinName ?? "",


                    shift1_TimeIn = currenTransaction != null && currenTransaction?.shift1_TimeIn != null  ? currenTransaction.shift1_TimeIn.Value.ToString("HH:mm") : "____",
                    shift1_TimeOut = currenTransaction != null&& currenTransaction?.shift1_TimeOut != null  ? currenTransaction.shift1_TimeOut.Value.ToString("HH:mm") : "____",
                    shift2_TimeIn = currenTransaction != null && currenTransaction?.shift2_TimeIn != null  ? currenTransaction.shift2_TimeIn.Value.ToString("HH:mm") : "____",
                    shift2_TimeOut = currenTransaction != null&& currenTransaction?.shift2_TimeOut != null  ? currenTransaction.shift2_TimeOut.Value.ToString("HH:mm") : "____",
                    shift3_TimeIn = currenTransaction != null && currenTransaction?.shift3_TimeIn != null  ? currenTransaction.shift3_TimeIn.Value.ToString("HH:mm") : "____",
                    shift3_TimeOut = currenTransaction != null&& currenTransaction?.shift3_TimeOut != null  ? currenTransaction.shift3_TimeOut.Value.ToString("HH:mm") : "____",
                    shift4_TimeIn = currenTransaction != null && currenTransaction?.shift4_TimeIn != null  ? currenTransaction.shift4_TimeIn.Value.ToString("HH:mm") : "____",
                    shift4_TimeOut = currenTransaction != null&& currenTransaction?.shift4_TimeOut != null  ? currenTransaction.shift4_TimeOut.Value.ToString("HH:mm") : "____" ,


                    totalShiftTime = additionalTimes != null && additionalTimes.totalShiftTime != null ?Attendance_Totals.convertTimeSpanToString(additionalTimes.totalShiftTime.Value) : "____",
                    lateTime       = additionalTimes != null && additionalTimes.lateTime != null ?Attendance_Totals.convertTimeSpanToString(additionalTimes.lateTime.Value): "____",
                    ExtraTime      = additionalTimes != null && additionalTimes.extraTime != null ? Attendance_Totals.convertTimeSpanToString(additionalTimes.extraTime.Value): "____",
                    WorkTime       = additionalTimes != null && additionalTimes.workingTime != null ?Attendance_Totals.convertTimeSpanToString(additionalTimes.workingTime.Value) : "____",

                    dayStatusAr = status.arabicName,
                    dayStatusEn = status.latinName,
                });
            }
            return DayStatues_employees;
        }
    }
}
