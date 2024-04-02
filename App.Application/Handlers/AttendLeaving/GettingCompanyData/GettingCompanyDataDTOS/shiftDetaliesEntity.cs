using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.GettingCompanyData.GettingCompanyDataDTOS
{
    public class shiftDetaliesEntity
    {
        public int Id {get;set;}
        public decimal TotalDayHours {get;set;}
        public bool IsVacation {get;set;}
        public int DayId {get;set;}
        public TimeSpan InTime { get;set;}
        public TimeSpan OutTime { get;set;}
        public TimeSpan StartIn { get;set;}
        public TimeSpan StartOut {get;set;}
        public TimeSpan EndIn {get;set;}
        public TimeSpan EndOut {get;set;}
        public TimeSpan AvailableEarlyTime {get;set;}
        public TimeSpan AvailableLateTime {get;set;}
        public int ShiftId {get;set;}
        public int ShiftType {get;set;}
        public int Seq { get; set; }
    }
}
