using App.Application.Handlers.AttendLeaving.Missions.GetMissions;
using App.Domain.Models.Response.HR.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Vaccation.GetVaccation
{
    public class GetVaccationHandler : IRequestHandler<GetVaccationRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Vaccation> _VaccationQuery;

        public GetVaccationHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Vaccation> missionsQuery)
        {
            _VaccationQuery = missionsQuery;
        }

        public async Task<ResponseResult> Handle(GetVaccationRequest request, CancellationToken cancellationToken)
        {
            var data = _VaccationQuery.TableNoTracking.Include(c => c.vaccationEmployees);
            var totalData = data.Count();
            var res = data.Where(c => !string.IsNullOrEmpty(request.SearchCriteria) ? c.ArabicName.Contains(request.SearchCriteria) || c.LatinName.Contains(request.SearchCriteria) : true);
            var dataCount = res.Count();
            var response = res
                .Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
                .Take(request.PageSize ?? 0)
                .ToList()
                .Select(c => new VaccationResponseDTO
                {
                    Id = c.Id,
                    ArabicName = c.ArabicName,
                    LatinName = c.LatinName,
                    candelete = !c.vaccationEmployees.Any()
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
}
