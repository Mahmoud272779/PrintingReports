using App.Application.Services.HelperService.AttendLeavingServices;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Dashboard.AttendingLeaveDetalies
{
    public class AttendingLeaveDetaliesHandler : IRequestHandler<AttendingLeaveDetaliesRequest, ResponseResult>
    {
        private readonly IAttendLeavingService _attendLeavingService;
        private readonly IRepositoryQuery<MachineTransactions> _MachineTransactionsQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery;
        private readonly IRepositoryQuery<RamadanDate> _RamadanDateQuery;

        public AttendingLeaveDetaliesHandler(IAttendLeavingService attendLeavingService, IRepositoryQuery<MachineTransactions> machineTransactionsQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesQuery, IRepositoryQuery<RamadanDate> ramadanDateQuery)
        {
            _attendLeavingService = attendLeavingService;
            _MachineTransactionsQuery = machineTransactionsQuery;
            _VaccationEmployeesQuery = vaccationEmployeesQuery;
            _RamadanDateQuery = ramadanDateQuery;
        }

        public async Task<ResponseResult> Handle(AttendingLeaveDetaliesRequest request, CancellationToken cancellationToken)
        {
            var employees = await _attendLeavingService.GetInvEmployeesForCurrentUser(request.branchId);
            employees = employees
                .Include(c => c.shiftsMaster)
                .ThenInclude(c => c.normalShiftDetalies)
                .Include(c => c.shiftsMaster.changefulTimeGroups)
                .ThenInclude(c => c.changefulTimeDays);
            bool isRamadan = CalcTimingHelper.IsRamadan(_RamadanDateQuery, request.day.Date);
            var dayId = Lists.days.Where(c => c.latinName == request.day.DayOfWeek.ToString()).FirstOrDefault().Id;
            var machineTransactions = _MachineTransactionsQuery
                .TableNoTracking
                .Where(c => c.TransactionDate.Date == request.day.Date);

            var empVacations = _VaccationEmployeesQuery.TableNoTracking
                .Where(c => request.day.Date >= c.DateFrom && request.day.Date <= c.DateTo)
                .Where(c => employees.Select(x => x.Id).Contains(c.EmployeeId))
                .Count();

            var normalShiftWeekendCount = employees.Count(c => c.shiftsMaster.normalShiftDetalies.Where(x => x.DayId == dayId && x.IsRamadan == isRamadan).Any(x => x.IsVacation));
            var ChangefulShiftWeekendCount = employees.Count(c => c.shiftsMaster.changefulTimeGroups.FirstOrDefault().changefulTimeDays.Where(x => x.day == request.day.Date && x.IsRamadan == isRamadan).Any(x => x.IsVacation));

            var empsHasPrintFingers = machineTransactions.Select(c => c.EmployeeCode)
                .GroupBy(c => c)
                .Select(c => c.FirstOrDefault())
                .ToArray();

            var empsHasNoPrintFingers = employees.Where(c => !empsHasPrintFingers.Contains(c.Code));


            #region absence
            var normalShiftAbsenceEmployees = empsHasNoPrintFingers
                .Where(c => c.shiftsMaster.shiftType == (int)Enums.shiftTypes.normal || c.shiftsMaster.shiftType == (int)Enums.shiftTypes.openShift)
                .Where(c =>
                 (c.shiftsMaster.normalShiftDetalies.FirstOrDefault(x => x.DayId == dayId && x.IsRamadan == isRamadan).shift1_endIn <= (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                 c.shiftsMaster.normalShiftDetalies.FirstOrDefault(x => x.DayId == dayId && x.IsRamadan == isRamadan).shift2_endIn <= (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                 c.shiftsMaster.normalShiftDetalies.FirstOrDefault(x => x.DayId == dayId && x.IsRamadan == isRamadan).shift3_endIn <= (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                 c.shiftsMaster.normalShiftDetalies.FirstOrDefault(x => x.DayId == dayId && x.IsRamadan == isRamadan).shift4_endIn <= (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay)) &&
                 c.shiftsMaster.normalShiftDetalies.FirstOrDefault(x => x.DayId == dayId && x.IsRamadan == isRamadan).TotalDayHours != TimeSpan.Zero
                 )
                .Count();


            var changefulAbsenceEmployees = empsHasNoPrintFingers
               .Where(c => c.shiftsMaster.shiftType == (int)Enums.shiftTypes.ChangefulTime)
               .Where(c =>
                c.shiftsMaster.changefulTimeGroups.FirstOrDefault().changefulTimeDays.FirstOrDefault(x => x.day == request.day.Date && x.IsRamadan == isRamadan).shift1_endIn <= (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                c.shiftsMaster.changefulTimeGroups.FirstOrDefault().changefulTimeDays.FirstOrDefault(x => x.day == request.day.Date && x.IsRamadan == isRamadan).shift2_endIn <= (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                c.shiftsMaster.changefulTimeGroups.FirstOrDefault().changefulTimeDays.FirstOrDefault(x => x.day == request.day.Date && x.IsRamadan == isRamadan).shift3_endIn <= (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                c.shiftsMaster.changefulTimeGroups.FirstOrDefault().changefulTimeDays.FirstOrDefault(x => x.day == request.day.Date && x.IsRamadan == isRamadan).shift4_endIn <= (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay))
               .Count();
            #endregion


            #region Waiting
            var normalShiftWaitingeEmployees = empsHasNoPrintFingers
                .Where(c => c.shiftsMaster.shiftType == (int)Enums.shiftTypes.normal || c.shiftsMaster.shiftType == (int)Enums.shiftTypes.openShift)
                .Where(c =>
                 c.shiftsMaster.normalShiftDetalies.FirstOrDefault(x => x.DayId == dayId && x.IsRamadan == isRamadan).shift1_endIn > (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                 c.shiftsMaster.normalShiftDetalies.FirstOrDefault(x => x.DayId == dayId && x.IsRamadan == isRamadan).shift2_endIn > (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                 c.shiftsMaster.normalShiftDetalies.FirstOrDefault(x => x.DayId == dayId && x.IsRamadan == isRamadan).shift3_endIn > (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                 c.shiftsMaster.normalShiftDetalies.FirstOrDefault(x => x.DayId == dayId && x.IsRamadan == isRamadan).shift4_endIn > (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                 c.shiftsMaster.normalShiftDetalies.FirstOrDefault(x => x.DayId == dayId && x.IsRamadan == isRamadan).TotalDayHours == TimeSpan.Zero)

                .Count();

            var changefulWaitingEmployees = empsHasNoPrintFingers
               .Where(c => c.shiftsMaster.shiftType == (int)Enums.shiftTypes.ChangefulTime)
               .Where(c =>
                c.shiftsMaster.changefulTimeGroups.FirstOrDefault().changefulTimeDays.FirstOrDefault(x => x.day == request.day.Date && x.IsRamadan == isRamadan).shift1_endIn > (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                c.shiftsMaster.changefulTimeGroups.FirstOrDefault().changefulTimeDays.FirstOrDefault(x => x.day == request.day.Date && x.IsRamadan == isRamadan).shift2_endIn > (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                c.shiftsMaster.changefulTimeGroups.FirstOrDefault().changefulTimeDays.FirstOrDefault(x => x.day == request.day.Date && x.IsRamadan == isRamadan).shift3_endIn > (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                c.shiftsMaster.changefulTimeGroups.FirstOrDefault().changefulTimeDays.FirstOrDefault(x => x.day == request.day.Date && x.IsRamadan == isRamadan).shift4_endIn > (request.day.Date == DateTime.Now.Date ? DateTime.Now.TimeOfDay : request.day.Date.AddHours(23).AddMinutes(59).TimeOfDay) ||
                c.shiftsMaster.changefulTimeGroups.FirstOrDefault().changefulTimeDays.FirstOrDefault(x => x.day == request.day.Date && x.IsRamadan == isRamadan) == null)
               .Count();
            #endregion

            var _vacationsCount = empVacations;
            var _weekendCount = normalShiftWeekendCount + ChangefulShiftWeekendCount;
            var _AttendedCount = machineTransactions
                            .Where(c => employees.Select(x => x.Code)
                            .ToArray()
                            .Contains(c.EmployeeCode))
                            .ToList()
                            .GroupBy(x => x.EmployeeCode)
                            .Select(c => c.FirstOrDefault())
                            .Count();


            var _absenceCount = changefulAbsenceEmployees + normalShiftAbsenceEmployees;
            var _waiting = normalShiftWaitingeEmployees + changefulWaitingEmployees;

            double _vacationsPercent = Math.Round((Convert.ToDouble(_vacationsCount) /  employees.Count()) * 100,2);
            double _weekendPercent =   Math.Round((Convert.ToDouble(_weekendCount) /  employees.Count()) * 100,2);
            double _AttendedPercent =  Math.Round((Convert.ToDouble(_AttendedCount) /  employees.Count()) * 100,2);
            double _absencePercent =   Math.Round((Convert.ToDouble(_absenceCount) /  employees.Count()) * 100,2);
            double _waitingPercent =   Math.Round((Convert.ToDouble(_waiting) /  employees.Count()) * 100,2);

            AttendingLeaveDetaliesResponse res = new AttendingLeaveDetaliesResponse
            {
                vacationsCount = _vacationsCount,
                vacationsPercent = _vacationsPercent,
                weekendCount = _weekendCount,
                weekendPercent = _weekendPercent,
                AttendedCount = _AttendedCount,
                AttendedPercent = _AttendedPercent,
                absenceCount = _absenceCount,
                absencePercent = _absencePercent,
                waitingCount = _waiting,
                waitingPercent = _waitingPercent,
                totalEmployees = employees.Count()

            };

            return new ResponseResult
            {
                Result = Result.Success,
                Data = res
            };

        }
    }
    public class AttendingLeaveDetaliesResponse
    {
        public int AttendedCount { get; set; }
        public int absenceCount { get; set; }
        public int vacationsCount { get; set; }
        public int weekendCount { get; set; }
        public int waitingCount { get; set; }
        public double AttendedPercent { get; set; }
        public double absencePercent { get; set; }
        public double vacationsPercent { get; set; }
        public double weekendPercent { get; set; }
        public double waitingPercent { get; set; }
        public int totalEmployees { get; set; }

    }
}
