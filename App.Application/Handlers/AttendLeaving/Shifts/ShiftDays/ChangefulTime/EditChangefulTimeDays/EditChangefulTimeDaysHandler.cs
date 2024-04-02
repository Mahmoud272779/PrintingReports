using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure;
using DocumentFormat.OpenXml.Wordprocessing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.EditChangefulTimeDays
{
    public class EditChangefulTimeDaysHandler : IRequestHandler<EditChangefulTimeDaysRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<ChangefulTimeGroupsDetalies> _ChangefulTimeGroupsDetaliesQuery;
        private readonly IRepositoryQuery<ChangefulTimeDays> _ChangefulTimeDaysQuery;

        private readonly IRepositoryCommand<ChangefulTimeGroupsDetalies> _ChangefulTimeGroupsDetaliesCommand;
        private readonly IRepositoryCommand<ChangefulTimeDays> _ChangefulTimeDaysCommand;

        public EditChangefulTimeDaysHandler(IRepositoryQuery<ChangefulTimeGroupsDetalies> changefulTimeGroupsDetaliesQuery, IRepositoryQuery<ChangefulTimeDays> changefulTimeDaysQuery, IRepositoryCommand<ChangefulTimeGroupsDetalies> changefulTimeGroupsDetaliesCommand, IRepositoryCommand<ChangefulTimeDays> changefulTimeDaysCommand)
        {
            _ChangefulTimeGroupsDetaliesQuery = changefulTimeGroupsDetaliesQuery;
            _ChangefulTimeDaysQuery = changefulTimeDaysQuery;
            _ChangefulTimeGroupsDetaliesCommand = changefulTimeGroupsDetaliesCommand;
            _ChangefulTimeDaysCommand = changefulTimeDaysCommand;
        }

        public async Task<ResponseResult> Handle(EditChangefulTimeDaysRequest request, CancellationToken cancellationToken)
        {
            var Detalies_Element = _ChangefulTimeGroupsDetaliesQuery.TableNoTracking.Include(c=> c.changefulTimeGroups)
                .Where(c => c.changefulTimeGroupsId == request.changefulTimeGroupsId && c.IsRamadan == request.IsRamadan);
            if (request.shifts.Any(c => c.workDaysNumber <= 0))
                return new ResponseResult
                {
                    Result =   Result.Failed,
                    Alart =   new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = "workdays must > 0", MessageEn = "workdays must > 0", titleAr = "خطأ", titleEn = "Error" }
                };
            if (request.shifts.Any(c => c.weekendNumber <= 0))
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = "worknumber must > 0", MessageEn = "worknumber must > 0", titleAr = "خطأ", titleEn = "Error" }
                };
            if (Detalies_Element.Any())
            {
                var days_Element = _ChangefulTimeDaysQuery.TableNoTracking
                .Where(c => c.changefulTimeGroupsId == request.changefulTimeGroupsId && c.IsRamadan == request.IsRamadan);
                if (days_Element.Any())
                {
                    _ChangefulTimeDaysCommand.RemoveRange(days_Element);
                    await _ChangefulTimeDaysCommand.SaveChanges();
                }
                _ChangefulTimeGroupsDetaliesCommand.RemoveRange(Detalies_Element);
                await _ChangefulTimeGroupsDetaliesCommand.SaveChanges();
            }
            var listOfDetalies = new List<ChangefulTimeGroupsDetalies>();

            //if(Detalies_Element.FirstOrDefault()==null)
            //    return new ResponseResult
            //    {
            //        Result = Result.Failed,
            //        Alart = new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = "معلومات المجموعة غير مدخلة", MessageEn = "معلومات المجموعة غير مدخلة", titleAr = "خطأ", titleEn = "Error" }
            //    };

            foreach (var item in request.shifts)
            {
                listOfDetalies.Add(new ChangefulTimeGroupsDetalies
                {
                    shift1_startIn = item.shift1_startIn,
                    shift1_endIn = item.shift1_endIn,
                    shift1_startOut = item.shift1_startOut,
                    shift1_endOut = item.shift1_endOut,
                    shift1_Start = item.shift1_Start,
                    shift1_End = item.shift1_End,
                    shift1_IsExtended = item.shift1_IsExtended,
                    shift1_lateAfter = item.shift1_lateAfter,
                    shift1_lateBefore = item.shift1_lateBefore,
                    

                    shift2_startIn = item.shift2_startIn,
                    shift2_endIn = item.shift2_endIn,
                    shift2_startOut = item.shift2_startOut,
                    shift2_endOut = item.shift2_endOut,
                    shift2_Start = item.shift2_Start,
                    shift2_End = item.shift2_End,
                    shift2_IsExtended = item.shift2_IsExtended,
                    IsHaveShift2 = item.IsHaveShift2,
                    shift2_lateAfter = item.shift2_lateAfter,
                    shift2_lateBefore = item.shift2_lateBefore,
                    


                    shift3_startIn = item.shift3_startIn,
                    shift3_endIn = item.shift3_endIn,
                    shift3_startOut = item.shift3_startOut,
                    shift3_endOut = item.shift3_endOut,
                    shift3_Start = item.shift3_Start,
                    shift3_End = item.shift3_End,
                    IsHaveShift3 = item.IsHaveShift3,
                    shift3_IsExtended = item.shift3_IsExtended,
                    shift3_lateAfter = item.shift3_lateAfter,
                    shift3_lateBefore = item.shift3_lateBefore,
                    

                    shift4_startIn = item.shift4_startIn,
                    shift4_endIn = item.shift4_endIn,
                    shift4_startOut = item.shift4_startOut,
                    shift4_endOut = item.shift4_endOut,
                    shift4_Start = item.shift4_Start,
                    shift4_End = item.shift4_End,
                    IsHaveShift4 = item.IsHaveShift4,
                    shift4_IsExtended = item.shift4_IsExtended,
                    shift4_lateAfter = item.shift4_lateAfter,
                    shift4_lateBefore = item.shift4_lateBefore,




                    startDate = Detalies_Element?.FirstOrDefault()?.changefulTimeGroups?.startDate?? default(DateTime),
                    endDate = Detalies_Element?.FirstOrDefault()?.changefulTimeGroups?.startDate.AddYears(1)?? default(DateTime),
                    workDaysNumber = item.workDaysNumber,
                    weekendNumber = item.weekendNumber,
                    changefulTimeGroupsId = request.changefulTimeGroupsId,
                    IsRamadan = request.IsRamadan
                });

            }
            request.startDate = Detalies_Element?.FirstOrDefault()?.changefulTimeGroups?.startDate?? default(DateTime);
            request.endDate = request.startDate.Value.AddYears(1);
            var days = await ShiftHelper.Calc_ChangefulTimeShiftDays(request);
            _ChangefulTimeGroupsDetaliesCommand.StartTransaction();
            _ChangefulTimeGroupsDetaliesCommand.AddRange(listOfDetalies);
            await _ChangefulTimeGroupsDetaliesCommand.SaveChanges();
            if(days.Count!=0)
            _ChangefulTimeDaysCommand.AddRange(days) ;
            var saved = await _ChangefulTimeDaysCommand.SaveChanges() > 0 ? true : false;
            _ChangefulTimeGroupsDetaliesCommand.CommitTransaction();

             return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
