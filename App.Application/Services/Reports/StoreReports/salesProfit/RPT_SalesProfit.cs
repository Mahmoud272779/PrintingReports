using App.Application.Handlers.Profit;
using App.Application.Handlers.Reports.SalesReports.ItemsProfitServices;
using App.Application.Helpers.LinqExtensions;
using App.Domain.Models.Request.Store.Sales;
using App.Domain.Models.Response.Store.Reports.Sales;
using App.Domain.Models.Security.Authentication.Response.Store.Reports.Purchases;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Domain.Enums.Enums;
using DocumentType = App.Domain.Enums.Enums.DocumentType;

namespace App.Application.Services.Reports.StoreReports.salesProfit
{
    public class RPT_SalesProfit : IRPT_SalesProfit
    {
        private readonly IRepositoryQuery<InvoiceMaster> invoiceMasterQuery;
        private readonly IRoundNumbers roundNumbers;
        private readonly IPrepareDataForProfit preparingProfit;
        private readonly iUserInformation _iUserInformation;
        private readonly IGeneralPrint _iGeneralPrint;
        public RPT_SalesProfit(IRepositoryQuery<InvoiceMaster> InvoiceMasterQuery, IRoundNumbers _roundNumbers, IPrepareDataForProfit preparingProfit, iUserInformation iUserInformation, IGeneralPrint iGeneralPrint)
        {
            invoiceMasterQuery = InvoiceMasterQuery;
            roundNumbers = _roundNumbers;
            this.preparingProfit = preparingProfit;
            _iUserInformation = iUserInformation;
            _iGeneralPrint = iGeneralPrint;
        }
        public async Task<ResponseResult> GetSalesProfit(RPT_SalesProfitRequest Parameter, bool isPrint = false)
        {
            try
            {
                var userinfo = await _iUserInformation.GetUserInformation();
                var salesShowOtherPersons =  userinfo.otherSettings.salesShowOtherPersonsInv;
                var posShowOtherPersons =  userinfo.otherSettings.posShowOtherPersonsInv;
                var salesType = new List<int> { (int)DocumentType.Sales, (int)DocumentType.ReturnSales };
                var posType = new List<int>   { (int)DocumentType.POS  , (int)DocumentType.ReturnPOS   };
                //bool isCalculate = await preparingProfit.PreparingDataForProfit();
                //if (!isCalculate)
                //{ return new ResponseResult() { Result = Result.Failed, ErrorMessageAr = "There is error in calculate profit", ErrorMessageEn = "There is error in calculate profit" };}
                if (string.IsNullOrEmpty(Parameter.branches))
                    return new ResponseResult() { Result = Result.Failed, ErrorMessageAr = "branches is required" };
                var branches = Parameter.branches.Split(',').Select(c => int.Parse(c)).ToArray();
                #region MyRegion
 //var totalDataCount = invoiceMasterQuery.TableNoTracking
                //               .Where(h => h.InvoiceDate >= Parameter.DateFrom 
                //               && h.InvoiceDate <= Parameter.DateTo 
                //               && h.IsDeleted == false 
                //               && branches.Contains(h.BranchId)   
                //               && Lists.SalesWithOutDeleteInvoicesList.Contains(h.InvoiceTypeId)
                //               //(h.InvoiceTypeId == (int)DocumentType.POS || h.InvoiceTypeId == (int)DocumentType.Sales)
                //               && (Parameter.PaymentType == 0 ? 1 == 1 : h.PaymentType==Parameter.PaymentType)).Count();

                #endregion
               


                var data = invoiceMasterQuery.TableNoTracking
                               .Where(h => h.InvoiceDate >= Parameter.DateFrom 
                               && h.InvoiceDate <= Parameter.DateTo 
                               && h.IsDeleted == false 
                               && branches.Contains(h.BranchId)

                               && (!salesShowOtherPersons && salesType.Contains(h.InvoiceTypeId) ? h.EmployeeId == userinfo.employeeId : 1==1)            
                               && (!posShowOtherPersons && posType.Contains(h.InvoiceTypeId) ? h.EmployeeId == userinfo.employeeId : 1==1)

                               &&(Lists.SalesWithOutDeleteInvoicesList.Contains(h.InvoiceTypeId))   
                               && (Parameter.PaymentType == 0 ? 1 == 1 : h.PaymentType == Parameter.PaymentType))
                               .OrderBy(o => o.InvoiceDate.Date).ThenBy(a => a.Serialize)
                               .Select(s => new RPT_SalesProfitResponse
                               {
                                   rowClassName = (s.InvoiceTypeId == (int)DocumentType.ReturnPOS || s.InvoiceTypeId == (int)DocumentType.ReturnSales) ? "text-danger" : "",// returnList .Where(c => c == s.InvoiceTypeId).Any() ? "text-danger" : "",
                                   InvoiceCode = s.InvoiceType,
                                   DocumenTypeID = s.InvoiceTypeId,
                                   FullInvoiceDate = s.InvoiceDate,
                                    paymentTypeId = s.PaymentType,
                                   //PaymentTypeNameAr = s.InvoicePaymentsMethods.Select(a => a.PaymentMethod).Count() > 1 ? " اخرى " : s.InvoicePaymentsMethods.Select(a => a.PaymentMethod).Count() == 1 ? s.InvoicePaymentsMethods.Select(a => a.PaymentMethod).Select(a => a.ArabicName).FirstOrDefault() : "اجل",
                                   //PaymentTypeNameEn = s.InvoicePaymentsMethods.Select(a => a.PaymentMethod).Count() > 1 ? " اخرى " : s.InvoicePaymentsMethods.Select(a => a.PaymentMethod).Count() == 1 ? s.InvoicePaymentsMethods.Select(a => a.PaymentMethod).Select(a => a.LatinName).FirstOrDefault() : "اجل",
                                   Net = roundNumbers.GetRoundNumber(s.Net - s.TotalVat),
                                   CustomerNameAr = s.Person.ArabicName,
                                   CustomerNameEn = s.Person.LatinName,
                                   InvoiceDate = s.InvoiceDate.ToString("yyyy-MM-dd"),
                                   Cost = roundNumbers.GetRoundNumber((s.InvoicesDetails.Where(h => h.parentItemId == null || h.parentItemId == 0)
                                            .Sum(a => (a.Cost * a.Quantity * a.ConversionFactor)))),
                               });//.Skip(isPrint?0:(Parameter.PageNumber - 1) * Parameter.PageSize ).TakeIfNotNull(Parameter.PageSize);

              
                var paymentTypes = Lists.paymentTypes;
              
                var finalData = data.ToList();
                finalData.Select(a => a.Profit = (a.DocumenTypeID == (int)DocumentType.ReturnPOS || a.DocumenTypeID == (int)DocumentType.ReturnSales) ?( roundNumbers.GetRoundNumber((a.Net - a.Cost)))*-1: (roundNumbers.GetRoundNumber((a.Net - a.Cost)))).ToList();
                //finalData.Select(a => a.Profit = (roundNumbers.GetRoundNumber((a.Net - a.Cost)))).ToList();
                finalData.Select(a => a.Net = (a.DocumenTypeID == (int)DocumentType.ReturnPOS || a.DocumenTypeID == (int)DocumentType.ReturnSales) ? a.Net * -1 : a.Net) .ToList();
                finalData.Select(a => a.Cost = (a.DocumenTypeID == (int)DocumentType.ReturnPOS || a.DocumenTypeID == (int)DocumentType.ReturnSales) ? a.Cost * -1 : a.Cost) .ToList();
                finalData.Select(a => a.DocumenTypeAr = listOfInvoicesNames.listOfNames().SingleOrDefault(h => h.invoiceTypeId == a.DocumenTypeID).NameAr).ToList();
                finalData.Select(a => a.DocumenTypeEn = listOfInvoicesNames.listOfNames().SingleOrDefault(h => h.invoiceTypeId == a.DocumenTypeID).NameEn).ToList();
                finalData.Select(a => a.PaymentTypeNameAr = paymentTypes.Where(c => c.id == a.paymentTypeId).First().arabicName).ToList();
                finalData.Select(a => a.PaymentTypeNameEn = paymentTypes.Where(c => c.id == a.paymentTypeId).First().latinName).ToList();
                TotalRPT_SalesProfitResponse finalDataWithTotals = new TotalRPT_SalesProfitResponse()
                {
                    TotalCost = roundNumbers.GetRoundNumber( finalData.Sum(a => a.Cost)),
                    TotalNet = roundNumbers.GetRoundNumber(finalData.Sum((a) => a.Net)),
                    TotalProfit = roundNumbers.GetRoundNumber(finalData.Sum(a => a.Profit)),
                    _salesProfitResponseList =isPrint? finalData : Pagenation<RPT_SalesProfitResponse>.pagenationList(Parameter.PageSize, Parameter.PageNumber, finalData),
                };

                //var query = data.ToQueryString();
                //int dataCount = finalData.Count;
                //var theRestOfData = dataCount - (Parameter.PageNumber * Parameter.PageSize);
                return new ResponseResult() { Data = finalDataWithTotals, Result = Result.Success, DataCount = data.Count()};

               ///

     

            }
            catch (Exception E)
            {

                return new ResponseResult() { Result = Result.Failed };

            }

        }
        public async Task<WebReport> SalesProfitReport(RPT_SalesProfitRequest Parameter, exportType exportType,bool isArabic,int fileId=0)
        {
            var data = await GetSalesProfit(Parameter, true);
            var mainData = (TotalRPT_SalesProfitResponse)data.Data;

            var userInfo = await _iUserInformation.GetUserInformation();


            var otherdata = ArabicEnglishDate.OtherDataWithDatesArEn(isArabic, Parameter.DateFrom, Parameter.DateTo);


            otherdata.EmployeeName = userInfo.employeeNameAr.ToString();
            otherdata.EmployeeNameEn = userInfo.employeeNameEn.ToString();

            var tablesNames = new TablesNames()
            {

                ObjectName = "SalesProfitResponse",
                FirstListName = "SalesProfitResponseList"
            };
            
            var report = await _iGeneralPrint.PrintReport<TotalRPT_SalesProfitResponse, RPT_SalesProfitResponse, object>(mainData, mainData._salesProfitResponseList, null, tablesNames, otherdata
             , (int)SubFormsIds.SalesBranchProfit, exportType, isArabic,fileId);
            return report;
        }



    }
}
