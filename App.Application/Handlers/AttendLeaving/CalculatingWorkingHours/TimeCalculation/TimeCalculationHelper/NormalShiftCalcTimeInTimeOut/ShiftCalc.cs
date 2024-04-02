using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.Models;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using System.Configuration;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.TimeCalculationHelper.NormalShiftCalcTimeInTimeOut
{
    public class ShiftCalcResponse
    {
        public DateTime? TimeIn { get; set; }
        public int? BranchIdIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public int? BranchIdOut { get; set; }
        public TimeSpan? TotalShiftHours { get; set; }
        public TimeSpan? ExtraTimeBefore { get; set; }
        public TimeSpan? ExtraTimeAfter { get; set; }
        public TimeSpan? LateTime { get; set; }
        public TimeSpan? TotalWorkHours { get; set; }
        public TimeSpan? LeaveEarly { get; set; }
        public bool IsHoliday { get; set; }
    }
    public class ShiftTime
    {
        public TimeSpan startIn { get; set; }
        public TimeSpan endIn { get; set; }
        public TimeSpan startOut { get; set; }
        public TimeSpan endOut { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public bool IsExtended { get; set; }
        public TimeSpan lateBefore { get; set; }
        public TimeSpan lateAfter { get; set; }
        public bool IsVacation { get; set; }
        public int shiftType { get; set; }
        public TimeSpan totalDayWork { get; set; }

    }
    public class ShiftCalc
    {
        public static ShiftCalcResponse Shift(InvEmployees emp, MoviedTransactions day, List<MachineTransactions> employeeTransaction, App.Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings settings, ShiftTime Emp_shift)
        {
            var theDay = new ShiftCalcResponse
            {
                TimeIn = null,
                BranchIdIn = null,
                TimeOut = null,
                BranchIdOut = null,
                TotalShiftHours = null,
                ExtraTimeBefore = null,
                ExtraTimeAfter = null,
                LateTime = null,
                TotalWorkHours = null,
                LeaveEarly = null,
                IsHoliday = false,
            };

            if (Emp_shift.shiftType != (int)Enums.shiftTypes.openShift)
            {
                //start
                var shift1_In_Transactions = employeeTransaction
                        .Where(c => !c.IsEdited ? c.TransactionDate.Date == day.day.Date : c.EditedTransactionDate.Date == day.day.Date)
                        .Where(c => !c.IsEdited ? c.TransactionDate.TimeOfDay >= Emp_shift.startIn && c.TransactionDate.TimeOfDay <= Emp_shift.endIn :
                        c.EditedTransactionDate.TimeOfDay >= Emp_shift.startIn && c.EditedTransactionDate.TimeOfDay <= Emp_shift.endIn)
                        .FirstOrDefault();
                if (shift1_In_Transactions != null)
                {
                    theDay.TimeIn = !shift1_In_Transactions.IsEdited ? shift1_In_Transactions.TransactionDate : shift1_In_Transactions.EditedTransactionDate;
                    theDay.BranchIdIn = shift1_In_Transactions.machine.branchId;
                }




                //end
                List<MachineTransactions> shift1_Out_Transactions = new List<MachineTransactions>();
                var startOut = Emp_shift.IsExtended ?  day.day.Date.AddDays(1).Add(Emp_shift.startOut) : day.day.Date.Add(Emp_shift.startOut);
                var endOut   = Emp_shift.IsExtended ? day.day.Date.AddDays(1).Add(Emp_shift.endOut) : day.day.Date.Add(Emp_shift.endOut);
                shift1_Out_Transactions = employeeTransaction
                    .Where(c => !c.IsEdited ?
                    (c.TransactionDate >= startOut && c.TransactionDate <= endOut)
                    :
                    (c.EditedTransactionDate >= startOut && c.EditedTransactionDate <= endOut)
                    ).ToList();
                if (shift1_Out_Transactions.Any())
                {
                    MachineTransactions timeOut = new MachineTransactions();
                    if (settings.SetLastMoveAsLeave)
                        timeOut = shift1_Out_Transactions.OrderBy(c => c.TransactionDate).LastOrDefault();
                    else
                        timeOut = shift1_Out_Transactions.FirstOrDefault();


                    theDay.TimeOut = !timeOut.IsEdited ? timeOut.TransactionDate : timeOut.EditedTransactionDate;
                    theDay.BranchIdOut = timeOut.machine.branchId;
                }
            }
            else
            {
                var dayStart = day.day.Date.AddHours(emp.shiftsMaster.dayEndTime.TimeOfDay.TotalHours);
                var dayEnd = day.day.AddDays(1).Date.AddHours(emp.shiftsMaster.dayEndTime.TimeOfDay.TotalHours);

                var dayTransaction = employeeTransaction.Where(c => c.TransactionDate >= dayStart && c.TransactionDate < dayEnd).ToList();
                if (dayTransaction.Any())
                {
                    theDay.TimeIn = dayTransaction.OrderBy(c => c.TransactionDate).FirstOrDefault().TransactionDate;
                    theDay.BranchIdIn = dayTransaction.OrderBy(c => c.TransactionDate).FirstOrDefault().machine.branchId;
                    dayTransaction.Remove(dayTransaction.OrderBy(c => c.TransactionDate).FirstOrDefault());
                }

                if (dayTransaction.Any())
                {
                    theDay.TimeOut = dayTransaction.OrderBy(c => c.TransactionDate).LastOrDefault().TransactionDate;
                    theDay.BranchIdOut = dayTransaction.OrderBy(c => c.TransactionDate).LastOrDefault().machine.branchId;
                }

            }





            theDay.TotalShiftHours = !Emp_shift.IsExtended ? Emp_shift.End - Emp_shift.Start : DateTime.Today.AddDays(1).Add(Emp_shift.End) - DateTime.Today.Add(Emp_shift.Start);
            if (theDay.TotalShiftHours > new TimeSpan(24))
            {
                if (!Emp_shift.IsExtended)
                {
                    theDay.TotalShiftHours = Emp_shift.End - Emp_shift.Start;
                }
                else
                {
                    theDay.TotalShiftHours = Emp_shift.End - (Emp_shift.Start - TimeSpan.FromHours(24));
                }

            }

            if (theDay.TimeIn != null && theDay.TimeOut != null)
            {
                if(emp.shiftsMaster.shiftType != (int)Enums.shiftTypes.openShift)
                {
                    theDay.ExtraTimeBefore = ShiftHelper.calcExtraTime_BeforeShift(emp, settings, Emp_shift.Start, theDay.TimeIn);
                    theDay.ExtraTimeAfter = ShiftHelper.calcExtraTime_AfterShift(emp, settings, Emp_shift.End, theDay.TimeOut);
                    theDay.LateTime = ShiftHelper.lateTimeBefore(Emp_shift.Start, theDay.TimeIn, Emp_shift.lateBefore);
                    theDay.LeaveEarly = ShiftHelper.LeaveEarly(Emp_shift.End, theDay.TimeOut, Emp_shift.lateAfter);
                }

                theDay.TotalWorkHours = theDay.TimeOut - theDay.TimeIn;
                if(emp.shiftsMaster.shiftType == (int)Enums.shiftTypes.openShift)
                {
                    theDay.ExtraTimeAfter = theDay.TotalWorkHours > Emp_shift.totalDayWork ? theDay.TotalWorkHours - Emp_shift.totalDayWork : TimeSpan.Zero;
                    theDay.LateTime = theDay.TotalWorkHours < Emp_shift.totalDayWork ? Emp_shift.totalDayWork - theDay.TotalWorkHours : TimeSpan.Zero;
                }
 
            }
            else
            {
                theDay.LateTime = theDay.TotalShiftHours;
            }


            theDay.IsHoliday = Emp_shift.IsVacation;
            return theDay;
        }
    }
}
