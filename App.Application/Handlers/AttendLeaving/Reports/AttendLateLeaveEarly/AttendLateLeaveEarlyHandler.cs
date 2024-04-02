using System.Threading;
using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation;
using App.Application.Handlers.AttendLeaving.Reports.ReportHelper;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Infrastructure.settings;
using App.Infrastructure;
using MediatR;
using App.Domain.Entities.Process.AttendLeaving;
using App.Application.Handlers.AttendLeaving.AttendLeaving_Helper;

namespace App.Application.Handlers.AttendLeaving.Reports.AttendLateLeaveEarly
{
    public class AttendLateLeaveEarlyHandler : IRequestHandler<AttendLateLeaveEarlyRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.InvEmployees> _employeesQuery;
        private readonly IRepositoryQuery<MoviedTransactions> _MoviedTransactionsQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployees;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery;
        private readonly IRepositoryQuery<RamadanDate> _RamadanDateQuery;
        private readonly IMediator _mediator;
        public AttendLateLeaveEarlyHandler(IRepositoryQuery<GLBranch> gLBranchQuery, IRepositoryQuery<InvEmployees> employeesQuery, IRepositoryQuery<MoviedTransactions> moviedTransactionsQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployees, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesQuery, IMediator mediator, IRepositoryQuery<RamadanDate> ramadanDateQuery)
        {
            _GLBranchQuery = gLBranchQuery;
            _employeesQuery = employeesQuery;
            _MoviedTransactionsQuery = moviedTransactionsQuery;
            _HolidaysEmployees = holidaysEmployees;
            _VaccationEmployeesQuery = vaccationEmployeesQuery;
            _mediator = mediator;
            _RamadanDateQuery = ramadanDateQuery;
        }
        public async Task<ResponseResult> Handle(AttendLateLeaveEarlyRequest request, CancellationToken cancellationToken)
        {
            var calcRes = await _mediator.Send(new TimeCalculationRequest());
            if (calcRes.Result != Result.Success)
                return calcRes;
            if (request.DateFrom > request.DateTo)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.StartDateAfterEndDate,
                        MessageEn = ErrorMessagesEn.StartDateAfterEndDate,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            int[] branches = null;
            if (!string.IsNullOrEmpty(request.BranchId))
                branches = request.BranchId.Split(',').Select(c => int.Parse(c)).ToArray();

            var branchesObjs = _GLBranchQuery.TableNoTracking
                .Where(c => (branches != null && request.BranchId != "0" && branches.Any()) ? branches.Contains(c.Id) : true).ToList();

            var employees = _employeesQuery.TableNoTracking
                                .Include(c => c.shiftsMaster)
                                .ThenInclude(c => c.changefulTimeGroups)
                                .ThenInclude(c => c.changefulTimeDays)
                                .Include(c => c.shiftsMaster.normalShiftDetalies)
                                .Include(c => c.Job)
                                .Include(c => c.Sections)
                                .Include(c => c.Departments).Where(c => request.EmpId != null ? c.Id == request.EmpId : true)
                   .Where(c => request.DepartmentId != null ? c.DepartmentsId == request.DepartmentId : true)
                   .Where(c => request.SectionId != null ? c.SectionsId == request.SectionId : true)
                   .Where(c => (branches != null && branches.Any()) ? branches.Contains(c.gLBranchId) : true)
                   .Where(c => request.JobId != null ? c.JobId == request.JobId : true)
                   .Where(c => request.GroupId != null ? c.employeesGroupId == request.GroupId : true)
                   .Where(c => request.ShiftmasterId != null ? c.shiftsMasterId == request.ShiftmasterId : true)
                   .Where(c => request.projectId != null ? c.projectsId == request.projectId : true)
                   .ToList();
            var days = DatesService.GetDatesBetween(request.DateFrom, request.DateTo).ToList();

            var movedTransactions = _MoviedTransactionsQuery
                   .TableNoTracking
                   ?.Where(c => c.day.Day >= request.DateFrom.Day && c.day.Day <= request.DateTo.Day)
                   ?.Where(c => employees.Select(x => x.Id).ToArray().Contains(c.EmployeesId)).ToList();

            List<AttendLateLeaveEarlyResponseDTO_Branches> ListOf_branches = new List<AttendLateLeaveEarlyResponseDTO_Branches>();

            foreach (var branch in branchesObjs)
            {
                AttendLateLeaveEarlyResponseDTO_Branches thisBranch = new AttendLateLeaveEarlyResponseDTO_Branches
                {
                    Id = branch.Id,
                    arabicName = branch.ArabicName,
                    latinName = branch.LatinName
                };
                thisBranch.employees = new List<AttendLateLeaveEarlyResponseDTO_Employees>();
                var emps = employees.Where(c => c.gLBranchId == branch.Id);
                var zeroTimeSpan = new TimeSpan();

                foreach (var emp in emps)
                {
                    var empAttendLateTime = new TimeSpan();
                    var empLeaveEarlyTime = new TimeSpan();
                    List<AttendLateLeaveEarlyResponseDTO_Days> ListOf_days = new List<AttendLateLeaveEarlyResponseDTO_Days>();



                    foreach (var day in days)
                    {
                        var currentTransaction = movedTransactions.Where(x => x.day.Date == day.Date && x.EmployeesId == emp.Id).FirstOrDefault();

                        var status = await _mediator.Send(new AttendLeaving_GetStatusRequest { day = day, employees = emp, transactions = currentTransaction });


                        if (status.Id == (int)AttendLeavingStatusEnum.late)
                        {

                            empAttendLateTime +=
                                (currentTransaction.shift1_LateTime.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift2_LateTime.GetValueOrDefault(zeroTimeSpan))
                                   + (currentTransaction.shift3_LateTime.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift4_LateTime.GetValueOrDefault(zeroTimeSpan));

                            empLeaveEarlyTime += (currentTransaction.shift1_LeaveEarly.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift2_LeaveEarly.GetValueOrDefault(zeroTimeSpan))
                                 + (currentTransaction.shift3_LeaveEarly.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift4_LeaveEarly.GetValueOrDefault(zeroTimeSpan));


                            ListOf_days.Add(new AttendLateLeaveEarlyResponseDTO_Days
                            {
                                branchId = branch.Id,
                                employeeId = emp.Id,
                                sh1_attendance = currentTransaction.shift1_TimeIn?.ToString("HH:mm") ??defultData.EmptyAttendance,
                                sh2_attendance = currentTransaction.shift2_TimeIn?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                sh3_attendance = currentTransaction.shift3_TimeIn?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                sh4_attendance = currentTransaction.shift4_TimeIn?.ToString("HH:mm") ?? defultData.EmptyAttendance,

                                sh1_leaving = currentTransaction.shift1_TimeOut?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                sh2_leaving = currentTransaction.shift2_TimeOut?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                sh3_leaving = currentTransaction.shift3_TimeOut?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                sh4_leaving = currentTransaction.shift4_TimeOut?.ToString("HH:mm") ?? defultData.EmptyAttendance,

                                totalAttendlateInThisDay = Attendance_Totals.convertTimeSpanToString((currentTransaction.shift1_LateTime.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift2_LateTime.GetValueOrDefault(zeroTimeSpan))
                                   + (currentTransaction.shift3_LateTime.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift4_LateTime.GetValueOrDefault(zeroTimeSpan))),

                                totalLeavingEarlyInThisDay = Attendance_Totals.convertTimeSpanToString((currentTransaction.shift1_LeaveEarly.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift2_LeaveEarly.GetValueOrDefault(zeroTimeSpan))
                                  + (currentTransaction.shift3_LeaveEarly.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift4_LeaveEarly.GetValueOrDefault(zeroTimeSpan))),

                                date = day.ToString("yyyy-MM-dd") ?? "",
                                dayAr = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().arabicName,
                                dayEn = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().latinName,
                                //latetime = Attendance_Totals.convertTimeSpanToString(empLateTime)


                            });



                        }

                    }
                    thisBranch.employees.Add(new AttendLateLeaveEarlyResponseDTO_Employees
                    {

                        totalAttendlate = Attendance_Totals.convertTimeSpanToString(empAttendLateTime),
                        totalLeavingEarly = Attendance_Totals.convertTimeSpanToString(empLeaveEarlyTime),

                        code = emp.Code,
                        arabicName = emp.ArabicName,
                        latinName = emp.LatinName,
                        jobAr = emp.Job != null ? emp.Job.ArabicName : defultData.EmptyAttendance,
                        jobEn = emp.Job != null ? emp.Job.LatinName : defultData.EmptyAttendance,

                        Id = emp.Id,
                        branchId = emp.gLBranchId,
                        shiftAr = emp.shiftsMaster?.arabicName ?? defultData.EmptyAttendance,
                        shiftEn = emp.shiftsMaster?.latinName ?? defultData.EmptyAttendance,
                        days = ListOf_days
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



    public class AttendLateLeaveEarlyResponseDTO_Days
    {
        public int? branchId { get; set; }
        public int? employeeId { get; set; }
        public string? dayAr { get; set; }
        public string? dayEn { get; set; }
        public string? date { get; set; }


        public string? sh1_attendance { get; set; }
        public string? sh1_leaving { get; set; }

        public string? sh2_attendance { get; set; }
        public string? sh2_leaving { get; set; }

        public string? sh3_attendance { get; set; }
        public string? sh3_leaving { get; set; }

        public string? sh4_attendance { get; set; }
        public string? sh4_leaving { get; set; }

        public string? totalAttendlateInThisDay { get; set; }
        public string? totalLeavingEarlyInThisDay { get; set; }
    }
    public class AttendLateLeaveEarlyResponseDTO_Employees
    {
        public int? Id { get; set; }
        public int? branchId { get; set; }
        public int? code { get; set; }
        public string? arabicName { get; set; }
        public string? latinName { get; set; }

        public string? shiftAr { get; set; }
        public string? shiftEn { get; set; }

        public string? jobAr { get; set; }
        public string? jobEn { get; set; }
        public string? totalAttendlate { get; set; }
        public string? totalLeavingEarly { get; set; }

        public List<AttendLateLeaveEarlyResponseDTO_Days> days { get; set; }
    }
    public class AttendLateLeaveEarlyResponseDTO_Branches
    {
        public int? Id { get; set; }
        public string? arabicName { get; set; }
        public string? latinName { get; set; }
        public List<AttendLateLeaveEarlyResponseDTO_Employees> employees { get; set; }

    }


}
