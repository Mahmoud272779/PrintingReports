using App.Domain.Common;
using App.Domain.Entities.Notification;
using App.Domain.Entities.POS;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Shift;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Domain.Entities.Process.General;
using App.Domain.Entities.Process.Store;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Domain.Entities.Chat.chat;

namespace App.Domain.Entities.Process
{
    public class InvEmployees
    {
        public int Id { get; set; }
        //Employee Definition
        public int Code { get; set; }
        public int Status { get; set; }//Represent the status of the Employee 1 if active 2 if inactive and if 3 the employee is still new 
        [Required]
        public string ArabicName { get; set; }
        public string LatinName { get; set; }
        public int? JobId { get; set; }
        public IFormFile Image { get; set; }
        public string ImagePath { get; set; }
        public string Notes { get; set; }
        public int? FirstLogmachineId { get; set; }



        //Hiring Information.
        public int gLBranchId { get; set; }
        public int? shiftsMasterId { get; set; }
        public int? SectionsId { get; set; }
        public int? DepartmentsId { get; set; }
        public int? employeesGroupId { get; set; }
        public int? ManagerId { get; set; }
        public int? projectsId { get; set; }
        public int? missionsId { get; set; }



        //General Leadger
        public int? FinancialAccountId { get; set; }



        public bool CanDelete { get; set; }
        public string UserId { get; set; }
        public DateTime UTime { get; set; }


        //Person Information
        public int? nationalityId { get; set; }
        public string? IDNumber { get; set; }
        public int? religionsId { get; set; }
        public DateTime? birthday { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? address { get; set; }




        //Attend Leaving Settings 
        public bool Deduction_of_delay_from_additional_time { get; set; } = false;
        public bool Calculating_extra_time_before_work { get; set; } = false;
        public bool Calculating_extra_time_after_work { get; set; } = false;
        public bool Adding_working_hours_on_vacations { get; set; } = false;
        public bool Auto_Dismissal_registration { get; set; } = false;




        //public virtual ICollection<GLBranch> Branches { get; set; }
        public InvJobs Job { get; set; }
        public SectionsAndDepartments Sections { get; set; }
        public SectionsAndDepartments Departments { get; set; }
        public Nationality nationality { get; set; }
        public Missions missions { get; set; }
        public GLFinancialAccount FinancialAccount { get; set; }
        public Projects projects { get; set; }
        public EmployeesGroup employeesGroup { get; set; }
        public ShiftsMaster shiftsMaster { get; set; }
        public GLBranch GLBranch { get; set; }
        public religions religions { get; set; }
        public Machines FirstLogmachine { get; set; }



        public virtual ICollection<InvEmployeeBranch> EmployeeBranches { get; set; }
        public ICollection<userAccount> userAccount { get; set; }
        public ICollection<InvoiceMaster> invoiceMasters { get; set; }
        public ICollection<POSInvoiceSuspension> POSInvoiceSuspension { get; set; }
        public ICollection<SystemHistoryLogs> SystemHistoryLogs { get; set; }
        public ICollection<signalR> signalR { get; set; }
        public ICollection<InvPersons> InvPersons { get; set; }
        public ICollection<POSSession> pOSSessionsStart { get; set; }
        public ICollection<POSSession> pOSSessionsEnd { get; set; }
        public ICollection<chatMessages> chatMessagesFrom { get; set; }
        public ICollection<chatMessages> chatMessagesTo { get; set; }
        public ICollection<chatGroups> chatGroups { get; set; }
        public ICollection<chatGroupMembers> chatGroupMembers { get; set; }
        public ICollection<POSSessionHistory> pOSSessionHistories { get; set; }
        public ICollection<NotificationsMaster> NotificationsMaster { get; set; }
        public ICollection<NotificationsMaster> NotificationsMaster_insertedBy { get; set; }
        public ICollection<NotificationSeen> NotificationSeen { get; set; }
        public ICollection<SectionsAndDepartments> sectionsAndDepartments { get; set; }
        public  ICollection< HolidaysEmployees> EmployeesHolidays { get; set; }
        public ICollection<VaccationEmployees> VaccationEmployees { get; set; }
        public ICollection<OfferPriceMaster> OfferPriceMaster { get; set; }
        public ICollection<MoviedTransactions> MoviedTransactions { get; set; }
        public ICollection<AttendancPermission> permissions { get; set; }
        public ICollection<ChangefulTimeGroupsEmployees> ChangefulTimeGroupsEmployees { get; set; }
    }
}
