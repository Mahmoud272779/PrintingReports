using System.Threading;
using App.Application.Handlers.AttendLeaving.AttendLeaving_Helper;
using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation;
using App.Application.Handlers.AttendLeaving.Reports.ReportHelper;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Infrastructure;
using App.Infrastructure.settings;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.DetailedDailyLate
{
    public class DetailedDailyLateHandler : IRequestHandler<DetailedDailyLateRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<InvEmployees> _employeesQuery;
        private readonly IRepositoryQuery<RamadanDate> _RamadanDateQuery;

        public DetailedDailyLateHandler(IRepositoryQuery<InvEmployees> employeesQuery, IRepositoryQuery<GLBranch> gLBranchQuery, IRepositoryQuery<MoviedTransactions> moviedTransactionsQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployees, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesQuery, IMediator mediator, IRepositoryQuery<RamadanDate> ramadanDateQuery)
        {
            _employeesQuery = employeesQuery;
            _GLBranchQuery = gLBranchQuery;
            _MoviedTransactionsQuery = moviedTransactionsQuery;
            _HolidaysEmployees = holidaysEmployees;
            _VaccationEmployeesQuery = vaccationEmployeesQuery;
            _mediator = mediator;
            _RamadanDateQuery = ramadanDateQuery;
        }

        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;

        private readonly IRepositoryQuery<MoviedTransactions> _MoviedTransactionsQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployees;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery;
        private readonly IMediator _mediator;
        public async Task<ResponseResult> Handle(DetailedDailyLateRequest request, CancellationToken cancellationToken)
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
                   ?.Where(c => c.day >= request.DateFrom && c.day <= request.DateTo)
                   ?.Where(c => employees.Select(x => x.Id).ToArray().Contains(c.EmployeesId)).ToList();

            List<DetailedlateResponseDTO_Branches> ListOf_branches = new List<DetailedlateResponseDTO_Branches>();

            foreach (var branch in branchesObjs)
            {
                DetailedlateResponseDTO_Branches thisBranch = new DetailedlateResponseDTO_Branches
                {
                    Id = branch.Id,
                    arabicName = branch.ArabicName,
                    latinName = branch.LatinName
                };
                thisBranch.employees = new List<DetailedlateResponseDTO_Employees>();
                var emps = employees.Where(c => c.gLBranchId == branch.Id);
                var zeroTimeSpan = new TimeSpan();
                foreach (var emp in emps)
                {
                    List<DetailedlateResponseDTO_Days> ListOf_days = new List<DetailedlateResponseDTO_Days>();

                    foreach (var day in days)
                    {
                       var currentTransaction = movedTransactions.Where(x => x.day.Date == day.Date && x.EmployeesId == emp.Id).FirstOrDefault();

                        var status = await _mediator.Send(new AttendLeaving_GetStatusRequest { day = day, employees = emp, transactions = currentTransaction });


                        if (status.Id == (int)AttendLeavingStatusEnum.late)
                        {

                            var empLateTime = 
                                (currentTransaction.shift1_LateTime.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift2_LateTime.GetValueOrDefault(zeroTimeSpan))
                                   + (currentTransaction.shift3_LateTime.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift4_LateTime.GetValueOrDefault(zeroTimeSpan))
                                  + (currentTransaction.shift1_LeaveEarly.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift2_LeaveEarly.GetValueOrDefault(zeroTimeSpan))
                                  + (currentTransaction.shift3_LeaveEarly.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift4_LeaveEarly.GetValueOrDefault(zeroTimeSpan));

                            ListOf_days.Add(new DetailedlateResponseDTO_Days
                            {
                                date = day.ToString(defultData.datetimeFormat),
                                dayAr = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().arabicName,
                                dayEn = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().latinName,
                                latetime = Attendance_Totals.convertTimeSpanToString(empLateTime),
                                branchId = branch.Id,
                                employeeId = emp.Id

                            });



                        }
                        
                    }
                    if(ListOf_days.Any())
                        thisBranch.employees.Add(new DetailedlateResponseDTO_Employees
                        {
                            Id = emp.Id,
                            branchId = branch.Id,
                            code = emp.Code,
                            arabicName = emp.ArabicName,
                            latinName = emp.LatinName,
                            jobAr = emp.Job?.ArabicName ?? "",
                            jobEn = emp.Job?.LatinName ?? "",
                            shiftAr = emp.shiftsMaster?.arabicName ?? "",
                            shiftEn = emp.shiftsMaster?.latinName ?? "",
                            days = ListOf_days,
                            totalLate = DateTimeHelper.SumTimeSpans(ListOf_days.Select(c=> c.latetime).ToList())
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



    public class DetailedlateResponseDTO_Days
    {
        public int employeeId { get; set; }
        public int branchId { get; set; }
        public string? dayAr { get; set; }
        public string? dayEn { get; set; }
        public string? date { get; set; }
        public string? latetime { get; set; }


    }
    public class DetailedlateResponseDTO_Employees
    {
        public int Id { get; set; }
        public int branchId { get; set; }
        public int? code { get; set; }
        public string? arabicName { get; set; }
        public string? latinName { get; set; }
        public string? jobAr { get; set; }
        public string? jobEn { get; set; }
        public string? shiftAr { get; set; }
        public string? shiftEn { get; set; }
        public string totalLate { get; set; }

        public List<DetailedlateResponseDTO_Days> days { get; set; }
    }
    public class DetailedlateResponseDTO_Branches
    {
        public int Id { get; set; }
        public string? arabicName { get; set; }
        public string? latinName { get; set; }
        public List<DetailedlateResponseDTO_Employees> employees { get; set; }

    }




}
