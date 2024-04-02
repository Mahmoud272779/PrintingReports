using App.Domain.Entities.Process.AttendLeaving;
using FastReport.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Missions.GetMissions
{
    public class GetEmployeeGroupsHandler : IRequestHandler<GetEmployeeGroupsRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.EmployeesGroup> _EmployeeGroupsQuery;

        public GetEmployeeGroupsHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.EmployeesGroup> EmployeeGroupsQuery)
        {
            _EmployeeGroupsQuery = EmployeeGroupsQuery;
        }

        public async Task<ResponseResult> Handle(GetEmployeeGroupsRequest request, CancellationToken cancellationToken)
        {
            var data = _EmployeeGroupsQuery.TableNoTracking.Include(c=> c.InvEmployees);
            var totalData = data.Count();
            var res = data.Where(c => !string.IsNullOrEmpty(request.SearchCriteria) ? c.arabicName.Contains(request.SearchCriteria) || c.latinName.Contains(request.SearchCriteria) : true);
            var dataCount = res.Count();
            var response = res
                .Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
            .Take(request.PageSize ?? 0)
                .Select(c=> new EmployeeGroupsResponseDTO
                {
                    Id = c.Id,
                    arabicName = c.arabicName,
                    latinName = c.latinName,
                    canDelete = !c.InvEmployees.Any()
                });
            return new ResponseResult
            {
                Result = Result.Success,
                Data = response.OrderByDescending(c => c.Id),
                Note = Aliases.GetEndOfData(request.PageSize ?? 0, dataCount, request.PageNumber ?? 0),
                DataCount = dataCount,
                TotalCount = totalData
            };
        }
    }
    public class EmployeeGroupsResponseDTO
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public bool canDelete { get; set; }
    }
}
