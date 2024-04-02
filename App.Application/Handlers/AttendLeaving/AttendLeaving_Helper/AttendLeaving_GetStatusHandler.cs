using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure.UserManagementDB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendLeaving_Helper
{
    public class AttendLeaving_GetStatusHandler : IRequestHandler<AttendLeaving_GetStatusRequest, AttendLeavingStatus>
    {
        private readonly IRepositoryQuery<RamadanDate> _RamadanDateQuery;
        IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> HolidaysEmployees;
        IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery;
        public AttendLeaving_GetStatusHandler(IRepositoryQuery<RamadanDate> ramadanDateQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployees, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesQuery)
        {
            _RamadanDateQuery = ramadanDateQuery;
            HolidaysEmployees = holidaysEmployees;
            _VaccationEmployeesQuery = vaccationEmployeesQuery;
        }
        public async Task<AttendLeavingStatus> Handle(AttendLeaving_GetStatusRequest request, CancellationToken cancellationToken)
        {
            var isRamadan = CalcTimingHelper.IsRamadan(_RamadanDateQuery, request.day);
            var zeroTimeSpan = new TimeSpan();
            var status = new AttendLeavingStatus()
            {
                Id = 0,
                arabicName = "",
                latinName = ""
            };
            bool weekend = false;

            if (request.employees.shiftsMaster == null)
            {
                status = new AttendLeavingStatus()
                {
                    Id = 0,
                    arabicName = "غير مسجل علي دوام",
                    latinName = "Has no shift"
                };
                return status;
            }

            var shiftType = request.employees.shiftsMaster.shiftType;
            if (shiftType == (int)Enums.shiftTypes.normal || shiftType == (int)Enums.shiftTypes.openShift)
            {
                var DayId = Lists.days.Where(c => c.latinName == request.day.DayOfWeek.ToString()).FirstOrDefault();
                if (!request.employees.shiftsMaster.normalShiftDetalies.Any(c => c.DayId == DayId.Id && c.IsRamadan == isRamadan))
                {
                    status = new AttendLeavingStatus()
                    {
                        Id = 0,
                        arabicName = "لم يتم ادحال ساعات الدوام فى هذا اليوم",
                        latinName = "Working Hours of this shift of this day are not logged"
                    };
                    return status;
                }
                if ((request.employees.shiftsMaster.normalShiftDetalies.FirstOrDefault(c => c.DayId == DayId.Id && c.IsRamadan == isRamadan).shift1_Start == zeroTimeSpan ||
                    request.employees.shiftsMaster.normalShiftDetalies.FirstOrDefault(c => c.DayId == DayId.Id && c.IsRamadan == isRamadan).shift1_End == zeroTimeSpan) && 
                    request.employees.shiftsMaster.normalShiftDetalies.FirstOrDefault(c=> c.DayId == DayId.Id && c.IsRamadan == isRamadan).TotalDayHours == zeroTimeSpan)
                {

                    status = new AttendLeavingStatus()
                    {
                        Id = 0,
                        arabicName = "لم يتم ادحال ساعات الدوام فى هذا اليوم",
                        latinName = "Working Hours of this shift of this day are not logged"
                    };
                    return status;
                }

                if (request.employees.shiftsMaster.normalShiftDetalies.FirstOrDefault(c => c.DayId == DayId.Id && c.IsRamadan == isRamadan) != null)
                    weekend = request.employees.shiftsMaster.normalShiftDetalies.FirstOrDefault(c => c.DayId == DayId.Id && c.IsRamadan == isRamadan).IsVacation;
                if (weekend)
                {
                    status = Lists.AttendLeavingStatus.Where(c => c.Id == (int)AttendLeavingStatusEnum.weekend).FirstOrDefault();
                    return status;
                }
            }
            else if (shiftType == (int)Enums.shiftTypes.ChangefulTime)
            {
                var theShiftDay = request.employees.shiftsMaster.changefulTimeGroups.FirstOrDefault().changefulTimeDays.FirstOrDefault(c => c.day.Date == request.day.Date && c.IsRamadan == isRamadan);
                if (theShiftDay == null)
                {
                    status = new AttendLeavingStatus()
                    {
                        Id = 0,
                        arabicName = "غير مسجل علي دوام",
                        latinName = "Has no shift"
                    };
                    return status;
                }
                weekend = theShiftDay.IsVacation;
                if (weekend)
                {
                    status = Lists.AttendLeavingStatus.Where(c => c.Id == (int)AttendLeavingStatusEnum.weekend).FirstOrDefault();
                    return status;
                }
            }

            //Holiday
            var holiday = HolidaysEmployees
                .TableNoTracking
                .Include(c => c.Holidays)
                .Where(c => c.EmployeesId == request.employees.Id && c.Holidays.enddate.Date >= DateTime.Now.Date)
                .FirstOrDefault();
            if (holiday != null)
            {
                status = new AttendLeavingStatus
                {
                    Id = (int)AttendLeavingStatusEnum.holiday,
                    arabicName = holiday.Holidays.arabicName,
                    latinName = holiday.Holidays.latinName
                };
                return status;
            }
            //vecation
            var empVecation = _VaccationEmployeesQuery
                .TableNoTracking
            .Include(c => c.Vaccations)
                .Where(c => c.EmployeeId == request.employees.Id)
                .Where(c => c.DateFrom.Date <= request.day.Date && c.DateTo.Date >= request.day.Date)
                .FirstOrDefault();
            if (empVecation != null)
            {
                status = new AttendLeavingStatus
                {
                    Id = (int)AttendLeavingStatusEnum.Vacation,
                    arabicName = empVecation.Vaccations.ArabicName,
                    latinName = empVecation.Vaccations.LatinName
                };
                return status;
            }



            if (request.transactions == null)
            {
                status = Lists.AttendLeavingStatus.Where(c => c.Id == (int)AttendLeavingStatusEnum.Absence).FirstOrDefault();
                return status;
            }

            var lateTime = request.transactions.shift1_LateTime.GetValueOrDefault(zeroTimeSpan) + request.transactions.shift1_LeaveEarly.GetValueOrDefault(zeroTimeSpan);
            if (request.transactions.IsHaveShift2)
                lateTime += request.transactions.shift2_LateTime.GetValueOrDefault(zeroTimeSpan) + request.transactions.shift2_LeaveEarly.GetValueOrDefault(zeroTimeSpan);
            if (request.transactions.IsHaveShift3)
                lateTime += request.transactions.shift3_LateTime.GetValueOrDefault(zeroTimeSpan) + request.transactions.shift3_LeaveEarly.GetValueOrDefault(zeroTimeSpan);
            if (request.transactions.IsHaveShift4)
                lateTime += request.transactions.shift4_LateTime.GetValueOrDefault(zeroTimeSpan) + request.transactions.shift4_LeaveEarly.GetValueOrDefault(zeroTimeSpan);


            if (request.transactions == null)
            {
                status = Lists.AttendLeavingStatus.Where(c => c.Id == (int)AttendLeavingStatusEnum.Absence).FirstOrDefault();
                return status;
            }
            
            if (lateTime > new TimeSpan() && lateTime.TotalMinutes >= 1 ) 
            {
                status = Lists.AttendLeavingStatus.Where(c => c.Id == (int)AttendLeavingStatusEnum.late).FirstOrDefault();
                return status;
            }
            status = Lists.AttendLeavingStatus.Where(c => c.Id == (int)AttendLeavingStatusEnum.working).FirstOrDefault();
            return status;
        }
    }
}
