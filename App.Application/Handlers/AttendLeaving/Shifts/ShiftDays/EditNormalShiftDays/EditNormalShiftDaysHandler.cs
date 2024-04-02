using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Models.Security.Authentication.Response.Store;
using App.Infrastructure;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.EditNormalShiftDays
{
    public class EditNormalShiftDaysHandler : IRequestHandler<EditNormalShiftDaysRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<NormalShiftDetalies> _NormalShiftDetaliesQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.ShiftsMaster> _ShiftMasterQuery;
        private readonly IRepositoryCommand<NormalShiftDetalies> _NormalShiftDetaliesCommand;

        public EditNormalShiftDaysHandler(IRepositoryQuery<NormalShiftDetalies> normalShiftDetaliesQuery, IRepositoryCommand<NormalShiftDetalies> normalShiftDetaliesCommand, IRepositoryQuery<ShiftsMaster> shiftMasterQuery)
        {
            _NormalShiftDetaliesQuery = normalShiftDetaliesQuery;
            _NormalShiftDetaliesCommand = normalShiftDetaliesCommand;
            _ShiftMasterQuery = shiftMasterQuery;
        }

        public async Task<ResponseResult> Handle(EditNormalShiftDaysRequest request, CancellationToken cancellationToken)
        {
            var shift1Duration = request.shift1_IsExtended ? CalculateShiftDuration(request.shift1_Start, request.shift1_End) : (request.shift1_End - request.shift1_Start);
            var shift2Duration = request.shift2_IsExtended ? CalculateShiftDuration(request.shift2_Start, request.shift2_End) : (request.shift2_End - request.shift2_Start);
            var shift3Duration = request.shift3_IsExtended ? CalculateShiftDuration(request.shift3_Start, request.shift3_End) : (request.shift3_End - request.shift3_Start);
            var shift4Duration = request.shift4_IsExtended ? CalculateShiftDuration(request.shift4_Start, request.shift4_End) : (request.shift4_End - request.shift4_Start);

            var totalDayHours = shift1Duration + shift2Duration + shift3Duration + shift4Duration;
            var shift = await _ShiftMasterQuery.GetByIdAsync(request.shiftId);
            if (shift == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = "هذا الدوام غير موجود", MessageEn = "This shift is not exist", titleAr = "خطأ", titleEn = "Error" }
                };
            bool saved = false;
            if (request.repeteAll)
            {
                var elementsExist = _NormalShiftDetaliesQuery.TableNoTracking.Where(c => c.ShiftId == request.shiftId && c.IsRamadan == request.IsRamadan);
                if (elementsExist.Any())
                {
                    _NormalShiftDetaliesCommand.RemoveRange(elementsExist);
                    await _NormalShiftDetaliesCommand.SaveChanges();
                }
                var listOf_NormalShiftDetalies = new List<NormalShiftDetalies>();
                if (request.totalHoursForOpenShift == null && shift.shiftType == (int)shiftTypes.openShift)
                {
                    return new ResponseResult
                    {
                        Result = Result.Failed,
                        Alart = new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = "يجب ادخال عدد ساعات الدوام", MessageEn = "You shoul add total working hours", titleAr = "خطأ", titleEn = "Error" }
                    };
                }
                foreach (var item in Lists.days)
                {
                    if (shift.shiftType == (int)shiftTypes.openShift)
                    {
                        var _totalDayHours = new TimeSpan();
                        if(!string.IsNullOrEmpty(request.totalHoursForOpenShift) || request.totalHoursForOpenShift != "null")
                        {
                            _totalDayHours = TimeSpan.ParseExact(request.totalHoursForOpenShift, "hh\\:mm", null);

                        }
                        listOf_NormalShiftDetalies.Add(new NormalShiftDetalies
                        {
                            IsVacation = request.IsVacation,
                            ShiftId = request.shiftId,
                            DayId = item.Id,
                            TotalDayHours = _totalDayHours, //request.totalHoursForOpenShift.Value,
                            IsRamadan = request.IsRamadan,
                        });
                    }
                    else
                    {
                        listOf_NormalShiftDetalies.Add(new NormalShiftDetalies
                        {
                            IsVacation = request.IsVacation,
                            ShiftId = request.shiftId,
                            DayId = item.Id,
                            TotalDayHours = totalDayHours,
                            IsRamadan = request.IsRamadan,

                            shift1_startIn = request.shift1_startIn,
                            shift1_endIn = request.shift1_endIn,
                            shift1_startOut = request.shift1_startOut,
                            shift1_endOut = request.shift1_endOut,
                            shift1_Start = request.shift1_Start,
                            shift1_End = request.shift1_End,
                            shift1_IsExtended = request.shift1_IsExtended,
                            shift1_lateAfter = request.shift1_lateAfter,
                            shift1_lateBefore = request.shift1_lateBefore,



                            shift2_startIn = request.shift2_startIn,
                            shift2_endIn = request.shift2_endIn,
                            shift2_startOut = request.shift2_startOut,
                            shift2_endOut = request.shift2_endOut,
                            shift2_Start = request.shift2_Start,
                            shift2_End = request.shift2_End,
                            IsHaveShift2 = request.IsHaveShift2,
                            shift2_IsExtended = request.shift2_IsExtended,
                            shift2_lateAfter = request.shift2_lateAfter,
                            shift2_lateBefore = request.shift2_lateBefore,



                            shift3_startIn = request.shift3_startIn,
                            shift3_endIn = request.shift3_endIn,
                            shift3_startOut = request.shift3_startOut,
                            shift3_endOut = request.shift3_endOut,
                            shift3_Start = request.shift3_Start,
                            shift3_End = request.shift3_End,
                            IsHaveShift3 = request.IsHaveShift3,
                            shift3_IsExtended = request.shift3_IsExtended,
                            shift3_lateAfter = request.shift3_lateAfter,
                            shift3_lateBefore = request.shift3_lateBefore,

                            shift4_startIn = request.shift4_startIn,
                            shift4_endIn = request.shift4_endIn,
                            shift4_startOut = request.shift4_startOut,
                            shift4_endOut = request.shift4_endOut,
                            shift4_Start = request.shift4_Start,
                            shift4_End = request.shift4_End,
                            shift4_IsExtended = request.shift4_IsExtended,
                            shift4_lateAfter = request.shift4_lateAfter,
                            shift4_lateBefore = request.shift4_lateBefore,
                            IsHaveShift4 = request.IsHaveShift4
                        });
                    }
                }
                saved = await _NormalShiftDetaliesCommand.AddAsync(listOf_NormalShiftDetalies);
                return new ResponseResult
                {
                    Result = saved ? Result.Success : Result.Failed,
                    Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
                };
            }
            else
            {
                var elementExist = _NormalShiftDetaliesQuery.TableNoTracking.Where(c => c.ShiftId == request.shiftId && c.IsRamadan == request.IsRamadan && c.DayId == request.DayId).FirstOrDefault();
                if (elementExist != null)
                {
                    _NormalShiftDetaliesCommand.Remove(elementExist);
                    await _NormalShiftDetaliesCommand.SaveChanges();

                }

                if (shift.shiftType == (int)shiftTypes.openShift)
                {
                    var _totalDayHours = new TimeSpan();
                    if (!string.IsNullOrEmpty(request.totalHoursForOpenShift) || request.totalHoursForOpenShift != "null")
                    {
                        _totalDayHours = TimeSpan.ParseExact(request.totalHoursForOpenShift, "hh\\:mm", null);

                    }
                    saved = await _NormalShiftDetaliesCommand.AddAsync(new NormalShiftDetalies
                    {
                        IsVacation = request.IsVacation,
                        ShiftId = request.shiftId,
                        DayId = request.DayId,
                        TotalDayHours = _totalDayHours,// request.totalHoursForOpenShift.Value,
                        IsRamadan = request.IsRamadan,
                    });
                }
                else
                {
                    saved = await _NormalShiftDetaliesCommand.AddAsync(new NormalShiftDetalies
                    {
                        IsVacation = request.IsVacation,
                        ShiftId = request.shiftId,
                        DayId = request.DayId,
                        TotalDayHours = totalDayHours,
                        IsRamadan = request.IsRamadan,

                        shift1_startIn = request.shift1_startIn,
                        shift1_endIn = request.shift1_endIn,
                        shift1_startOut = request.shift1_startOut,
                        shift1_endOut = request.shift1_endOut,
                        shift1_Start = request.shift1_Start,
                        shift1_End = request.shift1_End,
                        shift1_IsExtended = request.shift1_IsExtended,

                        IsHaveShift2 = request.IsHaveShift2,
                        shift2_startIn = request.shift2_startIn,
                        shift2_endIn = request.shift2_endIn,
                        shift2_startOut = request.shift2_startOut,
                        shift2_endOut = request.shift2_endOut,
                        shift2_Start = request.shift2_Start,
                        shift2_End = request.shift2_End,
                        shift2_IsExtended = request.shift2_IsExtended,

                        IsHaveShift3 = request.IsHaveShift3,
                        shift3_startIn = request.shift3_startIn,
                        shift3_endIn = request.shift3_endIn,
                        shift3_startOut = request.shift3_startOut,
                        shift3_endOut = request.shift3_endOut,
                        shift3_Start = request.shift3_Start,
                        shift3_End = request.shift3_End,
                        shift3_IsExtended = request.shift3_IsExtended,

                        IsHaveShift4 = request.IsHaveShift4,
                        shift4_startIn = request.shift4_startIn,
                        shift4_endIn = request.shift4_endIn,
                        shift4_startOut = request.shift4_startOut,
                        shift4_endOut = request.shift4_endOut,
                        shift4_Start = request.shift4_Start,
                        shift4_End = request.shift4_End,
                        shift4_IsExtended = request.shift4_IsExtended,
                    });

                }
                return new ResponseResult
                {
                    Result = saved ? Result.Success : Result.Failed,
                    Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
                };
            }
            return null;
        }


        TimeSpan CalculateShiftDuration(TimeSpan start, TimeSpan end)
        {

            if (end < start)
            {
                DateTime startDate = DateTime.Today;
                DateTime startDateTime = startDate.Add(start);
                DateTime endDateTime = startDate.Add(end).AddDays(1);

                return endDateTime - startDateTime;
            }
            else
            {
                return end - start;
            }

        }
    }
}
