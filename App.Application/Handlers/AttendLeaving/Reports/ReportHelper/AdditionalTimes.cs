using App.Domain.Entities.Process.AttendLeaving.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.ReportHelper
{
    public static class AdditionalTimes
    {
        public static GenerateAdditionalTimesDTO GenerateAdditionalTimes(MoviedTransactions currentTransaction, InvEmployees emp)
        {
            var zeroSpan = new TimeSpan();


            var extraTime = currentTransaction.shift1_ExtraTimeAfter.GetValueOrDefault(zeroSpan) + currentTransaction.shift1_ExtraTimeBefore.GetValueOrDefault(zeroSpan);
            var lateTime = currentTransaction.shift1_LateTime.GetValueOrDefault(zeroSpan) +  currentTransaction.shift1_LeaveEarly.GetValueOrDefault(zeroSpan);
            var workingTime = currentTransaction.shift1_TotalWorkHours !=null ? currentTransaction.shift1_TotalWorkHours : TimeSpan.Zero;
            var totalShiftTime = currentTransaction.shift1_TotalShiftHours != null ? currentTransaction.shift1_TotalShiftHours : zeroSpan;


            if (currentTransaction.IsHaveShift2)
            {
                lateTime += currentTransaction.shift2_LateTime.GetValueOrDefault(zeroSpan) +currentTransaction.shift2_LeaveEarly.GetValueOrDefault(zeroSpan);
                extraTime += currentTransaction.shift2_ExtraTimeAfter.GetValueOrDefault(zeroSpan) + currentTransaction.shift2_ExtraTimeBefore.GetValueOrDefault(zeroSpan);
                workingTime += currentTransaction.shift2_TotalWorkHours.GetValueOrDefault(zeroSpan);
                totalShiftTime += currentTransaction.shift2_TotalShiftHours.GetValueOrDefault(zeroSpan); ;
            }
            if (currentTransaction.IsHaveShift3)
            {
                lateTime += currentTransaction.shift3_LateTime.GetValueOrDefault(zeroSpan) +currentTransaction.shift3_LeaveEarly.GetValueOrDefault(zeroSpan);
                extraTime +=  currentTransaction.shift3_ExtraTimeAfter.GetValueOrDefault(zeroSpan) + currentTransaction.shift3_ExtraTimeBefore.GetValueOrDefault(zeroSpan);
                workingTime += currentTransaction.shift3_TotalWorkHours.GetValueOrDefault(zeroSpan);
                totalShiftTime += currentTransaction.shift3_TotalShiftHours.GetValueOrDefault(zeroSpan);

            }
            if (currentTransaction.IsHaveShift4)
            {
                lateTime += currentTransaction.shift4_LateTime.GetValueOrDefault(zeroSpan) + currentTransaction.shift4_LeaveEarly.GetValueOrDefault(zeroSpan);
                extraTime += currentTransaction.shift4_ExtraTimeAfter.GetValueOrDefault(zeroSpan) + currentTransaction.shift4_ExtraTimeBefore.GetValueOrDefault(zeroSpan);
                workingTime += currentTransaction.shift4_TotalWorkHours;
                totalShiftTime += currentTransaction.shift4_TotalShiftHours;

            }
            if (emp.Deduction_of_delay_from_additional_time)
            {
                var theLate = lateTime;
                lateTime = lateTime - extraTime;
                if (lateTime < new TimeSpan())
                    lateTime = new TimeSpan();
                extraTime = extraTime - theLate;
                if (extraTime < new TimeSpan())
                    extraTime = new TimeSpan();
            }
            GenerateAdditionalTimesDTO res = new GenerateAdditionalTimesDTO
            {
                extraTime = extraTime,
                lateTime = lateTime,
                totalShiftTime = totalShiftTime,
                workingTime = workingTime 
            };
            return res;
        }
    }
    public class GenerateAdditionalTimesDTO
    {
        public TimeSpan? extraTime { get; set; }
        public TimeSpan? lateTime { get; set; }
        public TimeSpan? workingTime { get; set; }
        public TimeSpan? totalShiftTime { get; set; }
    }
}
