using App.Domain.Models.Response.HR.AttendLeaving;
using MediatR;
using System.Linq;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.HolidaysEmployees.GetEmployeesForAddModel
{
    public class GetEmployeesForAddModelHandler : IRequestHandler<GetEmployeesForAddModelRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Holidays> _HolidaysQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployeesQuery;

        public GetEmployeesForAddModelHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Holidays> holidaysQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployeesQuery)
        {
            _InvEmployeesQuery = invEmployeesQuery;
            _HolidaysQuery = holidaysQuery;
            _HolidaysEmployeesQuery = holidaysEmployeesQuery;
        }

        public async Task<ResponseResult> Handle(GetEmployeesForAddModelRequest request, CancellationToken cancellationToken)
        {
            int[] branches = null;

            if (!string.IsNullOrEmpty(request.branchId) && request.branchId != "0")
                branches = request.branchId.Split(',').Select(c => int.Parse(c)).ToArray();
            var choosedEmp = _HolidaysEmployeesQuery.TableNoTracking.Where(c => c.HolidaysId == request.holidayId).ToList();
            var res = _InvEmployeesQuery
                .TableNoTracking
                .Include(c => c.Sections)
                .Include(c => c.Departments)
                .Where(c => !choosedEmp.Select(x => x.EmployeesId).Contains(c.Id));
            var totalData = res.Count();
            
            var emps = res
                .Where(c => request.empId != null && request.empId != 0 ? c.Id == request.empId : true)
                .Where(c => request.code != null && request.code != 0 ? c.Code.ToString().Contains(request.code.ToString()) : true)
                .Where(c => branches != null ? branches.Contains(c.gLBranchId) : true)
                .Where(c => request.SectionsId != null && request.SectionsId != 0 ? c.SectionsId == request.SectionsId : true)
                .Where(c => request.SectionsId != null && request.SectionsId != 0 ? c.DepartmentsId == request.DepartmentId : true)
                .Where(c => request.ShiftId != null && request.ShiftId != 0 ? c.shiftsMasterId == request.ShiftId : true)
                .Where(c => request.JobId != null && request.JobId != 0 ? c.JobId == request.JobId : true);


            var dataCount = emps.Count();

            var response = emps
                .Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
                .Take(request.PageSize ?? 0)
                .ToList()
                .Select(c => new GetShiftMasterDropDownlist_ResponseDTO
                {
                    code=c.Code,
                    Id = c.Id,
                    arabicName = c.ArabicName,
                    latinName = c.LatinName,

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
}
