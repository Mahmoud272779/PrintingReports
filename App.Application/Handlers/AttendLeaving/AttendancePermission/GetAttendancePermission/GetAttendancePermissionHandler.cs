using App.Application.Handlers.AttendLeaving.HolidaysEmployees.GetHolidaysEmployees;
using App.Domain.Entities.Process.AttendLeaving;
using MediatR;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.AttendancePermission.GetAttendancePermission
{
    public class GetAttendancePermissionHandler : IRequestHandler<GetAttendancePermissionRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.AttendancPermission> _AttendancPermissionQuery;
        private readonly IRepositoryQuery<InvEmployees> _invEmpsQuery;
        public GetAttendancePermissionHandler(IRepositoryQuery<AttendancPermission> attendancPermissionQuery, IRepositoryQuery<InvEmployees> invEmpsQuery)
        {
            _AttendancPermissionQuery = attendancPermissionQuery;
            _invEmpsQuery = invEmpsQuery;
        }

        public async Task<ResponseResult> Handle(GetAttendancePermissionRequest request, CancellationToken cancellationToken)
        {
            var emps = _invEmpsQuery.TableNoTracking.ToList();
            var _allPermessions = _AttendancPermissionQuery.TableNoTracking;
            var totalCount = _allPermessions.Count();
            var allPermessions = _allPermessions
                .Include(c => c.employees)
                 .ThenInclude(c=>c.GLBranch)
                 .Include(c=>c.employees.shiftsMaster)
                 

                .Where(c => !string.IsNullOrEmpty(request.SearchCriteria) ? request.SearchCriteria.Contains(c.employees.ArabicName) || request.SearchCriteria.Contains(c.employees.LatinName)
                || request.SearchCriteria.Contains(c.employees.Code.ToString()) : true).ToList();
                var dataCount =allPermessions.Count();
                

            var data = allPermessions.Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
                .Take(request.PageSize ?? 0).Select(c => new GetAttendancePermissionDTO
            {
                id = c.Id,
                code=c.Id,
                Employee = new Employeedto
                {
                    id=c.employees.Id,
                    arabicName = c.employees.ArabicName,
                    latinName = c.employees.LatinName,
                },
                Branch = new Branchdto
                {
                    arabicName = c.employees.GLBranch.ArabicName,
                    latinName = c.employees.GLBranch.LatinName,
                },
                Shift = new Shiftdto
                {
                    arabicName = c.employees.shiftsMaster?.arabicName ?? "",
                    latinName = c.employees.shiftsMaster?.latinName ?? "",
                },
                permissiontype=new permissiontype
                {
                    type=c.type,
                    arabicName=c.type==1? "يومى" :"مؤقت",
                    latinName= c.type == 1 ? "Day" : "Temp",
                },
                docDate=c.Day,
                note=c.note,


            });

            return new ResponseResult
            {
                Result = Result.Success,
                Data = data,
                Note = Aliases.GetEndOfData(request.PageSize ?? 0, data.Count(), request.PageNumber ?? 0),
                DataCount = dataCount,
                TotalCount = totalCount
            };
        }

        public class GetAttendancePermissionDTO
        {
            public int id { get; set; }
            public Employeedto Employee { get; set; }
            public Branchdto Branch { get; set; }
            public Shiftdto Shift { get; set; }

            public permissiontype permissiontype { get; set; }
            public DateTime docDate { get; set; }
            public string? note { get; set; }
            public int? code { get; set; }

        }
    }

    public class permissiontype
    {
        public int? type { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }

    }
}
