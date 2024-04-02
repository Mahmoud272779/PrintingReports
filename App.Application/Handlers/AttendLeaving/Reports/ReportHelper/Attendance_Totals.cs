using App.Application.Handlers.AttendLeaving.AttendLeaving_Helper;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.ReportHelper
{
    public static class Attendance_Totals
    {
        public static string totalHours(List<MoviedTransactions> Transactions,
            DateTime dateFrom, DateTime dateTo, ShiftsMaster shift,
             IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> HolidaysEmployees,
            IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery,
            IRepositoryQuery<RamadanDate> _RamadanDateQuery,
            IMediator _mediator)
        {
            var totalDays = DatesService.GetDatesBetween(dateFrom, dateTo);
            double totalTime = 0;
            if (shift.shiftType == (int)shiftTypes.normal || shift.shiftType == (int)shiftTypes.openShift)
            {
                foreach (var item in totalDays)
                {
                    var isRamadan = CalcTimingHelper.IsRamadan(_RamadanDateQuery, item);
                    //var _status = StatusGenerator.GetStatus(Transactions.Where(c => c.day.Date == item.Date).FirstOrDefault(), Transactions.FirstOrDefault().Employees, item, HolidaysEmployees, _VaccationEmployeesQuery, _RamadanDateQuery);
                    var status = _mediator.Send(new AttendLeaving_GetStatusRequest { day = item, employees = Transactions.FirstOrDefault().Employees, transactions = Transactions.Where(c => c.day.Date == item.Date).FirstOrDefault() }).Result;



                    if (status.Id == (int)AttendLeavingStatusEnum.holiday || status.Id == (int)AttendLeavingStatusEnum.weekend)
                        continue;
                    totalTime += shift.normalShiftDetalies
                        .FirstOrDefault(c => c.DayId == Lists.days.Find(v => v.latinName == item.DayOfWeek.ToString()).Id && c.IsRamadan == isRamadan && !c.IsVacation)
                        .TotalDayHours
                        .TotalHours;
                }
            }
            else if (shift.shiftType == (int)shiftTypes.ChangefulTime)
            {
                var days = shift.changefulTimeGroups
                    .FirstOrDefault()
                    .changefulTimeDays
                    .Where(c => !c.IsVacation)
                    .Where(c=> c.IsRamadan == CalcTimingHelper.IsRamadan(_RamadanDateQuery, c.day));
                foreach (var item in days)
                {

                    //var _status = StatusGenerator.GetStatus(Transactions.Where(c => c.day.Date == item.day.Date).FirstOrDefault(), Transactions.FirstOrDefault().Employees, item.day, HolidaysEmployees, _VaccationEmployeesQuery, _RamadanDateQuery);
                    var status = _mediator.Send(new AttendLeaving_GetStatusRequest 
                    { 
                        day = item.day,
                        employees = Transactions.FirstOrDefault().Employees, 
                        transactions = Transactions.Where(c => c.day.Date == item.day.Date).FirstOrDefault()
                    }).Result;
                    if (status.Id == (int)AttendLeavingStatusEnum.holiday)
                        continue;

                    totalTime += (item.shift1_End - item.shift1_Start).TotalHours +
                                 (item.shift2_End - item.shift2_Start).TotalHours +
                                 (item.shift3_End - item.shift3_Start).TotalHours +
                                 (item.shift4_End - item.shift4_Start).TotalHours;
                }

            }

            return convertTimeSpanToString(TimeSpan.FromHours(totalTime));
        }
        public static string actualWorkingHours(List<MoviedTransactions> Transactions)
        {
            var zeroSpan = new TimeSpan();
            //var actualWorkingHours_Shift =
            //    Transactions.Sum(c =>
            //    (c.shift1_TotalShiftHours.GetValueOrDefault(zeroSpan).TotalHours - c.shift1_LateTime.GetValueOrDefault(zeroSpan).TotalHours) +
            //    (c.shift2_TotalShiftHours.GetValueOrDefault(zeroSpan).TotalHours - c.shift2_LateTime.GetValueOrDefault(zeroSpan).TotalHours) +
            //    (c.shift3_TotalShiftHours.GetValueOrDefault(zeroSpan).TotalHours - c.shift3_LateTime.GetValueOrDefault(zeroSpan).TotalHours) +
            //    (c.shift4_TotalShiftHours.GetValueOrDefault(zeroSpan).TotalHours - c.shift4_LateTime.GetValueOrDefault(zeroSpan).TotalHours) 
            
            //    );
            var actualWorkingHours_Shift =
                Transactions.Sum(c =>
                c.shift1_TotalWorkHours.GetValueOrDefault(zeroSpan).TotalHours  +
                c.shift2_TotalWorkHours.GetValueOrDefault(zeroSpan).TotalHours  +
                c.shift3_TotalWorkHours.GetValueOrDefault(zeroSpan).TotalHours  +
                c.shift4_TotalWorkHours.GetValueOrDefault(zeroSpan).TotalHours );
            var time = TimeSpan.FromHours(actualWorkingHours_Shift);
            return convertTimeSpanToString(time);
        }
        public static string total_absence(List<MoviedTransactions> transactions, DateTime dateFrom, DateTime dateTo,
            IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> HolidaysEmployees,
            IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery, IRepositoryQuery<RamadanDate> _RamadanDateQuery,
            IMediator _mediator)
        {
            double totalAbsance = 0;
            var totalDays = DatesService.GetDatesBetween(dateFrom, dateTo);
            ShiftsMaster shift = transactions.FirstOrDefault().Employees.shiftsMaster;
            foreach (var item in totalDays)
            {
                //var _status = StatusGenerator.GetStatus(transactions.FirstOrDefault().Employees, item);
                var status = _mediator.Send(new AttendLeaving_GetStatusRequest
                {
                    day = item,
                    employees = transactions.FirstOrDefault().Employees,
                    transactions = transactions.Where(c => c.day.Date == item.Date).FirstOrDefault()
                }).Result;

                var dayId = Lists.days.Find(v => v.latinName == item.DayOfWeek.ToString()).Id;
                if (status.Id == (int)AttendLeavingStatusEnum.Absence)
                {
                    var isRamadan = CalcTimingHelper.IsRamadan(_RamadanDateQuery, item);

                    if (shift.shiftType == (int)shiftTypes.normal || shift.shiftType == (int)shiftTypes.openShift)
                        totalAbsance += shift.normalShiftDetalies.FirstOrDefault(c => c.DayId == dayId && c.IsRamadan == isRamadan && !c.IsVacation).TotalDayHours.TotalHours;
                    else
                        totalAbsance += shift
                            .changefulTimeGroups
                            .FirstOrDefault()
                            .changefulTimeDays
                            .Where(c => c.day.Date == item.Date && c.IsRamadan == isRamadan && !c.IsVacation)
                            .Sum(c =>
                            (c.shift1_End - c.shift1_Start).TotalHours +
                            (c.shift2_End - c.shift2_Start).TotalHours +
                            (c.shift3_End - c.shift3_Start).TotalHours +
                            (c.shift4_End - c.shift4_Start).TotalHours);
                }


            }

            return convertTimeSpanToString(TimeSpan.FromHours(totalAbsance));
        }

        public static string total_vacations(List<MoviedTransactions> transactions, DateTime dateFrom, DateTime dateTo,
           IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> HolidaysEmployees,
           IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery, IRepositoryQuery<RamadanDate> _RamadanDateQuery,
           IMediator _mediator)
        {
            double totalAbsance = 0;
            var totalDays = DatesService.GetDatesBetween(dateFrom, dateTo);
            ShiftsMaster shift = transactions.FirstOrDefault().Employees.shiftsMaster;
            foreach (var item in totalDays)
            {
                //var _status = StatusGenerator.GetStatus(transactions.Where(c => c.day.Date == item.Date).FirstOrDefault(), transactions.FirstOrDefault().Employees, item, HolidaysEmployees, _VaccationEmployeesQuery, _RamadanDateQuery);
                var status = _mediator.Send(new AttendLeaving_GetStatusRequest
                {
                    day = item,
                    employees = transactions.FirstOrDefault().Employees,
                    transactions = transactions.Where(c => c.day.Date == item.Date).FirstOrDefault()
                }).Result;


                var dayId = Lists.days.Find(v => v.latinName == item.DayOfWeek.ToString()).Id;
                if (status.Id == (int)AttendLeavingStatusEnum.Vacation)
                {
                    var isRamadan = CalcTimingHelper.IsRamadan(_RamadanDateQuery, item);

                    if (shift.shiftType == (int)shiftTypes.normal || shift.shiftType == (int)shiftTypes.openShift)
                        totalAbsance += shift.normalShiftDetalies.FirstOrDefault(c => c.DayId == dayId && c.IsRamadan == isRamadan && !c.IsVacation).TotalDayHours.TotalHours;
                    else
                        totalAbsance += shift
                            .changefulTimeGroups
                            .FirstOrDefault()
                            .changefulTimeDays
                            .Where(c => c.day.Date == item.Date && c.IsRamadan == isRamadan && !c.IsVacation)
                            .Sum(c =>
                            (c.shift1_End - c.shift1_Start).TotalHours +
                            (c.shift2_End - c.shift2_Start).TotalHours +
                            (c.shift3_End - c.shift3_Start).TotalHours +
                            (c.shift4_End - c.shift4_Start).TotalHours);
                }


            }



            return convertTimeSpanToString(TimeSpan.FromHours(totalAbsance));
        }
        public static string total_extraTime(List<MoviedTransactions> Transactions)
        {
            var zeroSpan = new TimeSpan();
            double totalTime = Transactions.Sum(c =>
                (c.Employees.Calculating_extra_time_before_work ? (c.shift1_ExtraTimeBefore.GetValueOrDefault(zeroSpan) + c.shift2_ExtraTimeBefore.GetValueOrDefault(zeroSpan) + c.shift3_ExtraTimeBefore.GetValueOrDefault(zeroSpan) + c.shift4_ExtraTimeBefore.GetValueOrDefault(zeroSpan)).TotalHours : 0) +
                (c.Employees.Calculating_extra_time_after_work ? (c.shift1_ExtraTimeAfter.GetValueOrDefault(zeroSpan) + c.shift2_ExtraTimeAfter.GetValueOrDefault(zeroSpan) + c.shift3_ExtraTimeAfter.GetValueOrDefault(zeroSpan) + c.shift4_ExtraTimeAfter.GetValueOrDefault(zeroSpan)).TotalHours : 0));
            return convertTimeSpanToString(TimeSpan.FromHours(totalTime));
        }
        public static string total_delay(List<MoviedTransactions> Transactions)
        {
            var zeroSpan = new TimeSpan();
            var actualWorkingHours_Shift =
                Transactions.Sum(c =>
                c.shift1_LateTime.GetValueOrDefault(zeroSpan).TotalHours + c.shift1_LeaveEarly.GetValueOrDefault(zeroSpan).TotalHours +
                c.shift2_LateTime.GetValueOrDefault(zeroSpan).TotalHours + c.shift2_LeaveEarly.GetValueOrDefault(zeroSpan).TotalHours +
                c.shift3_LateTime.GetValueOrDefault(zeroSpan).TotalHours + c.shift3_LeaveEarly.GetValueOrDefault(zeroSpan).TotalHours +
                c.shift4_LateTime.GetValueOrDefault(zeroSpan).TotalHours + c.shift4_LeaveEarly.GetValueOrDefault(zeroSpan).TotalHours
                );

            var newTime = TimeSpan.FromHours(actualWorkingHours_Shift);

            var res = convertTimeSpanToString(newTime);
            return res;
        }

        public static string convertTimeSpanToString(TimeSpan newTime)
        {
            return $"{(((int)newTime.TotalDays) * 24) + newTime.Hours:D2}:{newTime.Minutes:D2}";
        }
    }
}
