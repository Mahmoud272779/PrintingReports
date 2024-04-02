using App.Application.Handlers.AttendLeaving.AttendLeaving_Helper;
using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation;
using App.Application.Handlers.AttendLeaving.Reports.ReportHelper;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Domain.Entities.Process.Store;
using App.Domain.Models.Response.HR.AttendLeaving.Reports;
using App.Infrastructure.settings;
using DocumentFormat.OpenXml.EMMA;
using MediatR;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.DetailedAttendance
{
    public class DetailedAttendanceHandler : IRequestHandler<DetailedAttendanceRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryQuery<MoviedTransactions> _MoviedTransactionsQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployees;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery;
        private readonly IMediator _mediator;
        private readonly IRepositoryQuery<RamadanDate> _RamadanDateQuery;

        public DetailedAttendanceHandler(IRepositoryQuery<GLBranch> gLBranchQuery, IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<MoviedTransactions> moviedTransactionsQuery, IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployees, IMediator mediator, IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesQuery, IRepositoryQuery<RamadanDate> ramadanDateQuery)
        {
            _GLBranchQuery = gLBranchQuery;
            _InvEmployeesQuery = invEmployeesQuery;
            _MoviedTransactionsQuery = moviedTransactionsQuery;
            _HolidaysEmployees = holidaysEmployees;
            _mediator = mediator;
            _VaccationEmployeesQuery = vaccationEmployeesQuery;
            _RamadanDateQuery = ramadanDateQuery;
        }

        public async Task<ResponseResult> Handle(DetailedAttendanceRequest request, CancellationToken cancellationToken)
        {

            var calcRes = await _mediator.Send(new TimeCalculationRequest());
            if (calcRes.Result != Result.Success)
                return calcRes;

            var employees = _InvEmployeesQuery
                                .TableNoTracking
                                .Include(c => c.shiftsMaster)
                                .ThenInclude(c => c.changefulTimeGroups)
                                .ThenInclude(c => c.changefulTimeDays)
                                .Include(c => c.shiftsMaster.normalShiftDetalies)
                                .Include(c => c.Job)
                                .Include(c => c.Sections)
                                .Include(c => c.Departments)
                                .Where(c => c.Status != (int)Status.newElement)
                                .Where(c => request.empId != null ? c.Id == request.empId : true)
                                .Where(c => request.debartmentId != null ? c.DepartmentsId == request.debartmentId : true)
                                .Where(c => request.sectionId != null ? c.SectionsId == request.sectionId : true)
                                .Where(c => request.empGroupId != null ? c.employeesGroupId == request.empGroupId : true);

            var branches = _GLBranchQuery.TableNoTracking.Where(c => request.branchId != null ? request.branchId == request.branchId : true);

            var days = DatesService.GetDatesBetween(request.dateFrom, request.dateTo);

            var movedTransactions = _MoviedTransactionsQuery
                .TableNoTracking
                .Where(c => c.day.Date >= request.dateFrom.Date && c.day.Date <= request.dateTo.Date)
                .Where(c => employees.Select(x => x.Id).ToArray().Contains(c.EmployeesId));

            List<DetailedAttendanceResponseDTO_Branches> ListOf_branches = new List<DetailedAttendanceResponseDTO_Branches>();
            foreach (var branch in branches)
            {
                DetailedAttendanceResponseDTO_Branches thisBranch = new DetailedAttendanceResponseDTO_Branches
                {
                    Id = branch.Id,
                    arabicName = branch.ArabicName,
                    latinName = branch.LatinName
                };
                thisBranch.employees = new List<DetailedAttendanceResponseDTO_Employees>();
                var emps = employees.Where(c => c.gLBranchId == branch.Id);
                foreach (var emp in emps)
                {
                    List<DetailedAttendanceResponseDTO_Days> ListOf_days = new List<DetailedAttendanceResponseDTO_Days>();

                    foreach (var day in days)
                    {
                        var currentTransaction = movedTransactions.Where(x => x.day.Date == day.Date && x.EmployeesId == emp.Id).FirstOrDefault();

                        var status = await _mediator.Send(new AttendLeaving_GetStatusRequest { day = day, employees = emp, transactions = currentTransaction });

                        if (currentTransaction == null)
                        {

                            ListOf_days.Add(new DetailedAttendanceResponseDTO_Days
                            {
                                date = day.ToString(defultData.datetimeFormat),
                                dayAr = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().arabicName,
                                dayEn = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().latinName,
                                branchId = branch.Id,
                                employeeId = emp.Id,
                                statusAr = status.arabicName,
                                statusEn = status.latinName,
                                status = status.Id,
                                shift1_TimeIn = "____",
                                shift1_TimeOut = "____",
                                shift2_TimeIn = "____",
                                shift2_TimeOut = "____",
                                shift3_TimeIn = "____",
                                shift3_TimeOut = "____",
                                shift4_TimeIn = "____",
                                shift4_TimeOut = "____",
                                totalShiftTime = "____",
                                lateTime = "____",
                                extraTime = "____",
                                workingTime = "____",


                            });
                        }
                        else
                        {

                            var additionalTimes = AdditionalTimes.GenerateAdditionalTimes(currentTransaction, emp);

                            ListOf_days.Add(new DetailedAttendanceResponseDTO_Days
                            {
                                branchId = branch.Id,
                                employeeId = emp.Id,
                                date = day.ToString(defultData.datetimeFormat),
                                dayAr = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().arabicName,
                                dayEn = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().latinName,

                                shift1_TimeIn = currentTransaction.shift1_TimeIn?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                shift1_TimeOut = currentTransaction.shift1_TimeOut?.ToString("HH:mm") ?? defultData.EmptyAttendance,

                                shift2_TimeIn = currentTransaction.shift2_TimeIn?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                shift2_TimeOut = currentTransaction.shift2_TimeOut?.ToString("HH:mm") ?? defultData.EmptyAttendance,

                                shift3_TimeIn  = currentTransaction.shift3_TimeIn?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                shift3_TimeOut = currentTransaction.shift3_TimeOut?.ToString("HH:mm") ?? defultData.EmptyAttendance,

                                shift4_TimeIn = currentTransaction.shift4_TimeIn?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                shift4_TimeOut = currentTransaction.shift4_TimeOut?.ToString("HH:mm") ?? defultData.EmptyAttendance,

                                totalShiftTime = additionalTimes.totalShiftTime?.ToString(@"hh\:mm") ??"00:00",
                                lateTime = additionalTimes.lateTime?.ToString(@"hh\:mm") ?? "00:00",
                                extraTime = additionalTimes.extraTime?.ToString(@"hh\:mm") ?? "00:00",
                                workingTime = additionalTimes.workingTime?.ToString(@"hh\:mm") ?? "00:00",
                                statusAr = status.arabicName,
                                statusEn = status.latinName,
                                status = status.Id
                            });
                        }
                    }
                    thisBranch.employees.Add(new DetailedAttendanceResponseDTO_Employees
                    {
                        code = emp.Code,
                        Id = emp.Id,
                        branchId = branch.Id,
                        arabicName = emp.ArabicName,
                        latinName = emp.LatinName,
                        jobAr = emp.Job?.ArabicName ?? "",
                        jobEn = emp.Job?.LatinName ?? "",
                        shiftAr = emp.shiftsMaster?.arabicName ?? "",
                        shiftEn = emp.shiftsMaster?.latinName ?? "",
                        days = ListOf_days,
                        TotalExtraTime = DateTimeHelper.SumTimeSpans(ListOf_days.Where(c=> c.extraTime != defultData.EmptyAttendance).Select(c=> c.extraTime??"00:00").ToList()),
                        TotalLateTime = DateTimeHelper.SumTimeSpans(ListOf_days.Where(c => c.lateTime != defultData.EmptyAttendance).Select(c => c.lateTime ?? "00:00").ToList()),
                        TotalWorkingTime = DateTimeHelper.SumTimeSpans(ListOf_days.Where(c => c.workingTime != defultData.EmptyAttendance).Select(c => c.workingTime ?? "00:00").ToList()),
                    });
                }
                if (thisBranch.employees.Any())
                    ListOf_branches.Add(thisBranch);

            }
            return new ResponseResult
            {
                Result = Result.Success,
                Data = ListOf_branches
            };
        }
    }
}
