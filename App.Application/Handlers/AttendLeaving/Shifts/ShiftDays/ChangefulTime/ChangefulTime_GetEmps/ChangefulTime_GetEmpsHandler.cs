using App.Domain.Entities.Process.AttendLeaving.Shift;
using App.Domain.Models.Response.HR.AttendLeaving;
using MediatR;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.ChangefulTime_GetEmps
{
    public class ChangefulTime_GetEmpsHandler : IRequestHandler<ChangefulTime_GetEmpsRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<ChangefulTimeGroupsEmployees> _ChangefulTimeGroupsEmployeesQuery;
        public async Task<ResponseResult> Handle(ChangefulTime_GetEmpsRequest request, CancellationToken cancellationToken)
        {
            var emps = _ChangefulTimeGroupsEmployeesQuery.TableNoTracking.Include(c=> c.invEmployees)
                .Where(c => c.changefulTimeGroupsMasterId == request.GroupId);

            var totalData = emps.Count();
            var res = emps.Where(c => !string.IsNullOrEmpty(request.SearchCriteria) ? request.SearchCriteria.Contains(c.invEmployees.ArabicName) || request.SearchCriteria.Contains(c.invEmployees.LatinName) : true);
            var dataCount = res.Count();
            var response = res.Skip((request.PageSize ?? 0 - 1) * request.PageNumber ?? 0).Take(request.PageNumber ?? 0)
                .Select(c=> new ChangefulTime_EmployeesResponeDTO
                {
                    arabicName = c.invEmployees.ArabicName,
                    latinName = c.invEmployees.LatinName,
                    EmployeeId = c.invEmployeesId,
                    Id = c.Id
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
