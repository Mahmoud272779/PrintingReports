﻿using App.Application.Helpers;
using App.Application.Services.Process.Invoices.Purchase;
using App.Domain.Entities.Process;
using App.Domain.Models.Security.Authentication.Response;
using App.Domain.Models.Security.Authentication.Response.PurchasesDtos;
using App.Domain.Models.Shared;
using App.Infrastructure.Helpers;
using App.Infrastructure.Interfaces.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Application.Helpers.Aliases;
using static App.Domain.Enums.Enums;

namespace App.Application.Services.Process.StoreServices.Invoices.HistoryOfInvoices
{
    public class HistoryInvoiceService : BaseClass, IHistoryInvoiceService
    {
        private readonly IRepositoryCommand<InvoiceMasterHistory> InvoiceMasterHistoryRepositoryCommand;
        private readonly IRepositoryQuery<InvoiceMasterHistory> InvoiceMasterHistoryRepositoryQuery;
        private readonly iUserInformation _iUserInformation;
        private readonly IGetInvoiceByIdService _iGetInvoiceByIdService; 
         
        private readonly IHttpContextAccessor httpContext;
        private readonly IGetTempInvoiceByIdService _iGetOfferPriceByIdservice;

        public HistoryInvoiceService(
                              IRepositoryCommand<InvoiceMasterHistory> _InvoiceMasterHistoryRepositoryCommand,
                              IRepositoryQuery<InvoiceMasterHistory> _InvoiceMasterHistoryRepositoryQuery,
                              IHttpContextAccessor _httpContext,
                              iUserInformation iUserInformation, IGetInvoiceByIdService iGetInvoiceByIdService, IGetTempInvoiceByIdService iGetOfferPriceByIdservice) : base(_httpContext)
        {
            InvoiceMasterHistoryRepositoryCommand = _InvoiceMasterHistoryRepositoryCommand;
            InvoiceMasterHistoryRepositoryQuery = _InvoiceMasterHistoryRepositoryQuery;
            httpContext = _httpContext;
            _iUserInformation = iUserInformation;
            _iGetInvoiceByIdService = iGetInvoiceByIdService;
            _iGetOfferPriceByIdservice = iGetOfferPriceByIdservice;
        }
        public async Task HistoryInvoiceMaster(int branchId, string notes, string browserName, string lastTransactionAction, string addTransactionUser, string BookIndex, int Code
, DateTime InvoiceDate, int InvoiceId, string InvoiceType, int InvoiceTypeId , int SubType, bool IsDeleted, string ParentInvoiceCode, double Serialize, int StoreId, double TotalPrice,int collectionMainCode=0)

