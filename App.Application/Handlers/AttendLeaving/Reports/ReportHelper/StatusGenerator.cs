using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.ReportHelper
{
    public class StatusGenerator
    {
        public static AttendLeavingStatus GetStatus(MoviedTransactions transactions,InvEmployees employees,DateTime day,IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> HolidaysEmployees, IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery)
        {
            var status = new AttendLeavingStatus()
            {
                Id = 0,
                arabicName = "",
                latinName = ""
            };
            bool weekend = false;
           
            if(employees.shiftsMaster == null)
            {
                status = new AttendLeavingStatus()
                {
                    Id = 0,
                    arabicName = "غير مسجل علي دوام",
                    latinName = "Has no shift"
                };
                return status;
            }

            var shiftType = employees.shiftsMaster.shiftType;
            if(shiftType == (int)Enums.shiftTypes.normal || shiftType == (int)Enums.shiftTypes.openShift)
            {
                var DayId = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault();
                weekend = employees.shiftsMaster.normalShiftDetalies.FirstOrDefault(c => c.DayId == DayId.Id).IsVacation;
                if (weekend)
                {
                    status = Lists.AttendLeavingStatus.Where(c => c.Id == (int)AttendLeavingStatusEnum.weekend).FirstOrDefault();
                    return status;
                }
            }
            else if (shiftType == (int)Enums.shiftTypes.ChangefulTime)
            {
                var theShiftDay = employees.shiftsMaster.changefulTimeGroups.FirstOrDefault().changefulTimeDays.FirstOrDefault(c => c.day.Date == day.Date);
                if(theShiftDay == null)
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
            var holiday = HolidaysEmployees.TableNoTracking.Include(c=> c.Holidays).Where(c => c.EmployeesId == employees.Id && c.Holidays.enddate.Date >= DateTime.Now.Date).FirstOrDefault();
            if(holiday != null)
            {
                status = new AttendLeavingStatus
                {
                    Id = (int)AttendLeavingStatusEnum.holiday,
                    arabicName = holiday.Holidays.arabicName,
                    latinName  = holiday.Holidays.latinName
                };
                return status;
            }
            //vecation
            var empVecation = _VaccationEmployeesQuery
                .TableNoTracking
                .Include(c => c.Vaccations)
                .Where(c => c.EmployeeId == employees.Id)
                .Where(c => c.DateFrom.Date <= day.Date && c.DateTo.Date >= day.Date)
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



            if (transactions == null)
            {
                status = Lists.AttendLeavingStatus.Where(c => c.Id == (int)AttendLeavingStatusEnum.Absence).FirstOrDefault();
                return status;
            }

            var lateTime = transactions.shift1_LateTime + transactions.shift1_LeaveEarly;
            if (transactions.IsHaveShift2)
                lateTime += transactions.shift2_LateTime + transactions.shift2_LeaveEarly;
            if (transactions.IsHaveShift3)
                lateTime += transactions.shift3_LateTime + transactions.shift3_LeaveEarly;
            if (transactions.IsHaveShift4)
                lateTime += transactions.shift4_LateTime + transactions.shift4_LeaveEarly;



            if (transactions == null)
            {
                status = Lists.AttendLeavingStatus.Where(c => c.Id == (int)AttendLeavingStatusEnum.Absence).FirstOrDefault();
                return status;
            }
            
            if(lateTime > new TimeSpan())
            {
                status = Lists.AttendLeavingStatus.Where(c => c.Id == (int)AttendLeavingStatusEnum.late).FirstOrDefault();
                return status;
            }
            status = Lists.AttendLeavingStatus.Where(c => c.Id == (int)AttendLeavingStatusEnum.working).FirstOrDefault();
            return status;
        }
    }
}
