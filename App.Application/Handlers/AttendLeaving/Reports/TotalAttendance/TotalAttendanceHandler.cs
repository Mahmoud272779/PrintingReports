using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation;
using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.Models;
using App.Application.Handlers.AttendLeaving.Reports.ReportHelper;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.GetMasterShift;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Domain.Models.Response.HR.AttendLeaving;
using App.Infrastructure.settings;
using App.Infrastructure.UserManagementDB;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Hangfire.Common;
using MediatR;
using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.TotalAttendance
{
    public class TotalAttendanceHandler : IRequestHandler<TotalAttendanceRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<MoviedTransactions> _MoviedTransactionsQuery;
        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;
        private readonly IMediator _mediator;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployees;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery;
        private readonly IRepositoryQuery<RamadanDate> _RamadanDateQuery;

        public TotalAttendanceHandler(IRepositoryQuery<MoviedTransactions> moviedTransactionsQuery, IRepositoryQuery<GLBranch> gLBranchQuery, IMediator mediator, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployees, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesQuery, IRepositoryQuery<RamadanDate> ramadanDateQuery)
        {
            _MoviedTransactionsQuery = moviedTransactionsQuery;
            _GLBranchQuery = gLBranchQuery;
            _mediator = mediator;
            _HolidaysEmployees = holidaysEmployees;
            _VaccationEmployeesQuery = vaccationEmployeesQuery;
            _RamadanDateQuery = ramadanDateQuery;
        }

        public async Task<ResponseResult> Handle(TotalAttendanceRequest request, CancellationToken cancellationToken)
        {
            var calcRes = await _mediator.Send(new TimeCalculationRequest());
            if (calcRes.Result != Result.Success)
                return calcRes;
            var brancheIds = request.branchIds.Split(',').Select(c => int.Parse(c)).ToArray();
            int[] managementIds = null;
            if (!string.IsNullOrEmpty(request.managementIds))
                managementIds = request.managementIds.Split(',').Select(c => int.Parse(c)).ToArray();
            int[] sectionsIds = null;
            if (!string.IsNullOrEmpty(request.sectionsIds))
                sectionsIds = request.sectionsIds.Split(',').Select(c => int.Parse(c)).ToArray();

            var data = _MoviedTransactionsQuery
                        .TableNoTracking
                        .Include(c => c.Employees)
                        .ThenInclude(c => c.shiftsMaster)
                        .ThenInclude(c=> c.changefulTimeGroups)
                        .ThenInclude(c=> c.changefulTimeDays)
                        .Include(c=> c.Employees.shiftsMaster.normalShiftDetalies)
                        .Where(c => c.day.Date >= request.dateFrom.Date && c.day.Date <= request.dateTo.Date)
                        .Where(c => request.employeeId != null ? c.EmployeesId == request.employeeId : true)
                        .Where(c => request.branchIds != "0" && !string.IsNullOrEmpty(request.branchIds) ? brancheIds.Contains(c.Employees.gLBranchId) : true)
                        .Where(c => request.managementIds != "0" && !string.IsNullOrEmpty(request.managementIds) ? managementIds.Contains(c.Employees.gLBranchId) : true)
                        .Where(c => request.sectionsIds != "0" && !string.IsNullOrEmpty(request.sectionsIds) ? sectionsIds.Contains(c.Employees.gLBranchId) : true)
                        .Where(c => request.projectId != null ? c.Employees.projectsId == request.projectId : true)
                        .Where(c => request.jobId != null ? c.Employees.JobId == request.jobId : true)
                        .Where(c => request.employeeGroupId != null ? c.Employees.employeesGroupId == request.employeeGroupId : true)
                        .Where(c => request.shiftId != null ? c.Employees.shiftsMasterId == request.shiftId : true)
                        .Include(c => c.Employees.GLBranch)
                        .Include(c => c.Employees.Job)
                        .ToList()
                        .GroupBy(c => c.Employees.gLBranchId);
            List<TotalAttendanceDTO_Branches_root> branches = new List<TotalAttendanceDTO_Branches_root>();
            foreach (var item in data)
            {
                List<TotalAttendanceDTO_Employees> ListOf_Employees = new List<TotalAttendanceDTO_Employees>();
                var employeee = item.Select(c => c.Employees).GroupBy(c => c.Id).Select(c => c.FirstOrDefault()).ToList();
                foreach (var emp in employeee)
                {
                    TotalAttendanceDTO_Employees NewEmp = new TotalAttendanceDTO_Employees();

                    NewEmp.Id = emp.Id;

                    NewEmp.code = emp.Code;
                    NewEmp.arabicName = emp.ArabicName;
                    NewEmp.latinName = emp.LatinName;
                    NewEmp.employee = new TotalAttendance_Emp
                    {
                        Id = NewEmp.Id,
                        arabicName = NewEmp.arabicName,
                        latinName = NewEmp.latinName,
                        code = NewEmp.code,
                    };
                    NewEmp.branchId = emp.gLBranchId;
                    NewEmp.jobAr = emp.Job != null ? emp.Job.ArabicName : "";
                    NewEmp.jobEn = emp.Job != null ? emp.Job.LatinName : "";
                    NewEmp.shiftAr = emp.shiftsMaster != null ? emp.shiftsMaster.arabicName : "";
                    NewEmp.shiftEn = emp.shiftsMaster != null ? emp.shiftsMaster.latinName : "";
                    NewEmp.job = emp.Job != null ? new TotalAttendance_job
                    {
                        Id = emp.Job.Id,
                        arabicName = emp.Job.ArabicName,
                        latinName = emp.LatinName
                    } : new TotalAttendance_job
                    {
                        Id = 0,
                        arabicName = "",
                        latinName = ""
                    };
                    NewEmp.shift = emp.shiftsMaster != null ? new TotalAttendance_Shift
                    {
                        Id = emp.shiftsMaster.Id,
                        arabicName = emp.shiftsMaster.arabicName,
                        latinName = emp.shiftsMaster.latinName
                    } : null;

                    NewEmp.totalHours = Attendance_Totals.totalHours(item.Where(c => c.EmployeesId == emp.Id).ToList(), request.dateFrom, request.dateTo, emp.shiftsMaster, _HolidaysEmployees, _VaccationEmployeesQuery, _RamadanDateQuery,_mediator);
                    //if (NewEmp.totalHours == "00:00") NewEmp.totalHours = defultData.EmptyAttendance;

                    NewEmp.actualWorkingHours = Attendance_Totals.actualWorkingHours(item.Where(c => c.EmployeesId == emp.Id).ToList());
                    //if (NewEmp.actualWorkingHours == "00:00") NewEmp.actualWorkingHours = defultData.EmptyAttendance;

                    NewEmp.absence = Attendance_Totals.total_absence(item.Where(c => c.EmployeesId == emp.Id).ToList(), request.dateFrom, request.dateTo, _HolidaysEmployees, _VaccationEmployeesQuery, _RamadanDateQuery, _mediator);
                    //if (NewEmp.absence == "00:00") NewEmp.absence = defultData.EmptyAttendance;

                    NewEmp.delay = Attendance_Totals.total_delay(item.Where(c => c.EmployeesId == emp.Id).Where(c => c.EmployeesId == emp.Id).ToList());
                    //if (NewEmp.delay == "00:00") NewEmp.delay = defultData.EmptyAttendance;

                    NewEmp.extraTime = Attendance_Totals.total_extraTime(item.Where(c => c.EmployeesId == emp.Id).ToList());
                    //if (NewEmp.extraTime == "00:00") NewEmp.extraTime = defultData.EmptyAttendance;

                    NewEmp.vacations = Attendance_Totals.total_vacations(item.Where(c => c.EmployeesId == emp.Id).ToList(), request.dateFrom, request.dateTo, _HolidaysEmployees, _VaccationEmployeesQuery, _RamadanDateQuery, _mediator);
                    //if (NewEmp.vacations == "00:00") NewEmp.vacations = defultData.EmptyAttendance;
                    ListOf_Employees.Add(NewEmp);
                }

                TotalAttendanceDTO_Branches_root branch = new TotalAttendanceDTO_Branches_root
                {
                    Id = item.First().Employees.GLBranch.Id,
                    arabicName = item.First().Employees.GLBranch.ArabicName,
                    latinName = item.First().Employees.GLBranch.LatinName,
                    employees = ListOf_Employees
                };



                branches.Add(branch);
            }


            return new ResponseResult
            {
                Result = Result.Success,
                Data = branches
            };
        }
    }
}
