using App.Domain.Entities.Process;
using App.Domain.Enums;
using App.Infrastructure.Persistence.Context;
using App.Infrastructure.settings;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Domain.Enums.Enums;

namespace App.Application.Helpers.UpdateSystem.Updates
{
    public static class updateNum1
    {
        public async static Task Update_1(ClientSqlDbContext dbContext)
        {
            await Method1_GLPurchasesAndSalesSettingsAddCreditorFundsItems(dbContext);
        }
        public async static Task Update_2(ClientSqlDbContext dbContext)
        {
            //await Method_1_Update_2_UpdateRulesTable_MainFormId(dbContext);
        }


        private async static Task Method1_GLPurchasesAndSalesSettingsAddCreditorFundsItems(ClientSqlDbContext dbContext)
        {
            var branches = dbContext.branchs;
            var listOfGLPurchasesAndSalesSettings = defultData.New_getlistOfGLPurchasesAndSalesSettingsTable();
            var GLPurchasesAndSalesSettingsDB = dbContext.GLPurchasesAndSalesSettings;
            var listOfGLPurchasesForAdd = new List<GLPurchasesAndSalesSettings>();

            var itemFundCreditor = listOfGLPurchasesAndSalesSettings.Where(x => x.RecieptsType == (int)DocumentType.itemsFund && x.ReceiptElemntID == (int)DebitoAndCredito.creditor).FirstOrDefault();
            foreach (var item in branches)
            {
                if (item.Id == 1)
                    continue;
                //check if element exist
                if (GLPurchasesAndSalesSettingsDB.Where(x => x.branchId == item.Id && x.RecieptsType == (int)DocumentType.itemsFund && x.ReceiptElemntID == (int)DebitoAndCredito.creditor).Any())
                    continue;


                listOfGLPurchasesForAdd.Add(new GLPurchasesAndSalesSettings()
                {
                    ArabicName = itemFundCreditor.ArabicName,
                    LatinName = itemFundCreditor.LatinName,
                    FinancialAccountId = itemFundCreditor.FinancialAccountId,
                    MainType = itemFundCreditor.MainType,
                    ReceiptElemntID = itemFundCreditor.ReceiptElemntID,
                    branchId = item.Id,
                    RecieptsType = itemFundCreditor.RecieptsType
                });
            }
            if (listOfGLPurchasesForAdd.Any())
            {
                dbContext.GLPurchasesAndSalesSettings.AddRange(listOfGLPurchasesForAdd);
                var saved = await dbContext.SaveChangesAsync();
            }
        }



        private async static Task Method_1_Update_2_UpdateRulesTable_MainFormId(ClientSqlDbContext dbContext)
        {
            var listOfRules = returnSubForms.returnRules();
            var rules = dbContext.rules.ToList();
            rules.ForEach(x =>
            {
                x.mainFormCode = listOfRules.Where(c => c.subFormCode == x.subFormCode).FirstOrDefault().mainFormCode;
            });
            dbContext.rules.UpdateRange(rules);
            dbContext.SaveChanges();
        }
    }
}
