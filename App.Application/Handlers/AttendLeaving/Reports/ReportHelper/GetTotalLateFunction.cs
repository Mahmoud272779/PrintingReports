using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.AttendLeaving_Helper;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.ReportHelper
{
    public static class GetTotalLateFunction
    {
        public static string GetTotalLate(List<DateTime> days, List<MoviedTransactions> movedTransactions,
            InvEmployees emp, IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployees,
         IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery, IRepositoryQuery<RamadanDate> _RamadanDateQuery,
         IMediator _mediator)
        {
            var zeroTimeSpan = new TimeSpan();
            var empLateTime= new TimeSpan();
            foreach (var day in days)
            {
                var currentTransaction = movedTransactions.Where(x => x.day.Date == day.Date && x.EmployeesId == emp.Id).FirstOrDefault();
                
                //var _status = StatusGenerator.GetStatus(currentTransaction, emp, day, _HolidaysEmployees, _VaccationEmployeesQuery, _RamadanDateQuery);
                var status = _mediator.Send(new AttendLeaving_GetStatusRequest
                {
                    day = day,
                    employees = emp,
                    transactions = currentTransaction
                }).Result;



                if (status.Id == (int)AttendLeavingStatusEnum.late)
                {
                    empLateTime += (currentTransaction.shift1_LateTime.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift2_LateTime.GetValueOrDefault(zeroTimeSpan))
                                  + (currentTransaction.shift3_LateTime.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift4_LateTime.GetValueOrDefault(zeroTimeSpan))
                                 + (currentTransaction.shift1_LeaveEarly.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift2_LeaveEarly.GetValueOrDefault(zeroTimeSpan))
                                 + (currentTransaction.shift3_LeaveEarly.GetValueOrDefault(zeroTimeSpan) + currentTransaction.shift4_LeaveEarly.GetValueOrDefault(zeroTimeSpan));

                }
            }

            return Attendance_Totals.convertTimeSpanToString(empLateTime);
        }
    }
}
