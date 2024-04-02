using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.AttendLeaving_Settings.EditAttendLeaving_Settings
{
    public class EditAttendLeaving_SettingsRequest :  IRequest<ResponseResult>
    {
        [Range(1, 4)]
        public int numberOfShiftsInReports { get; set; }


        public int TimeOfNeglectingMovementsInMinutes { get; set; }

        public bool is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift { get; set; }
        public int The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift { get; set; }

        public bool is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift { get; set; }
        public int The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift { get; set; }

        public bool is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift { get; set; }
        public int The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift { get; set; }

        public bool is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift { get; set; }
        public int The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift { get; set; }

        public bool is_The_maximum_delay_in_minutes { get; set; }
        public int The_maximum_delay_in_minutes { get; set; }

        public bool is_The_Maximum_limit_for_early_dismissal_in_minutes { get; set; }
        public int The_Maximum_limit_for_early_dismissal_in_minutes { get; set; }

        public bool SetLastMoveAsLeave { get; set; }
    }
}
