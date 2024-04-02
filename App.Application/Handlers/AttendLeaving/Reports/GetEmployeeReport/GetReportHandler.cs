using MediatR;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.Reports.GetReport
{
    public class GetReportHandler : IRequestHandler<GetReportRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.InvEmployees> _InvEmployeesQuery;

        public GetReportHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery)
        {
            _InvEmployeesQuery = invEmployeesQuery;
        }

        public async Task<ResponseResult> Handle(GetReportRequest request, CancellationToken cancellationToken)
        {
            int[] branches = null;
            if (!string.IsNullOrEmpty(request.BranchId))
                branches = request.BranchId.Split(',').Select(c => int.Parse(c)).ToArray();
            var data = _InvEmployeesQuery.TableNoTracking
                .Include(c => c.GLBranch)
                .Include(c => c.Job)
                .Include(c => c.employeesGroup)
                .Include(c => c.shiftsMaster)
                .Include(c => c.Sections)
                .Include(c => c.Departments)
                .Where(c => c.Status != (int)Status.newElement)
                .Where(c => request.EmpId != null ? c.Id == request.EmpId : true)
                .Where(c => c.DepartmentsId != null ? c.DepartmentsId == request.DepartmentId : true)
                .Where(c => c.SectionsId != null ? c.SectionsId == request.SectionId : true)
                .Where(c => (branches != null && branches.Any()) ? branches.Contains(c.gLBranchId) : true)
                .Where(c => request.JobId != null ? c.JobId == request.JobId : true)
                .Where(c => request.GroupId != null ? c.JobId == request.GroupId : true)
                .Where(c => request.ShiftmasterId != null ? c.shiftsMasterId == request.ShiftmasterId : true)
                .ToList();


            var totalData = data.Count();
            var res = data;
            var dataCount = res.Count();

            List<EmpDTO> empsList = new List<EmpDTO>();

            foreach (var c in data)
            {
                empsList.Add(new EmpDTO {

                    EmpId = c.Id,
                    code = c.Code,
                    EmpNameAr = c?.ArabicName,
                    EmpNameEn = c?.LatinName,
                    BranchNameAr = c.GLBranch?.ArabicName,
                    BranchNameEn = c.GLBranch?.LatinName,
                    SectionNameAr = c.Sections?.arabicName ?? "",
                    SectionNameEn = c.Sections?.latinName ?? "",
                    DepNameAr = c.Departments?.arabicName ?? "",
                    DepNameEn = c.Departments?.latinName ?? "",
                    JobNameAr = c.Job?.ArabicName,
                    JobNameEn = c.Job?.LatinName,
                    GroupNameAr = c.employeesGroup?.arabicName,
                    GroupNameEn = c.employeesGroup?.latinName,
                    shiftmasterNameAr = c.shiftsMaster?.arabicName,
                    shiftmasterNameEn = c.shiftsMaster?.latinName

                });
            }

            
            return new ResponseResult
            {
                Result = Result.Success,
                Data = empsList,

                DataCount = dataCount,
                TotalCount = totalData
            };
        }

    }

    public class EmpDTO
    {
        public int? EmpId { get; set; }
        public int? code { get; set; }
        public string? EmpNameAr { get; set; }
        public string? EmpNameEn { get; set; }
        public string? BranchNameAr { get; set; }
        public string? BranchNameEn { get; set; }

        public string? SectionNameAr { get; set; }
        public string? SectionNameEn { get; set; }

        public string? DepNameAr { get; set; }
        public string? DepNameEn { get; set; }

        public string? JobNameAr { get; set; }
        public string? JobNameEn { get; set; }

        public string? GroupNameAr { get; set; }
        public string? GroupNameEn { get; set; }

        public string? shiftmasterNameAr { get; set; }
        public string? shiftmasterNameEn { get; set; }
    }
}
