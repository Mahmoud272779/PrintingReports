using App.Application.Handlers.AttendLeaving.AttendLeaving_Helper;
using App.Application.Handlers.AttendLeaving.Reports.ReportHelper;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Infrastructure;
using App.Infrastructure.settings;
using MediatR;
using System.Text.Json.Serialization;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.Reports.GetAbsanceReport
{
    public class GetAbsanceReportHandler : IRequestHandler<GetAbsanceReportRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.InvEmployees> _employeesQuery;
        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;

        private readonly IRepositoryQuery<MoviedTransactions> _MoviedTransactionsQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployees;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery;
        private readonly IRepositoryQuery<RamadanDate> _RamadanDateQuery;
        private readonly IMediator _mediator;

        public GetAbsanceReportHandler(IRepositoryQuery<InvEmployees> employeesQuery, IRepositoryQuery<GLBranch> gLBranchQuery, IRepositoryQuery<MoviedTransactions> moviedTransactionsQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployees, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesQuery, IRepositoryQuery<RamadanDate> ramadanDateQuery, IMediator mediator)
        {
            _employeesQuery = employeesQuery;
            _GLBranchQuery = gLBranchQuery;
            _MoviedTransactionsQuery = moviedTransactionsQuery;
            _HolidaysEmployees = holidaysEmployees;
            _VaccationEmployeesQuery = vaccationEmployeesQuery;
            _RamadanDateQuery = ramadanDateQuery;
            _mediator = mediator;
        }




        public async Task<ResponseResult> Handle(GetAbsanceReportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.dateFrom > request.dateTo)
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


                var employees = _employeesQuery.TableNoTracking
                    .Include(c => c.GLBranch)
                    .Include(c => c.Job)
                    .Include(c => c.projects)
                    .Include(c => c.shiftsMaster)
                    .ThenInclude(c => c.changefulTimeGroups)
                    .ThenInclude(c => c.changefulTimeDays)
                    .Include(c=> c.shiftsMaster.normalShiftDetalies)
                    .Include(c => c.Sections)
                    .Include(c => c.Departments)
                    .Where(c => c.Status != (int)Status.newElement)
                    .Where(c => request.EmpId != null ? c.Id == request.EmpId : true)
                    .Where(c => request.DepartmentId != null ? c.DepartmentsId == request.DepartmentId : true)
                    .Where(c => request.SectionId != null ? c.SectionsId == request.SectionId : true)
                    .Where(c => (branches != null && branches.Any()) ? branches.Contains(c.gLBranchId) : true)
                    .Where(c => request.JobId != null ? c.JobId == request.JobId : true)
                    .Where(c => request.GroupId != null ? c.employeesGroupId == request.GroupId : true)
                    .Where(c => request.ShiftmasterId != null ? c.shiftsMasterId == request.ShiftmasterId : true)
                    .ToList();


                var allbranches = _GLBranchQuery
                    .TableNoTracking
                    .Where(c => request.BranchId != null ? branches.Contains(c.Id) : true);

                var days = DatesService.GetDatesBetween(request.dateFrom, request.dateTo);

                var movedTransactions = _MoviedTransactionsQuery
                    .TableNoTracking
                    ?.Where(c => c.day >= request.dateFrom && c.day <= request.dateTo)
                    ?.Where(c => employees.Select(x => x.Id).ToArray().Contains(c.EmployeesId));


                List<DayDTO> ListOf_days = new List<DayDTO>();
                int index = 0;
                foreach (var day in days)
                {
                    index++;
                    List<BranchDTO> ListOf_branches = new List<BranchDTO>();

                    DayDTO thisday = (new DayDTO
                    {
                        DayIndex = index,
                        date = day.ToString(defultData.datetimeFormat),
                        dayAr = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().arabicName,
                        dayEn = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().latinName,
                    });

                    thisday.Branches = new List<BranchDTO>();

                    int branchIndex = 0;
                    foreach (var branch in allbranches)
                    {
                        branchIndex++;
                        BranchDTO thisBranch = new BranchDTO
                        {
                            DayIndex = index,
                            BranchIndex = branchIndex,
                            BranchNameAr = branch.ArabicName,
                            BranchNameEn = branch.LatinName,
                            branchId = branch.Id,
                        };
                        thisday.Branches.Add(thisBranch);
                        thisBranch.Employees = new List<EmployeeDTO>();

                        var emps = employees.Where(c => c.gLBranchId == branch.Id);
                        foreach (var emp in emps)
                        {



                            var currentTransaction = movedTransactions.Where(x => x.day.Date == day.Date && x.EmployeesId == emp.Id).FirstOrDefault();
                        
                            var status = await _mediator.Send(new AttendLeaving_GetStatusRequest { day = day, employees = emp, transactions = currentTransaction });

                            if (status.Id == (int)AttendLeavingStatusEnum.Absence)
                            {

                                thisBranch.Employees.Add(new EmployeeDTO
                                {
                                    DayIndex = index,
                                    BranchIndex = branchIndex,
                                    empcode = emp.Code,
                                    empId = emp.Id,
                                    empnameAr = emp.ArabicName,
                                    empnameEn = emp.LatinName,
                                    shiftmasterNameAr = emp.shiftsMaster?.arabicName,
                                    shiftmasterNameEn = emp.shiftsMaster?.latinName,
                                    ProjectNameAr = emp.projects?.arabicName ?? "",
                                    ProjectNameEn = emp.projects?.latinName ?? "",
                                    SectionNameAr = emp.Sections?.arabicName ?? "",
                                    SectionNameEn = emp.Sections?.latinName ?? "",
                                    DepNameAr = emp.Departments?.arabicName ?? "",
                                    DepNameEn = emp.Departments?.latinName ?? "",
                                    jobAr = emp.Job?.ArabicName??"",
                                    jobEn = emp.Job?.LatinName??"",
                                });
                            }


                        }
                        if (thisBranch.Employees.Any())
                            ListOf_branches.Add(thisBranch);
                    }
                    if (ListOf_branches.Any() && ListOf_branches.Any(c=> c.Employees.Any()))
                        ListOf_days.Add(new DayDTO
                        {
                            DayIndex = index,
                            date = day.ToString(defultData.datetimeFormat),
                            Branches = ListOf_branches,
                            dayAr = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().arabicName,
                            dayEn = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().latinName,
                        });
                }


                return new ResponseResult
                {
                    Result = Result.Success,
                    Data = ListOf_days
                };
            }

            catch (Exception ex)
            {
                return new ResponseResult { Result = Result.Failed, ErrorMessageEn = "Error" };
            }

        }
    }

    public class EmployeeDTO
    {
        [JsonIgnore]
        public int DayIndex { get; set; }
        [JsonIgnore]
        public int BranchIndex { get; set; }
        public int? empcode { get; set; }
        public int? empId { get; set; }
        public string? empnameAr { get; set; }
        public string? empnameEn { get; set; }

        public string? SectionNameAr { get; set; }
        public string? SectionNameEn { get; set; }

        public string? DepNameAr { get; set; }
        public string? DepNameEn { get; set; }

        public string? ProjectNameAr { get; set; }
        public string? ProjectNameEn { get; set; }
        public string? jobAr { get; set; }
        public string? jobEn { get; set; }



        public string? shiftmasterNameAr { get; set; }
        public string? shiftmasterNameEn { get; set; }

    }

    public class BranchDTO
    {

        [JsonIgnore]
        public int DayIndex { get; set; }
        [JsonIgnore]
        public int BranchIndex { get; set; }
        public string? BranchNameAr { get; set; }
        public string? BranchNameEn { get; set; }
        public int branchId { get; set; }
        public List<EmployeeDTO> Employees { get; set; }
    }
    public class DayDTO
    {
        [JsonIgnore]
        public int DayIndex { get; set; }
        public string dayAr { get; set; }
        public string dayEn { get; set; }
        public string date { get; set; }
        public List<BranchDTO> Branches { get; set; }
    }


}
