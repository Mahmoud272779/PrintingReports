using DocumentFormat.OpenXml.Office2010.Excel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.GetChangefulTimeGroupsMaster
{
    public class GetChangefulTimeGroupsMasterHandler : IRequestHandler<GetChangefulTimeGroupsMasterRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> _ChangefulTimeGroupsMasterQuery;

        public GetChangefulTimeGroupsMasterHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> changefulTimeGroupsMasterQuery)
        {
            _ChangefulTimeGroupsMasterQuery = changefulTimeGroupsMasterQuery;
        }

        public async Task<ResponseResult> Handle(GetChangefulTimeGroupsMasterRequest request, CancellationToken cancellationToken)
        {
            var data = _ChangefulTimeGroupsMasterQuery.TableNoTracking.Where(c=> c.shiftsMasterId == request.Id);
            var totalData = data.Count();
            var res = data.Where(c => !string.IsNullOrEmpty(request.SearchCriteria) ? request.SearchCriteria.Contains(c.arabicName) || request.SearchCriteria.Contains(c.latinName) : true)
                .Select(c => new {

                    Id = c.Id,
                    arabicName = c.arabicName,
                    latinName=c.latinName,
                    startDate=c.startDate,

                });
           
            var dataCount = res.Count();
            var response = res.Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0).Take(request.PageSize ?? 0);
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
