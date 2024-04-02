using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.GettingCompanyData.GettingCompanyDataDTOS
{
    public class companyEntity
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string AdressAr { get; set; }
        public string AdressEn { get; set; }
        public string ActivityAr { get; set; }
        public string ActivityEn { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public int BundleID { get; set; }
        public int EmployeesCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Trial { get; set; }
        public string UniqueName { get; set; }
        public int Multiples { get; set; }
    }
}