        {
            var userInfo = await _iUserInformation.GetUserInformation();
            var history = new InvoiceMasterHistory()
            {
                employeesId=userInfo.employeeId,
                LastDate = DateTime.Now,
                LastAction = lastTransactionAction,
                LastTransactionAction = lastTransactionAction,
                AddTransactionUser = addTransactionUser,
                LastTransactionUser = userInfo.employeeNameEn.ToString(),
                LastTransactionUserAr = userInfo.employeeNameAr.ToString(),
                BranchId = branchId,
                CollectionMainCode=collectionMainCode,
                Notes = notes,
                BookIndex = BookIndex,
                Code = Code,
                InvoiceDate = InvoiceDate,
                InvoiceId = InvoiceId,
                InvoiceType = InvoiceType,
                InvoiceTypeId = InvoiceTypeId,
                SubType = SubType,
                IsDeleted = IsDeleted,
                ParentInvoiceCode = ParentInvoiceCode,
                Serialize = Serialize,
                StoreId = StoreId,
                TotalPrice = TotalPrice,
                BrowserName = httpContext.HttpContext.Request.Headers[HeaderNames.UserAgent].ToString().Contains("Dart")?"APK": httpContext.HttpContext.Request.Headers[HeaderNames.UserAgent].ToString(),
               // employees=userInfo.em
               isTechnicalSupport = userInfo.isTechincalSupport
            };
            try
            {
var res=InvoiceMasterHistoryRepositoryCommand.Add(history);
            }
            catch (Exception e)
            {

                var x = e.Message;
            }
          //  history.employees = userInfo;
            
           // await InvoiceMasterHistoryRepositoryCommand.SaveAsync();
        }
        public async Task<ResponseResult> GetAllInvoiceMasterHistory(int InvoiceId , string? ParentInvoiceCode , int invoiceTypeId)
        {
            var userInfo = await _iUserInformation.GetUserInformation();
            if(!userInfo.otherSettings.showHistory)
                return new ResponseResult()
                {
                    Note = Actions.YouCantViewTheHistory,
                    Result = Result.Failed
                };
            var invoice = new InvoiceDto();
                
            
            //IQueryable<InvoiceMasterHistory> parentDatasQuey;
            //if (!invoice.CanDeleted)
            //{
            //    parentDatasQuey = InvoiceMasterHistoryRepositoryQuery.FindQueryable(s => s.InvoiceId == InvoiceId  ||
            //  (Lists.returnInvoiceList.Contains(invoiceTypeId) ?
            // 1 != 1 : s.ParentInvoiceCode == ParentInvoiceCode));
            //}
            //else

           //var parentDatasQuey = InvoiceMasterHistoryRepositoryQuery.FindQueryable(s => s.InvoiceId == InvoiceId ||
           //  (Lists.returnInvoiceList.Contains(invoiceTypeId) ?
           // 1 != 1 : s.ParentInvoiceCode == ParentInvoiceCode)).Include(a => a.employees);
            bool orderPurchaseOrOfferPrice = Lists.orderPurchaseAndOfferPrice.Contains(invoiceTypeId);
            var parentDatasQuey = InvoiceMasterHistoryRepositoryQuery.FindQueryable(s =>
            (orderPurchaseOrOfferPrice ? Lists.orderPurchaseAndOfferPrice.Contains(s.InvoiceTypeId)
                     : !Lists.orderPurchaseAndOfferPrice.Contains(s.InvoiceTypeId))
             && ((Lists.returnInvoiceList.Contains(invoiceTypeId) ?
              1 != 1 : s.ParentInvoiceCode == ParentInvoiceCode)
              || s.InvoiceId == InvoiceId)).Include(a => a.employees);

            var historyList = new List<HistoryResponceDto>();
           // var invoiceType = "";
           if(orderPurchaseOrOfferPrice)
            {
                invoice = await _iGetOfferPriceByIdservice.GetInvoiceDto(InvoiceId, false);

            }
            else
                invoice= await _iGetInvoiceByIdService.GetInvoiceDto(InvoiceId, false);
            foreach (var item in parentDatasQuey.ToList())
            {
                var historyDto = new HistoryResponceDto();
                historyDto.Date = item.LastDate.Value;

                //if (item.LastAction == "U")
                //{
                //    historyDto.TransactionTypeAr = "تعديل";
                //}
                //else if (item.LastAction == "A")
                //{
                //    historyDto.TransactionTypeAr = "اضافة";
                //}
                //else if (item.LastAction == "D")
                //{
                //    historyDto.TransactionTypeAr = "حذف";
                //}

                HistoryActionsNames actionName = HistoryActionsAliasNames.HistoryName[item.LastAction];
                historyDto.TransactionTypeEn=actionName.LatinName;
                historyDto.TransactionTypeAr =actionName.ArabicName;
                if (item.SubType == (int)SubType.CollectionReceipt)
                {
                    historyDto.TransactionTypeAr +=  " " + NotesOfReciepts.CollectionReceiptsAr +"( بقيمة : "+ item.TotalPrice+')';
                    historyDto.TransactionTypeEn += " " + NotesOfReciepts.CollectionReceiptsEn + "( Value : " + item.TotalPrice + ')';
                }
                if (item.SubType == (int)SubType.PaidReceipt)
                {
                    historyDto.TransactionTypeAr += " " + NotesOfReciepts.PaidReciptsAr + "( بقيمة : " + item.TotalPrice + ')';
                    historyDto.TransactionTypeEn += " " + NotesOfReciepts.PaidReciptsEn + "( Value : " + item.TotalPrice + ')';
                }
                if (Lists.returnInvoiceList.Contains(item.InvoiceTypeId))
                {
                    if (item.SubType == (int)SubType.PartialReturn)
                    {
                        historyDto.TransactionTypeAr = "مرتجع جزئي";
                        historyDto.TransactionTypeEn = "Partial Return";
                    }
                        

                    else if (item.SubType == (int)SubType.TotalReturn)
                    {
                        historyDto.TransactionTypeAr = "مرتجع كلى";
                        historyDto.TransactionTypeEn = "Total Return";

                    }
                }

                if (item.isTechnicalSupport)
                {
                    historyDto.ArabicName = HistoryActions.TechnicalSupportAr;
                    historyDto.LatinName = HistoryActions.TechnicalSupportEn;
                }
                else
                {
                    historyDto.LatinName = item.employees.LatinName;
                    historyDto.ArabicName = item.employees.ArabicName;
                }
              
                historyDto.BrowserName = item.BrowserName;
                if (item.BrowserName.Contains("Chrome"))
                {
                    historyDto.BrowserName = "Chrome";
                }
               else if (item.BrowserName.Contains("Firefox")|| item.BrowserName.Contains("Mozilla"))
                {
                    historyDto.BrowserName = "Firefox";
                }
               else if (item.BrowserName.Contains("Opera"))
                {
                    historyDto.BrowserName = "Opera";
                }
                else if (item.BrowserName.Contains("InternetExplorer")) 
                {
                    historyDto.BrowserName = "InternetExplorer";
                }
                else if (item.BrowserName.Contains("Microsoft Edge"))
                {
                    historyDto.BrowserName = "Microsoft Edge";
                }
                
                historyList.Add(historyDto);
                //invoiceType = item.InvoiceType;
            }
            return new ResponseResult() { Data = historyList, Id = null, Result = Result.Success, Note = invoice.InvoiceType };
        }


    }
}
