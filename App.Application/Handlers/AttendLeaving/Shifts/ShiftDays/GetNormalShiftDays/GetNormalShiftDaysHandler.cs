using App.Application.Handlers.AttendLeaving.Shifts.EditNormalShiftDays;
using App.Domain.Entities.Process.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.GetNormalShiftDays
{
    public class GetNormalShiftDaysHandler : IRequestHandler<GetNormalShiftDaysRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<NormalShiftDetalies> _NormalShiftDetaliesQuery;
        private readonly IRepositoryQuery<ShiftsMaster> _ShiftsMasterQuery;

        public GetNormalShiftDaysHandler(IRepositoryQuery<NormalShiftDetalies> normalShiftDetaliesQuery, IRepositoryQuery<ShiftsMaster> shiftsMasterQuery)
        {
            _NormalShiftDetaliesQuery = normalShiftDetaliesQuery;
            _ShiftsMasterQuery = shiftsMasterQuery;
        }

        public async Task<ResponseResult> Handle(GetNormalShiftDaysRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var shiftDetails = _ShiftsMasterQuery.TableNoTracking.Where(c => c.Id == request.ShiftId);
                var normalShiftDetails = await _NormalShiftDetaliesQuery
                    .TableNoTracking
                    .Where(c => c.ShiftId == request.ShiftId && c.IsRamadan == request.isRamadan)
                    .OrderBy(c => c.DayId)
                    .ToListAsync();

                return new ResponseResult
                {
                    Data = normalShiftDetails.Any() ? normalShiftDetails : null,
                    Result = Result.Success
                };
            }
            catch(Exception e)
            {
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Note = e.Message,
                    ErrorMessageEn = e.InnerException.Message
                };
            }
            

        }
    }
}
