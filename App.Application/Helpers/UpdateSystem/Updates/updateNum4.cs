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
    internal class updateNum4
    {
        public static async void Update_4(ClientSqlDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            method_1_addPrintFiles(dbContext, webHostEnvironment);

            dbContext.invGeneralSettings.FirstOrDefault().SystemUpdateNumber = 4;
            dbContext.SaveChanges();
        }
        //set methods for update here


        private async static void method_1_addPrintFiles(ClientSqlDbContext dbContext, IWebHostEnvironment _webHostEnvironment)
        {
            await ReportFilesUpdate.AddPrintFilesForUpdate(dbContext, _webHostEnvironment, 4);
        }
    }
}
