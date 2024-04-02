﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class Nationality
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public ICollection<InvEmployees> InvEmployees { get; set; }
    }
}
