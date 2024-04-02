using MediatR;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.VaccationEmployees.GetVaccationEmployees
{
    public class GetVaccationEmployeesHandler : IRequestHandler<GetVaccationEmployeesRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery;

        public GetVaccationEmployeesHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesQuery)
        {
            _VaccationEmployeesQuery = vaccationEmployeesQuery;
        }

        public async Task<ResponseResult> Handle(GetVaccationEmployeesRequest request, CancellationToken cancellationToken)
        {

            var data = _VaccationEmployeesQuery
                .TableNoTracking
                .Include(c => c.Employees)
                .ThenInclude(c=>c.GLBranch)
                .Include(c=>c.Vaccations)
                .ToList();
            var totalData = data.Count();
            var res = data;
            var dataCount = res.Count();
            var response = res
                .Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
                .Take(request.PageSize ?? 0)
                .ToList()
                .Select(c => new VaccationEmployeesDTO
                {
                    Id = c.Id,
                    code = c.Id,
                    employee = c.Employees != null ? new VaccationEmployees_Comman
                    {
                        Id = c.EmployeeId,
                        arabicName = c.Employees.ArabicName,
                        latinName = c.Employees.LatinName
                    } : null,
                    vacation = c.Vaccations != null ? new VaccationEmployees_Comman
                    {
                        Id = c.VaccationId,
                        arabicName = c.Vaccations.ArabicName,
                        latinName = c.Vaccations.LatinName
                    }:null,
                    startdate = c.DateFrom,
                    enddate = c.DateTo,
                    Note=c.Note,
                    branch = c.Employees.GLBranch != null ? new VaccationEmployees_Comman
                    {
                        Id = c.Employees.GLBranch.Id,
                        arabicName = c.Employees.GLBranch.ArabicName,
                        latinName = c.Employees.GLBranch.LatinName
                    } : null,
                    duration = c.DateTo.Subtract(c.DateFrom).TotalDays,

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

    public class VaccationEmployeesDTO
    {
        public int Id { get; set; }
        public int code { get; set; }

        public VaccationEmployees_Comman branch { get; set; }
        public VaccationEmployees_Comman employee { get; set; }
        public VaccationEmployees_Comman vacation { get; set; }
        public double duration { get; set; }

        public string Note { get; set; }

        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }

    }
    public class VaccationEmployees_Comman
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
}
