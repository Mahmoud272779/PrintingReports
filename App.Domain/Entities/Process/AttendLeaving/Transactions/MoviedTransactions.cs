using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving.Transactions
{
    public class MoviedTransactions
    {
        public int Id { get; set; }
        public int EmployeesId { get; set; }
        public DateTime day { get; set; }




        public int? shift1_BranchIdIn { get; set; } // فرع الحضور
        public int? shift1_BranchIdOut { get; set; } // فرع الانصراف
        public DateTime? shift1_TimeIn { get; set; } // وقت الحضور
        public DateTime? shift1_TimeOut { get; set; } //وقت الانصراف
        public TimeSpan? shift1_ExtraTimeBefore { get; set; }  // الاضافي قبل الدوام
        public TimeSpan? shift1_ExtraTimeAfter { get; set; } // الاضافي بعد الدوام
        public TimeSpan? shift1_LateTime { get; set; } // التاخير
        public TimeSpan? shift1_LeaveEarly { get; set; } // الانصراف المبكر
        public TimeSpan? shift1_TotalShiftHours { get; set; } // عدد سعات الدوام
        public TimeSpan? shift1_TotalWorkHours { get; set; } // عدد ساعات العمل الفعلي



        public int? shift2_BranchIdIn { get; set; } // فرع الحضور
        public int? shift2_BranchIdOut { get; set; } // فرع الانصراف
        public DateTime? shift2_TimeIn { get; set; } // وقت الحضور
        public DateTime? shift2_TimeOut { get; set; } //وقت الانصراف
        public TimeSpan? shift2_ExtraTimeBefore { get; set; }  // الاضافي قبل الدوام
        public TimeSpan? shift2_ExtraTimeAfter { get; set; } // الاضافي بعد الدوام
        public TimeSpan? shift2_LateTime { get; set; } // التاخير
        public TimeSpan? shift2_LeaveEarly { get; set; } // الانصراف المبكر
        public TimeSpan? shift2_TotalShiftHours { get; set; } // عدد سعات الدوام
        public TimeSpan? shift2_TotalWorkHours { get; set; } // عدد ساعات العمل الفعلي
        public bool IsHaveShift2 { get; set; } = false;


        public int? shift3_BranchIdIn { get; set; } // فرع الحضور
        public int? shift3_BranchIdOut { get; set; } // فرع الانصراف
        public DateTime? shift3_TimeIn { get; set; } // وقت الحضور
        public DateTime? shift3_TimeOut { get; set; } //وقت الانصراف
        public TimeSpan? shift3_ExtraTimeBefore { get; set; }  // الاضافي قبل الدوام
        public TimeSpan? shift3_ExtraTimeAfter { get; set; } // الاضافي بعد الدوام
        public TimeSpan? shift3_LateTime { get; set; } // التاخير
        public TimeSpan? shift3_LeaveEarly { get; set; } // الانصراف المبكر
        public TimeSpan? shift3_TotalShiftHours { get; set; } // عدد سعات الدوام
        public TimeSpan? shift3_TotalWorkHours { get; set; } // عدد ساعات العمل الفعلي
        public bool IsHaveShift3 { get; set; } = false;


        public int? shift4_BranchIdIn { get; set; } // فرع الحضور
        public int? shift4_BranchIdOut { get; set; } // فرع الانصراف
        public DateTime? shift4_TimeIn { get; set; } // وقت الحضور
        public DateTime? shift4_TimeOut { get; set; } //وقت الانصراف
        public TimeSpan? shift4_ExtraTimeBefore { get; set; }  // الاضافي قبل الدوام
        public TimeSpan? shift4_ExtraTimeAfter { get; set; } // الاضافي بعد الدوام
        public TimeSpan? shift4_LateTime { get; set; } // التاخير
        public TimeSpan? shift4_LeaveEarly { get; set; } // الانصراف المبكر
        public TimeSpan? shift4_TotalShiftHours { get; set; } // عدد سعات الدوام
        public TimeSpan? shift4_TotalWorkHours { get; set; } // عدد ساعات العمل الفعلي
        public bool IsHaveShift4 { get; set; } = false;




        public DateTime cDate { get; set; }
        public bool IsEdited { get; set; } = false;
        public bool IsAbsance { get; set; } = false;
        public bool IsHoliday { get; set; } = false;
        public InvEmployees Employees { get; set; }
    }
}
