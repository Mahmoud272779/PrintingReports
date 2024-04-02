using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.GettingCompanyData.GettingCompanyDataDTOS
{
    public class EmployeesEntity
    {
        public int? ShiftId { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyUserId { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string PhoneNumber { get; set; }
        public int? WorkTasksId { get; set; }
        public bool IsManager { get; set; }
        public string PlaceStay { get; set; }
        public int Id { get; set; }
        public bool Active { get; set; }
        public int? AdminstratId { get; set; }
        public int BranchId { get; set; }
        public int? DepartmentId { get; set; }
        public string Email { get; set; }
        public int EmployeeCode { get; set; }
        public int? GroupId { get; set; }
        public bool IsBlock { get; set; }
        public int? JobId { get; set; }
        public int? NationalitiesId { get; set; }
        public string NationalityNumber { get; set; }
        public int? ProjectId { get; set; }
        public bool IsSendMessage { get; set; }
        public bool IsSendEmailAbsent { get; set; }
        public bool IsSendEmailLate { get; set; }
        public bool IsSendEmailPermission { get; set; }
        public bool IsSendEmailVocation { get; set; }



        public bool AddWorkingHoursOnHoliday { get; set; }
        public bool LogOutWithoutFingerprint { get; set; }
        public bool ExtraTimeAfterWorking { get; set; }
        public bool ExtraTimeBeforeWorking { get; set; }
        public bool ProvideDelayDiscountingOvertime { get; set; }

    }
}
