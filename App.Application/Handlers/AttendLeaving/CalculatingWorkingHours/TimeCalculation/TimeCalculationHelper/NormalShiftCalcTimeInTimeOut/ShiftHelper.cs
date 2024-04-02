using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.Models;
using App.Application.Services.Process.Invoices;
using App.Domain.Entities.Process.AttendLeaving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.TimeCalculationHelper.NormalShiftCalcTimeInTimeOut
{
    public class ShiftHelper
    {
        public static TimeSpan calcExtraTime_BeforeShift(InvEmployees emp, App.Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings settings, TimeSpan shift_TimeIn, DateTime? employeeTimeIn)
        {
            var extraTime = new TimeSpan();
            if(employeeTimeIn == null)
                return extraTime;

            if (shift_TimeIn < employeeTimeIn.Value.TimeOfDay)
                return new TimeSpan();
            //if (!emp.Calculating_extra_time_before_work)
            //    return new TimeSpan();


            extraTime = shift_TimeIn - employeeTimeIn.Value.TimeOfDay;



            if (settings.is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift)
                if (extraTime < settings.The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift)
                    return new TimeSpan();


            if (settings.is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift)
                if (extraTime >= settings.The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift)
                    return settings.The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift;


            return extraTime;
        }


        public static TimeSpan calcExtraTime_AfterShift(InvEmployees emp, App.Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings settings, TimeSpan shift_TimeOut, DateTime? employeeTimeOut)
        {
            var extraTime = new TimeSpan();

            if (employeeTimeOut == null)
                return extraTime;
            if (shift_TimeOut > employeeTimeOut.Value.TimeOfDay)
                return new TimeSpan();



            //if (!emp.Calculating_extra_time_after_work)
            //    return new TimeSpan();



            extraTime = employeeTimeOut.Value.TimeOfDay - shift_TimeOut;



            if (settings.is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift)
                if (extraTime > settings.The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift)
                    return new TimeSpan();

            if (settings.is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift)
                if (extraTime <= settings.The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift)
                    return settings.The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift;


            return extraTime;
        }


        public static TimeSpan lateTimeBefore(TimeSpan shift_TimeIn, DateTime? employeeTimeIn, TimeSpan lateBefore)
        {
            var lateTime = new TimeSpan();
            if (employeeTimeIn == null)
                return lateTime;
            var shift_TimeInAndLateAv = shift_TimeIn.Add(lateBefore);
            if (employeeTimeIn.Value.TimeOfDay < shift_TimeInAndLateAv)
                return lateTime;
            if (employeeTimeIn.Value.TimeOfDay.Add(lateBefore) < shift_TimeIn)
                return lateTime;
            lateTime = employeeTimeIn.Value.TimeOfDay - shift_TimeIn;
            return lateTime;
        }
        public static TimeSpan LeaveEarly(TimeSpan shift_TimeOut, DateTime? employeeTimeOut, TimeSpan lateAfter)
        {
            var lateTime = new TimeSpan();
            if (employeeTimeOut == null)
                return lateTime;
            if (employeeTimeOut.Value.TimeOfDay > shift_TimeOut)
                return lateTime;
            if (employeeTimeOut.Value.TimeOfDay.Add(lateAfter) > shift_TimeOut)
                return lateTime;
            lateTime = shift_TimeOut - employeeTimeOut.Value.TimeOfDay;
            return lateTime;
        }

    }
}
