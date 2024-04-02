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
    public class GetMissionsHandler : IRequestHandler<GetMissionsRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Missions> _MissionsQuery;

        public GetMissionsHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Missions> missionsQuery)
        {
            _MissionsQuery = missionsQuery;
        }

        public async Task<ResponseResult> Handle(GetMissionsRequest request, CancellationToken cancellationToken)
        {
            var data = _MissionsQuery.TableNoTracking.Include(c=> c.employees);
            var totalData = data.Count();
            var res = data.Where(c => !string.IsNullOrEmpty(request.SearchCriteria) ? c.arabicName.Contains(request.SearchCriteria) || c.latinName.Contains(request.SearchCriteria) : true);
            var dataCount = res.Count();
            var response = res
                .Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
                .Take(request.PageSize ?? 0)
                .ToList()
                .Select(c=> new MissionsResponseDTO
                {
                    Id = c.Id,
                    arabicName = c.arabicName,
                    latinName = c.latinName,
                    canDelete = !c.employees.Any()
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
    public class MissionsResponseDTO
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public bool canDelete { get; set; }
    }
}
