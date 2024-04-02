using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class AttendLeaving_Settings
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int BranchId { get; set; } = 1;

        [Range(1, 4)]
        public int numberOfShiftsInReports { get; set; } 

        
        public TimeSpan TimeOfNeglectingMovementsInMinutes { get; set; }

        public bool is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift { get; set; }
        public TimeSpan The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift { get; set; }

        public bool is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift { get; set; }
        public TimeSpan The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift { get; set; }

        public bool is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift { get; set; }
        public TimeSpan The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift { get; set; }

        public bool is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift { get; set; }
        public TimeSpan The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift { get; set; }

        public bool is_The_maximum_delay_in_minutes { get; set; }
        public TimeSpan The_maximum_delay_in_minutes { get; set; }

        public bool is_The_Maximum_limit_for_early_dismissal_in_minutes { get; set; }
        public TimeSpan The_Maximum_limit_for_early_dismissal_in_minutes { get; set; }

        public bool SetLastMoveAsLeave { get; set; }

        [JsonIgnore]
        public GLBranch GLBranch { get; set; } 
    }
}
