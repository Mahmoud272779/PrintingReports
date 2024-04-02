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
    public class GetProjectsHandler : IRequestHandler<GetProjectsRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Projects> _ProjectsQuery;

        public GetProjectsHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Projects> ProjectsQuery)
        {
            _ProjectsQuery = ProjectsQuery;
        }

        public async Task<ResponseResult> Handle(GetProjectsRequest request, CancellationToken cancellationToken)
        {
            var data = _ProjectsQuery.TableNoTracking.Include(c=> c.InvEmployees);
            var totalData = data.Count();
            var res = data.Where(c => !string.IsNullOrEmpty(request.searchCriteria) ? c.arabicName.Contains(request.searchCriteria) || c.latinName.Contains(request.searchCriteria) : true);
            var dataCount = res.Count();
            var response = res
                .Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
                .Take(request.PageSize ?? 0)
                .ToList()
                .Select(c=> new ProjectsResponseDTO
                {
                    Id = c.Id,
                    arabicName = c.arabicName,
                    latinName = c.latinName,
                    canDelete = !c.InvEmployees.Any()
                });
            return new ResponseResult
            {
                Result = Result.Success,
                Data = response.OrderByDescending(c=>c.Id),
                Note = Aliases.GetEndOfData(request.PageSize ?? 0, dataCount, request.PageNumber ?? 0),
                DataCount = dataCount,
                TotalCount = totalData
            };
        }
    }
    public class ProjectsResponseDTO
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public bool canDelete { get; set; }
    }
}
