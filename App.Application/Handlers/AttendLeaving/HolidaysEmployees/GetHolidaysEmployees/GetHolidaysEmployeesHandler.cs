using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.HolidaysEmployees.GetHolidaysEmployees
{
    public class GetHolidaysEmployeesHandler : IRequestHandler<GetHolidaysEmployeesRequest, ResponseResult>
    {

        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployeesQuery;

        public GetHolidaysEmployeesHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployeesQuery)
        {
            _HolidaysEmployeesQuery = holidaysEmployeesQuery;
        }

        public async Task<ResponseResult> Handle(GetHolidaysEmployeesRequest request, CancellationToken cancellationToken)
        {
            int[] branches = null;
            if (!string.IsNullOrEmpty(request.branchesIds))
                branches = request.branchesIds.Split(',').Select(c => int.Parse(c)).ToArray();
            int[] jobs = null;
            if (!string.IsNullOrEmpty(request.jobsIds))
                jobs = request.jobsIds.Split(',').Select(c => int.Parse(c)).ToArray();
            var data = _HolidaysEmployeesQuery
                .TableNoTracking
                .Include(c => c.Employees)
                .ThenInclude(c=>c.GLBranch)
            .Include(c=> c.Employees.shiftsMaster)
            .Where(c => !string.IsNullOrEmpty(request.SearchCriteria) ? c.Employees.ArabicName.Contains(request.SearchCriteria) || c.Employees.LatinName.Contains(request.SearchCriteria) || c.Employees.Code.ToString().Contains(request.SearchCriteria) : true)
            .Where(c => (branches != null && branches.Any()) ? branches.Contains(c.Employees.gLBranchId) : true)
            .Where(c => (jobs != null && jobs.Any()) ? jobs.Contains(c.Employees.JobId.Value) : true)
            .Where(c=> c.HolidaysId == request.holidayId)
            .ToList();
            var totalData = data.Count();
            var res = data;
            var dataCount = res.Count();
            var response = res
            .Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
                .Take(request.PageSize ?? 0)
                .ToList()
                .Select(c => new GetEmployeeHolidayDTO
                {
                    code=c.Employees.Code,
                    id = c.Id,
                    Employee = new Employeedto
                    {
                        arabicName = c.Employees.ArabicName,
                        latinName = c.Employees.LatinName,
                    },
                    Branch = new Branchdto
                    {
                        arabicName = c.Employees.GLBranch.ArabicName,
                        latinName = c.Employees.GLBranch.LatinName,
                    },
                    Shift = new Shiftdto
                    {
                        arabicName = c.Employees.shiftsMaster?.arabicName ?? "",
                        latinName = c.Employees.shiftsMaster?.latinName ?? "",
                    },
                    canDelete = true
                });
            return new ResponseResult
            {
                Result = Result.Success,
                Data = response,
                Note = Aliases.GetEndOfData(request.PageSize ?? 0, dataCount, request.PageNumber ?? 0),
                DataCount = dataCount,
                TotalCount = totalData
            };
        }

    }
    public class GetEmployeeHolidayDTO
    {
        public int id { get; set; }
        public Employeedto Employee { get; set; }
        public Branchdto Branch { get; set; }
        public Shiftdto Shift { get; set; }
        public bool canDelete { get; set; }

        public int code { get; set; }
    }
    public class Employeedto
    {
        public int id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
    public class Branchdto
    {
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
    public class Shiftdto
    {
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
}



