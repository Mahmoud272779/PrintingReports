using App.Application.Helpers.UpdateSystem.Updates;
using App.Domain.Models.Shared;
using App.Infrastructure;
using App.Infrastructure.Persistence.Context;
using App.Infrastructure.Persistence.Seed;
using App.Infrastructure.settings;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Helpers.UpdateSystem.Services
{
    public class updateService : iUpdateService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IErpInitilizerData erpInitializerData;
        public updateService(IHttpContextAccessor httpContext, IWebHostEnvironment webHostEnvironment, IErpInitilizerData erpInitializerData)
        {
            _httpContext = httpContext;
            _webHostEnvironment = webHostEnvironment;
            this.erpInitializerData = erpInitializerData;
        }



        public async Task UpdateDatabase(ClientSqlDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            var DatabaseUpdateNumber = dbContext.invGeneralSettings.AsNoTracking().FirstOrDefault().SystemUpdateNumber;
            if(DatabaseUpdateNumber < defultData.updateNumber)
            {
                if(DatabaseUpdateNumber < 3)
                {
                    updateNum3.Update_3(dbContext, webHostEnvironment);
                    DatabaseUpdateNumber = dbContext.invGeneralSettings.AsNoTracking().FirstOrDefault().SystemUpdateNumber;
                }
                if (DatabaseUpdateNumber < 4)
                {
                    updateNum4.Update_4(dbContext, webHostEnvironment);
                    DatabaseUpdateNumber = dbContext.invGeneralSettings.AsNoTracking().FirstOrDefault().SystemUpdateNumber;
                }if (DatabaseUpdateNumber < 5)
                {
                    updateNum5.Update_5(dbContext, webHostEnvironment);
                    DatabaseUpdateNumber = dbContext.invGeneralSettings.AsNoTracking().FirstOrDefault().SystemUpdateNumber;
                }
               
            }

        }

        public async Task updateFile(ClientSqlDbContext dbContext, int updateFilesNumber)
        {
            //update number 1
            await updateNum1.Update_1(dbContext);
            //update Report Files
            await ReportFilesUpdate.AddPrintFiles(dbContext, _webHostEnvironment);
            //if (updateFilesNumber < 1)
            //{
            //    await ReportFilesUpdate.AddBarcodeFiles(dbContext, _webHostEnvironment);

            //}

            await ReportFilesUpdate.UpdatePrintFiles(dbContext, _webHostEnvironment);
            await updateNum1.Update_2(dbContext);
        }
    }
}
