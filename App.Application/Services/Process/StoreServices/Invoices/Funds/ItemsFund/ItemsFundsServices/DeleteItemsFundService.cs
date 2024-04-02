﻿using App.Application.Helpers;
using App.Application.Services.Process.GeneralServices.SystemHistoryLogsServices;
using App.Application.Services.Process.Invoices.General_Process;
using App.Application.Services.Process.StoreServices.Invoices.Funds.ItemsFund.ItemFundGLIntegrationServices;
using App.Application.Services.Process.StoreServices.Invoices.ItemsFund.IItemsFundsServices;
using App.Domain.Entities.Process;
using App.Domain.Enums;
using App.Domain.Models.Security.Authentication.Request;
using App.Domain.Models.Shared;
using App.Infrastructure.Helpers;
using App.Infrastructure.Interfaces.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Domain.Enums.Enums;

namespace App.Application.Services.Process.StoreServices.Invoices.ItemsFund.ItemsFundsServices
{
    public class DeleteItemsFundService : BaseClass, IDeleteItemsFundService
    {
        private readonly IRepositoryQuery<InvoiceMaster> InvoiceMasterRepositoryQuery;
        private readonly IRepositoryCommand<InvoiceMaster> InvoiceMasterRepositoryCommand;

        private readonly IRepositoryQuery<InvGeneralSettings> InvGeneralSettingsRepositoryQuery;
        private readonly IDeleteInvoice GeneralProcessDelete;
        private readonly iItemFundGLIntegrationService _iItemFundGLIntegrationService;
        private readonly ISystemHistoryLogsService _systemHistoryLogsService;
        private readonly IHttpContextAccessor httpContext;

        public DeleteItemsFundService(IRepositoryQuery<InvoiceMaster> _InvoiceMasterRepositoryQuery,
                              IRepositoryCommand<InvoiceMaster> _InvoiceMasterRepositoryCommand,
                              IRepositoryQuery<InvGeneralSettings> _InvGeneralSettingsRepositoryQuery,
                              IDeleteInvoice generalProcessDelete,
                              iItemFundGLIntegrationService iItemFundGLIntegrationService,
                              ISystemHistoryLogsService systemHistoryLogsService,
                              IHttpContextAccessor _httpContext) : base(_httpContext)
        {
            InvoiceMasterRepositoryQuery = _InvoiceMasterRepositoryQuery;
            InvoiceMasterRepositoryCommand = _InvoiceMasterRepositoryCommand;
            httpContext = _httpContext;
            GeneralProcessDelete = generalProcessDelete;
            _iItemFundGLIntegrationService = iItemFundGLIntegrationService;
            _systemHistoryLogsService = systemHistoryLogsService;
            InvGeneralSettingsRepositoryQuery = _InvGeneralSettingsRepositoryQuery;
        }
        public async Task<ResponseResult> DeleteItemsFund(SharedRequestDTOs.Delete parameter)
        {

          /*  var invoicesMaster = InvoiceMasterRepositoryQuery.TableNoTracking
                        .Where(e => parameter.Ids.Contains(e.InvoiceId)).ToList();

            // update old invoice , change isdelete = true
            invoicesMaster.Select(e => { e.IsDeleted = true; return e; }).ToList();
            var updateOldMaster = await InvoiceMasterRepositoryCommand.UpdateAsyn(invoicesMaster);

            var setting = await InvGeneralSettingsRepositoryQuery.GetByAsync(q => 1 == 1);*/
           var res= await GeneralProcessDelete.DeleteInvoices(parameter.Ids.First(), (int)DocumentType.DeleteItemsFund, Aliases.InvoicesCode.DeleteItemsFund, (int)DocumentType.itemsFund);

            foreach (var item in parameter.Ids)
            {
                await _iItemFundGLIntegrationService.AddItemFundJournalEntry(new ItemFundJournalEntryDTO()
                {
                    documentId = item,
                    isDelete = true,
                });
            }
            await _systemHistoryLogsService.SystemHistoryLogsService(SystemActionEnum.deleteItemsEntryFund);
            return res;// new ResponseResult() { Data = null, Id = null, Result = Result.Success };
        }
    }
}