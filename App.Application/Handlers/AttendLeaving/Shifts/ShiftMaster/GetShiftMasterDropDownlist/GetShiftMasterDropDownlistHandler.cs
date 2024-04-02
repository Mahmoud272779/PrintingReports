using App.Domain.Entities.Process.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Domain.Models.Response.HR.AttendLeaving;
using static App.Domain.Models.Security.Authentication.Response.Store.EmployeeResponsesDTOs;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.GetMasterShift
{
    public class GetShiftMasterDropDownlistHandler : IRequestHandler<GetShiftMasterDropDownlistRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<ShiftsMaster> _ShiftsMasterQuery;
        private readonly IRepositoryQuery<ChangefulTimeGroupsMaster> _ChangefulTimeGroupsMasterQuery;

        public GetShiftMasterDropDownlistHandler(IRepositoryQuery<ShiftsMaster> shiftsMasterQuery, IRepositoryQuery<ChangefulTimeGroupsMaster> changefulTimeGroupsMasterQuery)
        {
            _ShiftsMasterQuery = shiftsMasterQuery;
            _ChangefulTimeGroupsMasterQuery = changefulTimeGroupsMasterQuery;
        }

        public async Task<ResponseResult> Handle(GetShiftMasterDropDownlistRequest request, CancellationToken cancellationToken)
        {
            var _data = _ShiftsMasterQuery
                            .TableNoTracking
                            .Include(c => c.changefulTimeGroups)
                            .Where(c=> c.shiftType != (int)shiftTypes.ChangefulTime)
                            .Select(c=> new HiringInformation_shift
                            {
                                Id = c.Id,
                                shiftType = c.shiftType,
                                groupId = 0,
                                arabicName = c.arabicName,
                                latinName = c.latinName

                            }).ToList();

            var groups = _ChangefulTimeGroupsMasterQuery
                            .TableNoTracking
                            .Include(c => c.shiftsMaster)
                            .Select(c => new HiringInformation_shift
                            {
                                Id = c.shiftsMaster.Id,
                                groupId = c.Id,
                                shiftType = c.shiftsMaster.shiftType,
                                arabicName = c.shiftsMaster.arabicName + " - " + c.arabicName,
                                latinName = c.shiftsMaster.latinName + " - " + c.latinName
                            }).ToList();
            
            var data = _data.Union(groups);

            var totalData = data.Count();
            var res = data.Where(c => !string.IsNullOrEmpty(request.searchCriteria) ? request.searchCriteria.Contains(c.arabicName) || request.searchCriteria.Contains(c.latinName) : true);
            var dataCount = res.Count();
            var response = res
                .Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
                .Take(request.PageSize ?? 0)
                .ToList();

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
