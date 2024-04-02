using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.TimeCalculationHelper.NormalShiftCalcTimeInTimeOut;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Shift;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.TimeCalculationHelper
{
    public static class CalcExistEmployeeDay
    {
        public static async Task<MoviedTransactions> calcExistEmployeeDay(InvEmployees emp, MoviedTransactions day, List<MachineTransactions> employeeTransaction, App.Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings settings, IRepositoryQuery<ChangefulTimeGroupsEmployees> _ChangefulTimeGroupsEmployeesQuery, IRepositoryQuery<RamadanDate> _RamadanDateQuery, IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.AttendancPermission> _AttendancPermissionQuery)
        {

            var Emp_Shift = GetCurrentShiftForEmployee.GetEmployee_CurrentShift(emp, _ChangefulTimeGroupsEmployeesQuery,day.day,_RamadanDateQuery, _AttendancPermissionQuery);
            if (Emp_Shift == null)
                return null;


            #region shift 1
            var shift1 = ShiftCalc.Shift(emp, day, employeeTransaction, settings, new ShiftTime
            {
                startIn = Emp_Shift.shift1_startIn,
                endIn = Emp_Shift.shift1_endIn,
                startOut = Emp_Shift.shift1_startOut,
                endOut = Emp_Shift.shift1_endOut,
                Start = Emp_Shift.shift1_Start,
                End = Emp_Shift.shift1_End,
                IsExtended = Emp_Shift.shift1_IsExtended,
                lateBefore = Emp_Shift.shift1_lateBefore,
                lateAfter = Emp_Shift.shift1_lateAfter,
                IsVacation = Emp_Shift.IsVacation,
                shiftType = Emp_Shift.type,
                totalDayWork = Emp_Shift.totalHours_ForOpenShift,
                
                
            });

            day.shift1_TimeIn = shift1.TimeIn;
            day.shift1_TimeOut = shift1.TimeOut;
            day.shift1_BranchIdIn = shift1.BranchIdIn;
            day.shift1_ExtraTimeBefore = shift1.ExtraTimeBefore;
            day.shift1_BranchIdOut = shift1.BranchIdOut;
            day.shift1_ExtraTimeAfter = shift1.ExtraTimeAfter;
            day.shift1_LateTime = shift1.LateTime;
            day.shift1_LeaveEarly = shift1.LeaveEarly;
            day.shift1_TotalShiftHours = shift1.TotalShiftHours;
            day.shift1_TotalWorkHours = shift1.TotalWorkHours;
            day.IsHoliday = shift1.IsHoliday;
            #endregion
            #region Shift 2
            if (Emp_Shift.IsHaveShift2)
            {
                var shift2 = ShiftCalc.Shift(emp, day, employeeTransaction, settings, new ShiftTime
                {
                    startIn = Emp_Shift.shift2_startIn,
                    endIn = Emp_Shift.shift2_endIn,
                    startOut = Emp_Shift.shift2_startOut,
                    endOut = Emp_Shift.shift2_endOut,
                    Start = Emp_Shift.shift2_Start,
                    End = Emp_Shift.shift2_End,
                    IsExtended = Emp_Shift.shift2_IsExtended,
                    lateBefore = Emp_Shift.shift2_lateBefore,
                    lateAfter = Emp_Shift.shift2_lateAfter,
                    IsVacation = Emp_Shift.IsVacation,
                });
                day.shift2_TimeIn = shift2.TimeIn;
                day.shift2_TimeOut = shift2.TimeOut;
                day.shift2_BranchIdIn = shift2.BranchIdIn;
                day.shift2_ExtraTimeBefore = shift2.ExtraTimeBefore;
                day.shift2_BranchIdOut = shift2.BranchIdOut;
                day.shift2_ExtraTimeAfter = shift2.ExtraTimeAfter;
                day.shift2_LateTime = shift2.LateTime;
                day.shift2_LeaveEarly = shift2.LeaveEarly;
                day.shift2_TotalShiftHours = shift2.TotalShiftHours;
                day.shift2_TotalWorkHours = shift2.TotalWorkHours;
                day.IsHaveShift2 = Emp_Shift.IsHaveShift2;
                day.IsHoliday = shift2.IsHoliday;

            }
            #endregion
            #region Shift 3
            if (Emp_Shift.IsHaveShift3)
            {
                var shift3 = ShiftCalc.Shift(emp, day, employeeTransaction, settings, new ShiftTime
                {
                    startIn = Emp_Shift.shift3_startIn,
                    endIn = Emp_Shift.shift3_endIn,
                    startOut = Emp_Shift.shift3_startOut,
                    endOut = Emp_Shift.shift3_endOut,
                    Start = Emp_Shift.shift3_Start,
                    End = Emp_Shift.shift3_End,
                    IsExtended = Emp_Shift.shift3_IsExtended,
                    lateBefore = Emp_Shift.shift3_lateBefore,
                    lateAfter = Emp_Shift.shift3_lateAfter,
                    IsVacation = Emp_Shift.IsVacation,
                });
                day.shift3_TimeIn = shift3.TimeIn;
                day.shift3_TimeOut = shift3.TimeOut;
                day.shift3_BranchIdIn = shift3.BranchIdIn;
                day.shift3_ExtraTimeBefore = shift3.ExtraTimeBefore;
                day.shift3_BranchIdOut = shift3.BranchIdOut;
                day.shift3_ExtraTimeAfter = shift3.ExtraTimeAfter;
                day.shift3_LateTime = shift3.LateTime;
                day.shift3_LeaveEarly = shift3.LeaveEarly;
                day.shift3_TotalShiftHours = shift3.TotalShiftHours;
                day.shift3_TotalWorkHours = shift3.TotalWorkHours;
                day.IsHaveShift3 = Emp_Shift.IsHaveShift3;
                day.IsHoliday = shift3.IsHoliday;

            }
            #endregion
            #region Shift 4
            if (Emp_Shift.IsHaveShift4)
            {
                var shift4 = ShiftCalc.Shift(emp, day, employeeTransaction, settings, new ShiftTime
                {
                    startIn = Emp_Shift.shift4_startIn,
                    endIn = Emp_Shift.shift4_endIn,
                    startOut = Emp_Shift.shift4_startOut,
                    endOut = Emp_Shift.shift4_endOut,
                    Start = Emp_Shift.shift4_Start,
                    End = Emp_Shift.shift4_End,
                    IsExtended = Emp_Shift.shift4_IsExtended,
                    lateBefore = Emp_Shift.shift4_lateBefore,
                    lateAfter = Emp_Shift.shift4_lateAfter,
                    IsVacation = Emp_Shift.IsVacation,
                });
                day.shift4_TimeIn = shift4.TimeIn;
                day.shift4_TimeOut = shift4.TimeOut;
                day.shift4_BranchIdIn = shift4.BranchIdIn;
                day.shift4_ExtraTimeBefore = shift4.ExtraTimeBefore;
                day.shift4_BranchIdOut = shift4.BranchIdOut;
                day.shift4_ExtraTimeAfter = shift4.ExtraTimeAfter;
                day.shift4_LateTime = shift4.LateTime;
                day.shift4_LeaveEarly = shift4.LeaveEarly;
                day.shift4_TotalShiftHours = shift4.TotalShiftHours;
                day.shift4_TotalWorkHours = shift4.TotalWorkHours;
                day.IsHaveShift4 = Emp_Shift.IsHaveShift4;
                day.IsHoliday = shift4.IsHoliday;

            }
            #endregion


            return day;
        }
    }
}
