using App.Application.Handlers.Chat.PrivateChat;
using App.Infrastructure.Persistence.Context;
using App.Infrastructure.settings;
using App.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Helpers.UpdateSystem.Updates
{
    public class updateNum3
    {
        
        public static async void Update_3(ClientSqlDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            method_1_UpGeneralLedgerPermissionIntegration(dbContext);


            method_2_AddScreenNamesAndPrintFiles(dbContext, webHostEnvironment);
            method_3_SetCollectionReceiptsForSuperAdmin(dbContext);
            dbContext.invGeneralSettings.FirstOrDefault().SystemUpdateNumber = 3;
            dbContext.SaveChanges();
        }
        //set methods for update here
        private async static void method_1_UpGeneralLedgerPermissionIntegration(ClientSqlDbContext dbContext)
        {
            var creditElemets = dbContext.GLPurchasesAndSalesSettings.AsNoTracking().Where(c => c.RecieptsType == (int)DocumentType.AddPermission || c.RecieptsType == (int)DocumentType.ExtractPermission).ToList();
            creditElemets.ForEach(c => c.ReceiptElemntID = (int)DebitoAndCredito.creditor);
            dbContext.GLPurchasesAndSalesSettings.UpdateRange(creditElemets);
            dbContext.SaveChanges();



            var branches = dbContext.branchs;
            List<GLPurchasesAndSalesSettings> listOfData = new List<GLPurchasesAndSalesSettings>(); 
            var data = defultData.New_getlistOfGLPurchasesAndSalesSettingsTable().Where(c => c.Id == -23 || c.Id == -24).ToList();
            var financialAccounts = dbContext.financialAccounts;
            var GLPurchasesAndSalesSettings = dbContext.GLPurchasesAndSalesSettings.AsNoTracking();
            foreach (var item in branches)
            {
                var isExist = financialAccounts.Where(c => c.Id == data.FirstOrDefault().FinancialAccountId).Any();
                foreach (var it in data)
                {
                    it.FinancialAccountId = isExist ? it.FinancialAccountId : -102004;
                    if (GLPurchasesAndSalesSettings.Where(c => c.branchId == item.Id && c.ReceiptElemntID == it.ReceiptElemntID && c.RecieptsType == it.RecieptsType).Any())
                        continue;
                    listOfData.Add(new GLPurchasesAndSalesSettings
                    {
                        ArabicName = it.ArabicName,
                        FinancialAccountId = it.FinancialAccountId,
                        LatinName = it.LatinName,
                        MainType = it.MainType,
                        ReceiptElemntID = it.ReceiptElemntID,
                        RecieptsType = it.RecieptsType,
                        branchId = item.Id
                    });
                }
            }
            dbContext.GLPurchasesAndSalesSettings.AddRange(listOfData);
            dbContext.SaveChanges();
        }

        private  async static void method_2_AddScreenNamesAndPrintFiles(ClientSqlDbContext dbContext, IWebHostEnvironment _webHostEnvironment)
        {
            int[] subFormsIds = new int[] { 130, 131 };
            var isScreensExist = dbContext.screenNames.Where(c => c.Id == 130 || c.Id == 13);
            if (isScreensExist.Any())
                return;
            var screens = await UpdateScreenNames.UpdateScreens(subFormsIds);
            await PrepareInsertIdentityInsert<ScreenName>.Prepare(screens.ToArray(), dbContext, "screenNames");

            await ReportFilesUpdate.AddPrintFilesForUpdate(dbContext, _webHostEnvironment, 3);
        }
        private async static void method_3_SetCollectionReceiptsForSuperAdmin(ClientSqlDbContext dbContext)
        {
            var superAdminSettings = dbContext.otherSettings.Where(c => c.userAccountId == 1).FirstOrDefault();
            if (superAdminSettings != null)
            {
                superAdminSettings.CollectionReceipts = true;
                dbContext.SaveChanges();
            }
        }
    }
}
