﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.GettingCompanyData.GettingCompanyDataDTOS
{
    public class WorkTasksEntity
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public string CreateUserId { get; set; }
        public string ModifyUserId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool IsBlock { get; set; }
    }
}
