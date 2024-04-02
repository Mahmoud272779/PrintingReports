﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.GettingCompanyData.GettingCompanyDataDTOS
{
    public class ShiftsEntity
    {
        public int Id { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyUserId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public bool IsBlock { get; set; }
        public bool IsShiftOpen { get; set; }
        public bool IsEndNextDay { get; set; }
        public TimeSpan DayEndTime { get; set; }
        public decimal TotalHours { get; set; }
        public int CompanyID { get; set; }
    }
}
