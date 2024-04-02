using App.Domain.Entities.Process.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Domain.Models.Response.HR.AttendLeaving;
using Org.BouncyCastle.Ocsp;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.GetMasterShift
{
    public class GetMasterShiftHandler : IRequestHandler<GetMasterShiftRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<ShiftsMaster> _ShiftsMasterQuery;

        public GetMasterShiftHandler(IRepositoryQuery<ShiftsMaster> shiftsMasterQuery)
        {
            _ShiftsMasterQuery = shiftsMasterQuery;
        }

        public async Task<ResponseResult> Handle(GetMasterShiftRequest request, CancellationToken cancellationToken)
        {
            request.searchCriteria = request.name;
            var data = _ShiftsMasterQuery.TableNoTracking.Include(c => c.InvEmployees)
                .Include(c => c.normalShiftDetalies)
                .Include(c => c.changefulTimeGroups);
            var totalData = data.Count();
            var res = data.Where(c => !string.IsNullOrEmpty(request.searchCriteria) ?  c.arabicName.Contains(request.searchCriteria) || c.latinName.Contains(request.searchCriteria) : true);
            var dataCount = res.Count();
            var response = res
                .Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
                .Take(request.PageSize ?? 0)
                .ToList()
                .Select(c => new GetMasterShift_ResponseDTO
                {
                    Id = c.Id,
                    arabicName = c.arabicName,
                    latinName = c.latinName,
                    dayEndTime = c.dayEndTime,
                    shiftType = c.shiftType,
                    shiftTypeNameAr = Lists.shitTypes.FirstOrDefault(x => x.Id == c.shiftType).arabicName,
                    shiftTypeNameEn = Lists.shitTypes.FirstOrDefault(x => x.Id == c.shiftType).latinName,
                    candelete = !c.InvEmployees.Any() && !c.normalShiftDetalies.Any() && !c.changefulTimeGroups.Any()

                }) ;

            return new ResponseResult
            {
                Result = Result.Success,
                Data = response.OrderByDescending(x => x.Id),
                
            Note = Aliases.GetEndOfData(request.PageSize ?? 0, dataCount, request.PageNumber ?? 0),
                DataCount = dataCount,
                TotalCount = totalData
            };
        }
    }


}
