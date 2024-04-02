using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.Models;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Shift;
using App.Domain.Models.Response.General;
using App.Infrastructure.settings;
using DocumentFormat.OpenXml.Drawing;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.TimeCalculationHelper
{
    public class GetCurrentShiftForEmployee
    {
        /// <summary>
        /// emp = emp inculde shiftmaster, NormalShiftDetalies,changefulTimeGroups,changefulTimeDays
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="_ChangefulTimeGroupsEmployeesQuery"></param>
        /// <returns></returns>
        public static Shift GetEmployee_CurrentShift(
            InvEmployees emp,
            IRepositoryQuery<ChangefulTimeGroupsEmployees>
            _ChangefulTimeGroupsEmployeesQuery,
            DateTime day,
            IRepositoryQuery<RamadanDate> _RamadanDateQuery,
            IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.AttendancPermission> _AttendancPermissionQuery)
        {
            var isRamadan = CalcTimingHelper.IsRamadan(_RamadanDateQuery, day);
            var dayId = Lists.days.Where(x => x.latinName == day.DayOfWeek.ToString()).First().Id;

            var HavePermission = _AttendancPermissionQuery.TableNoTracking.FirstOrDefault(c => c.EmpId == emp.Id && c.Day.Date == day.Date);
            if (HavePermission != null)
            {
                if (HavePermission.type == (int)Enums.PermissionTypeEnum.Day)
                    return null;
                var timeBeforeShift1EndAndShift1Start = (HavePermission.shift1_end.Value.TimeOfDay - HavePermission.shift1_start.Value.TimeOfDay) / 2.01;
                var timeBetweenShift2StartAndShift1End = HavePermission.haveShift2 ? (HavePermission.shift2_start - HavePermission.shift1_end) / 2.01 : TimeSpan.Zero;
                var timeBetweenShift2StartAndShift2End = HavePermission.haveShift2 ? (HavePermission.shift2_end - HavePermission.shift2_start) / 2.01 : TimeSpan.Zero;
                var timeBetweenShift3StartAndShift2End = HavePermission.haveShift2 && HavePermission.haveShift3 ? (HavePermission.shift3_start - HavePermission.shift2_end) / 2.01 : TimeSpan.Zero;
                var timeBetweenShift3StartAndShift3End = HavePermission.haveShift3 ? (HavePermission.shift3_end - HavePermission.shift3_start) / 2.01 : TimeSpan.Zero;
                var timeBetweenShift4StartAndShift3End = HavePermission.haveShift3 && HavePermission.haveShift4 ? (HavePermission.shift4_start - HavePermission.shift3_end) / 2.01 : TimeSpan.Zero;
                var timeBetweenShift4EndAndSHift4Start = HavePermission.haveShift4 ? (HavePermission.shift4_end - HavePermission.shift4_start) / 2.01 : TimeSpan.Zero;


                Shift shift = null;
                try
                {
                    shift = new Shift
                    {
                        shift1_startIn = HavePermission.shift1_start.Value.TimeOfDay.Subtract(TimeSpan.FromHours(3)),
                        shift1_endIn = HavePermission.shift1_start.Value.TimeOfDay.Add(timeBeforeShift1EndAndShift1Start),
                        shift1_startOut = HavePermission.shift1_end.Value.TimeOfDay.Subtract(timeBeforeShift1EndAndShift1Start),
                        shift1_endOut = HavePermission.haveShift2 ? HavePermission.shift1_end.Value.TimeOfDay.Add(timeBetweenShift2StartAndShift1End.Value) : HavePermission.shift1_end.Value.TimeOfDay.Add(TimeSpan.FromHours(3)),
                        shift1_Start = HavePermission.shift1_start?.TimeOfDay ?? TimeSpan.Zero,
                        shift1_End = HavePermission.shift1_end?.TimeOfDay ?? TimeSpan.Zero,
                        shift1_IsExtended = HavePermission.isShift1_extended,
                        shift1_lateBefore = TimeSpan.Zero,
                        shift1_lateAfter = TimeSpan.Zero,


                        shift2_startIn = HavePermission.haveShift2 ? HavePermission.shift2_start?.TimeOfDay.Subtract(timeBetweenShift2StartAndShift1End.Value) ?? TimeSpan.Zero : TimeSpan.Zero,
                        shift2_endIn = HavePermission.haveShift2 ? HavePermission.shift2_start?.TimeOfDay.Add(timeBetweenShift2StartAndShift1End.Value) ?? TimeSpan.Zero : TimeSpan.Zero,

                        shift2_startOut = HavePermission.haveShift2 ? HavePermission.shift2_end?.TimeOfDay.Subtract(timeBetweenShift2StartAndShift2End.Value) ?? TimeSpan.Zero : TimeSpan.Zero,
                        shift2_endOut = HavePermission.haveShift2 ? HavePermission.shift2_end?.TimeOfDay.Add(timeBetweenShift2StartAndShift2End.Value) ?? TimeSpan.Zero : TimeSpan.Zero,

                        shift2_Start = HavePermission.haveShift2 ? HavePermission.shift2_start.Value.TimeOfDay : TimeSpan.Zero,
                        shift2_End = HavePermission.haveShift2 ? HavePermission.shift2_end.Value.TimeOfDay : TimeSpan.Zero,
                        IsHaveShift2 = HavePermission.haveShift2,
                        shift2_IsExtended = HavePermission.isShift2_extended,
                        shift2_lateBefore = TimeSpan.Zero,
                        shift2_lateAfter = TimeSpan.Zero,



                        shift3_startIn = HavePermission.haveShift3 ? HavePermission.shift3_start?.TimeOfDay.Subtract(timeBetweenShift3StartAndShift2End.Value) ?? TimeSpan.Zero : TimeSpan.Zero,
                        shift3_endIn = HavePermission.haveShift3 ? HavePermission.shift3_start?.TimeOfDay.Add(timeBetweenShift3StartAndShift2End.Value) ?? TimeSpan.Zero : TimeSpan.Zero,

                        shift3_startOut = HavePermission.haveShift3 ? HavePermission.shift3_end?.TimeOfDay.Subtract(timeBetweenShift3StartAndShift3End.Value) ?? TimeSpan.Zero : TimeSpan.Zero,
                        shift3_endOut = HavePermission.haveShift3 ? HavePermission.shift3_end?.TimeOfDay.Add(timeBetweenShift3StartAndShift3End.Value) ?? TimeSpan.Zero : TimeSpan.Zero,

                        shift3_Start = HavePermission.shift3_start?.TimeOfDay ?? TimeSpan.Zero,
                        shift3_End = HavePermission.shift3_end?.TimeOfDay ?? TimeSpan.Zero,
                        IsHaveShift3 = HavePermission.haveShift3,
                        shift3_IsExtended = HavePermission.isShift3_extended,
                        shift3_lateBefore = TimeSpan.Zero,
                        shift3_lateAfter = TimeSpan.Zero,



                        shift4_startIn = HavePermission.haveShift4 ? HavePermission.shift4_start?.TimeOfDay.Subtract(timeBetweenShift4StartAndShift3End.Value) ?? TimeSpan.Zero : TimeSpan.Zero,
                        shift4_endIn = HavePermission.haveShift4 ? HavePermission.shift4_start?.TimeOfDay.Add(timeBetweenShift4StartAndShift3End.Value) ?? TimeSpan.Zero : TimeSpan.Zero,

                        shift4_startOut = HavePermission.haveShift4 ? HavePermission.shift4_end?.TimeOfDay.Subtract(timeBetweenShift4EndAndSHift4Start.Value) ?? TimeSpan.Zero : TimeSpan.Zero,
                        shift4_endOut = HavePermission.haveShift4 ? HavePermission.shift4_end?.TimeOfDay.Add(timeBetweenShift4EndAndSHift4Start.Value) ?? TimeSpan.Zero : TimeSpan.Zero,

                        shift4_Start = HavePermission.shift4_start?.TimeOfDay??TimeSpan.Zero,
                        shift4_End = HavePermission.shift4_end?.TimeOfDay ?? TimeSpan.Zero,
                        IsHaveShift4 = HavePermission.haveShift4,
                        shift4_IsExtended = HavePermission.isShift4_extended,
                        shift4_lateBefore = TimeSpan.Zero,
                        shift4_lateAfter = TimeSpan.Zero,



                        type = emp.shiftsMaster.shiftType,
                        totalHours_ForOpenShift = HavePermission.totalHoursForOpenShift != null ? HavePermission.totalHoursForOpenShift.Value : TimeSpan.Zero,
                        IsVacation = false

                    };
                }
                catch (Exception)
                {

                }
                
                return shift;
            }
            else if (emp.shiftsMaster.shiftType == (int)shiftTypes.normal || emp.shiftsMaster.shiftType == (int)shiftTypes.openShift)
            {
                if (emp.shiftsMaster.normalShiftDetalies == null)
                    return null;
                NormalShiftDetalies Emp_shift = emp.shiftsMaster.normalShiftDetalies
                    .Where(c => c.IsRamadan == isRamadan)
                    .FirstOrDefault(c => c.ShiftId == emp.shiftsMasterId && c.DayId == dayId);

                if (Emp_shift == null)
                    return null;
                Shift shift = new Shift
                {
                    shift1_startIn = Emp_shift.shift1_startIn,
                    shift1_endIn = Emp_shift.shift1_endIn,
                    shift1_startOut = Emp_shift.shift1_startOut,
                    shift1_endOut = Emp_shift.shift1_endOut,
                    shift1_Start = Emp_shift.shift1_Start,
                    shift1_End = Emp_shift.shift1_End,
                    shift1_IsExtended = Emp_shift.shift1_IsExtended,
                    shift1_lateBefore = Emp_shift.shift1_lateBefore,
                    shift1_lateAfter = Emp_shift.shift1_lateAfter,
                    shift2_startIn = Emp_shift.shift2_startIn,
                    shift2_endIn = Emp_shift.shift2_endIn,
                    shift2_startOut = Emp_shift.shift2_startOut,
                    shift2_endOut = Emp_shift.shift2_endOut,
                    shift2_Start = Emp_shift.shift2_Start,
                    shift2_End = Emp_shift.shift2_End,
                    IsHaveShift2 = Emp_shift.IsHaveShift2,
                    shift2_IsExtended = Emp_shift.shift2_IsExtended,
                    shift2_lateBefore = Emp_shift.shift2_lateBefore,
                    shift2_lateAfter = Emp_shift.shift2_lateAfter,
                    shift3_startIn = Emp_shift.shift3_startIn,
                    shift3_endIn = Emp_shift.shift3_endIn,
                    shift3_startOut = Emp_shift.shift3_startOut,
                    shift3_endOut = Emp_shift.shift3_endOut,
                    shift3_Start = Emp_shift.shift3_Start,
                    shift3_End = Emp_shift.shift3_End,
                    IsHaveShift3 = Emp_shift.IsHaveShift3,
                    shift3_IsExtended = Emp_shift.shift3_IsExtended,
                    shift3_lateBefore = Emp_shift.shift3_lateBefore,
                    shift3_lateAfter = Emp_shift.shift3_lateAfter,
                    shift4_startIn = Emp_shift.shift4_startIn,
                    shift4_endIn = Emp_shift.shift4_endIn,
                    shift4_startOut = Emp_shift.shift4_startOut,
                    shift4_endOut = Emp_shift.shift4_endOut,
                    shift4_Start = Emp_shift.shift4_Start,
                    shift4_End = Emp_shift.shift4_End,
                    IsHaveShift4 = Emp_shift.IsHaveShift4,
                    shift4_IsExtended = Emp_shift.shift4_IsExtended,
                    shift4_lateBefore = Emp_shift.shift4_lateBefore,
                    shift4_lateAfter = Emp_shift.shift4_lateAfter,
                    type = emp.shiftsMaster.shiftType,
                    totalHours_ForOpenShift = Emp_shift.TotalDayHours,
                    IsVacation = Emp_shift.IsVacation

                };
                return shift;
            }
            else if (emp.shiftsMaster.shiftType == (int)shiftTypes.ChangefulTime)
            {
                var employeeGroup = _ChangefulTimeGroupsEmployeesQuery?.TableNoTracking.Where(c => c.invEmployeesId == emp.Id).FirstOrDefault();
                var Emp_shift = emp.shiftsMaster.changefulTimeGroups
                    .FirstOrDefault()
                    .changefulTimeDays
                    .Where(c => c.IsRamadan == isRamadan)
                    .Where(c => c.changefulTimeGroupsId == employeeGroup.Id)
                    .Where(c => c.day.Date == day.Date)
                    .FirstOrDefault();
                if (Emp_shift == null)
                    return null;
                Shift shift = new Shift
                {
                    shift1_startIn = Emp_shift.shift1_startIn,
                    shift1_endIn = Emp_shift.shift1_endIn,
                    shift1_startOut = Emp_shift.shift1_startOut,
                    shift1_endOut = Emp_shift.shift1_endOut,
                    shift1_Start = Emp_shift.shift1_Start,
                    shift1_End = Emp_shift.shift1_End,
                    shift1_IsExtended = Emp_shift.shift1_IsExtended,
                    shift1_lateBefore = Emp_shift.shift1_lateBefore,
                    shift1_lateAfter = Emp_shift.shift1_lateAfter,
                    shift2_startIn = Emp_shift.shift2_startIn,
                    shift2_endIn = Emp_shift.shift2_endIn,
                    shift2_startOut = Emp_shift.shift2_startOut,
                    shift2_endOut = Emp_shift.shift2_endOut,
                    shift2_Start = Emp_shift.shift2_Start,
                    shift2_End = Emp_shift.shift2_End,
                    IsHaveShift2 = Emp_shift.IsHaveShift2,
                    shift2_IsExtended = Emp_shift.shift2_IsExtended,
                    shift2_lateBefore = Emp_shift.shift2_lateBefore,
                    shift2_lateAfter = Emp_shift.shift2_lateAfter,
                    shift3_startIn = Emp_shift.shift3_startIn,
                    shift3_endIn = Emp_shift.shift3_endIn,
                    shift3_startOut = Emp_shift.shift3_startOut,
                    shift3_endOut = Emp_shift.shift3_endOut,
                    shift3_Start = Emp_shift.shift3_Start,
                    shift3_End = Emp_shift.shift3_End,
                    IsHaveShift3 = Emp_shift.IsHaveShift3,
                    shift3_IsExtended = Emp_shift.shift3_IsExtended,
                    shift3_lateBefore = Emp_shift.shift3_lateBefore,
                    shift3_lateAfter = Emp_shift.shift3_lateAfter,
                    shift4_startIn = Emp_shift.shift4_startIn,
                    shift4_endIn = Emp_shift.shift4_endIn,
                    shift4_startOut = Emp_shift.shift4_startOut,
                    shift4_endOut = Emp_shift.shift4_endOut,
                    shift4_Start = Emp_shift.shift4_Start,
                    shift4_End = Emp_shift.shift4_End,
                    IsHaveShift4 = Emp_shift.IsHaveShift4,
                    shift4_IsExtended = Emp_shift.shift4_IsExtended,
                    shift4_lateBefore = Emp_shift.shift4_lateBefore,
                    shift4_lateAfter = Emp_shift.shift4_lateAfter,
                    type = (int)shiftTypes.normal,
                    IsVacation = Emp_shift.IsVacation
                };
                return shift;
            }
            return null;

        }


    }
}
