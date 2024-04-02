﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Response.General
{
    public class paymentTypes
    {
        public int id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
    public class Days
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
    public class shitTypes
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }

    public class AttendLeavingStatus
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
    public class PermissionProps
    {
        public int Id { get; set; }
        public string Arabicname { get; set; }
        public string Latinname { get; set; }
    }
}
