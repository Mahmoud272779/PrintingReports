using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure.Persistence.Context;
using App.Infrastructure.Persistence.Seed;
using App.Infrastructure.settings;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Helpers.UpdateSystem.Updates
{
    internal class updateNum5
    {
        public static async void Update_5(ClientSqlDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            method_1_AttendLeavingSettings(dbContext, webHostEnvironment);

            dbContext.invGeneralSettings.FirstOrDefault().SystemUpdateNumber = 5;
            dbContext.SaveChanges();
        }
        //set methods for update here


        private async static void method_1_AttendLeavingSettings(ClientSqlDbContext dbContext, IWebHostEnvironment _webHostEnvironment)
        {
            dbContext.attendLeaving_Settings.Add(new AttendLeaving_Settings
            {
                SetLastMoveAsLeave = false,
                //is_TimeOfNeglectingMovementsInMinutes = false,
                is_The_maximum_delay_in_minutes = false,
                is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift = false,
                is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift = false,
                is_The_Maximum_limit_for_early_dismissal_in_minutes = false,
                is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift = false,
                is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift = false,

                The_maximum_delay_in_minutes = new TimeSpan(),
                The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift = new TimeSpan(),
                The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift = new TimeSpan(),
                The_Maximum_limit_for_early_dismissal_in_minutes = new TimeSpan(),
                The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift = new TimeSpan(),
                The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift = new TimeSpan(),
                TimeOfNeglectingMovementsInMinutes = new TimeSpan(),
            });
            dbContext.SaveChanges();
        }
    }
}
