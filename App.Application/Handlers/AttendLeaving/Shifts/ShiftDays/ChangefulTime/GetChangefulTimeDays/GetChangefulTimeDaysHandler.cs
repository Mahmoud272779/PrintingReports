using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Models.Response.HR.AttendLeaving;
using MediatR;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.GetChangefulTimeDays
{
    public class GetChangefulTimeDaysHandler : IRequestHandler<GetChangefulTimeDaysRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<ChangefulTimeDays> _ChangefulTimeDaysQuery;
        private readonly IRepositoryQuery<ChangefulTimeGroupsDetalies> _ChangefulTimeGroupsDetaliesQuery;

        public GetChangefulTimeDaysHandler(IRepositoryQuery<ChangefulTimeDays> changefulTimeDaysQuery, IRepositoryQuery<ChangefulTimeGroupsDetalies> changefulTimeGroupsDetaliesQuery)
        {
            _ChangefulTimeDaysQuery = changefulTimeDaysQuery;
            _ChangefulTimeGroupsDetaliesQuery = changefulTimeGroupsDetaliesQuery;
        }

        public async Task<ResponseResult> Handle(GetChangefulTimeDaysRequest request, CancellationToken cancellationToken)
        {
            var detalies = _ChangefulTimeGroupsDetaliesQuery.TableNoTracking
                .Where(c => c.changefulTimeGroupsId == request.changefulTimeGroupsId && c.IsRamadan == request.IsRamadan);
            var detalies_Res = detalies.Select(c => new GetChangefulTimeDaysDTO_Detalies
            {
                Id = c.Id,
                workDaysNumber = c.workDaysNumber,
                weekendNumber = c.weekendNumber,
                changefulTimeGroupsId = c.changefulTimeGroupsId,

                shift1_startIn = c.shift1_startIn,
                shift1_endIn = c.shift1_endIn,
                shift1_startOut = c.shift1_startOut,
                shift1_endOut = c.shift1_endOut,
                shift1_Start = c.shift1_Start,
                shift1_End = c.shift1_End,
                shift1_IsExtended = c.shift1_IsExtended,
                shift1_lateAfter = c.shift1_lateAfter,
                shift1_lateBefore = c.shift1_lateBefore,


                shift2_startIn = c.shift2_startIn,
                shift2_endIn = c.shift2_endIn,
                shift2_startOut = c.shift2_startOut,
                shift2_endOut = c.shift2_endOut,
                shift2_Start = c.shift2_Start,
                shift2_End = c.shift2_End,
                IsHaveShift2 = c.IsHaveShift2,
                shift2_IsExtended = c.shift2_IsExtended,
                shift2_lateBefore = c.shift2_lateBefore,
                shift2_lateAfter = c.shift2_lateAfter,

                shift3_startIn = c.shift3_startIn,
                shift3_endIn = c.shift3_endIn,
                shift3_startOut = c.shift3_startOut,
                shift3_endOut = c.shift3_endOut,
                shift3_Start = c.shift3_Start,
                shift3_End = c.shift3_End,
                IsHaveShift3 = c.IsHaveShift3,
                shift3_IsExtended = c.shift3_IsExtended,
                shift3_lateAfter = c.shift3_lateAfter,
                shift3_lateBefore = c.shift3_lateBefore,


                shift4_startIn = c.shift4_startIn,
                shift4_endIn = c.shift4_endIn,
                shift4_startOut = c.shift4_startOut,
                shift4_endOut = c.shift4_endOut,
                shift4_Start = c.shift4_Start,
                shift4_End = c.shift4_End,
                IsHaveShift4 = c.IsHaveShift4,
                shift4_IsExtended = c.shift4_IsExtended,
                shift4_lateAfter = c.shift4_lateAfter,
                shift4_lateBefore = c.shift4_lateBefore

            }).ToHashSet();
            if (!detalies_Res.Any())
                return new ResponseResult
                {
                    Result = Result.NoDataFound
                };
            //var Days = _ChangefulTimeDaysQuery
            //    .TableNoTracking
            //    .Where(c => c.changefulTimeGroupsId == request.changefulTimeGroupsId && c.IsRamadan == request.IsRamadan)
            //    .Select(c => new GetChangefulTimeDaysDTO_Days
            //    {
            //        Id = c.Id,
            //        IsVacation = c.IsVacation,
            //        changefulTimeGroupsId = c.changefulTimeGroupsId,
            //        shift1_startIn = c.shift1_startIn,
            //        shift1_endIn = c.shift1_endIn,
            //        shift1_startOut = c.shift1_startOut,
            //        shift1_endOut = c.shift1_endOut,
            //        shift1_Start = c.shift1_Start,
            //        shift1_End = c.shift1_End,
            //        shift1_IsExtended = c.shift1_IsExtended,
            //        shift1_lateAfter = c.shift1_lateAfter,
            //        shift1_lateBefore = c.shift1_lateBefore,



            //        shift2_startIn = c.shift2_startIn,
            //        shift2_endIn = c.shift2_endIn,
            //        shift2_startOut = c.shift2_startOut,
            //        shift2_endOut = c.shift2_endOut,
            //        shift2_Start = c.shift2_Start,
            //        shift2_End = c.shift2_End,
            //        IsHaveShift2 = c.IsHaveShift2,
            //        shift2_IsExtended = c.shift2_IsExtended,
            //        shift2_lateAfter = c.shift2_lateAfter,
            //        shift2_lateBefore = c.shift2_lateBefore,


            //        shift3_startIn = c.shift3_startIn,
            //        shift3_endIn = c.shift3_endIn,
            //        shift3_startOut = c.shift3_startOut,
            //        shift3_endOut = c.shift3_endOut,
            //        shift3_Start = c.shift3_Start,
            //        shift3_End = c.shift3_End,
            //        IsHaveShift3 = c.IsHaveShift3,
            //        shift3_IsExtended = c.shift3_IsExtended,
            //        shift3_lateAfter = c.shift3_lateAfter,
            //        shift3_lateBefore = c.shift3_lateBefore,
                    

            //        shift4_startIn = c.shift4_startIn,
            //        shift4_endIn = c.shift4_endIn,
            //        shift4_startOut = c.shift4_startOut,
            //        shift4_endOut = c.shift4_endOut,
            //        shift4_Start = c.shift4_Start,
            //        shift4_End = c.shift4_End,
            //        IsHaveShift4 = c.IsHaveShift4,
            //        shift4_IsExtended = c.shift4_IsExtended,
            //        shift4_lateAfter = c.shift4_lateAfter,
            //        shift4_lateBefore = c.shift4_lateBefore

            //    }).ToHashSet();
            //if(Days.Count==0)
            //return new ResponseResult
            //{
            //    Result = Result.Success,
            //    Alart = new Alart
            //    {
            //        AlartType = AlartType.warrning,
            //        type = AlartShow.popup,
            //        MessageAr = "يجب حساب الدوام",
            //        MessageEn = "You have to calculate this shift"
            //    }
            //};
            var res = new GetChangefulTimeDaysRsponseDTO
            {
                startDate = detalies.FirstOrDefault().startDate,
                endDate = detalies.FirstOrDefault().endDate,
                //days = Days,
                detlies = detalies_Res
            };
            return new ResponseResult
            {
                Result = Result.Success,
                Data = res
            };
        }
    }
}
