using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts
{
    public static class ShiftHelper
    {
        public static async Task<List<App.Domain.Entities.Process.AttendLeaving.ChangefulTimeDays>> Calc_ChangefulTimeShiftDays(App.Domain.Models.Request.AttendLeaving.EditChangefulTimeDays request)
        {
            var list = new List<App.Domain.Entities.Process.AttendLeaving.ChangefulTimeDays>();

            var totalDays = request.endDate.Value.Date.Subtract(request.startDate.Value.Date).TotalDays;
            var day = request.startDate.Value;
            bool cont = true;
            for (int i = 0; i < totalDays;)
            {
                if (i >= totalDays)
                    break;
                foreach (var item in request.shifts)
                {
                    if (item.workDaysNumber == 0)
                    {
                        cont = false;
                        break;
                    }
                    if(item.weekendNumber == 0)
                    {
                        cont = false;
                        break;
                    }
                    if (i> totalDays)
                        break; 
                    //Work Days
                    for (int w = 0; w < item.workDaysNumber; w++)
                    {
                        if (i >= totalDays)
                            break;
                        list.Add(new Domain.Entities.Process.AttendLeaving.ChangefulTimeDays
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
                            IsHaveShift2 = item.IsHaveShift2,
                            shift2_IsExtended = item.shift2_IsExtended,
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

                            day = day,
                            changefulTimeGroupsId = request.changefulTimeGroupsId,
                            IsRamadan = request.IsRamadan
                        });
                        day = day.AddDays(1);
                        i++;
                        

                    }

                    //Weekend
                    for (int e = 0; e < item.weekendNumber; e++)
                    {
                        if (i >= totalDays)
                            break;
                        list.Add(new Domain.Entities.Process.AttendLeaving.ChangefulTimeDays
                        {
                            day = day,
                            IsVacation = true,
                            changefulTimeGroupsId = request.changefulTimeGroupsId
                        });
                        day = day.AddDays(1);
                        i++;
                    }
                }
                if (!cont)
                    break;
            }
            return list;
        }


    }
}
