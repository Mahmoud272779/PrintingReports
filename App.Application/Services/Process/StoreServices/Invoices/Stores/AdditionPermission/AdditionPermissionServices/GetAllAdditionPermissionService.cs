﻿using App.Application.Helpers;
using App.Application.Services.Process.Invoices.General_Process;
using App.Application.Services.Process.StoreServices.Invoices.AdditionPermission.IAdditionPermissionServices;
using App.Domain.Entities.Process;
using App.Domain.Models.Security.Authentication.Response.PurchasesDtos;
using App.Domain.Models.Security.Authentication.Response.Store.Invoices;
using App.Domain.Models.Shared;
using App.Infrastructure.Helpers;
using App.Infrastructure.Interfaces.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Domain.Enums.Enums;

namespace App.Application.Services.Process.StoreServices.Invoices.AdditionPermission.AdditionPermissionServices
{
    public class GetAllAdditionPermissionService : BaseClass, IGetAllAdditionPermissionService
    {
        private readonly IRepositoryQuery<InvoiceMaster> InvoiceMasterRepositoryQuery;
        private readonly IHttpContextAccessor httpContext;
        private readonly IGetAllInvoicesService GetAllInvoicesService;
        private readonly iUserInformation Userinformation;
        public GetAllAdditionPermissionService(IRepositoryQuery<InvoiceMaster> _InvoiceMasterRepositoryQuery,
            IGetAllInvoicesService _GetAllInvoicesService, iUserInformation Userinformation,
            IHttpContextAccessor _httpContext) : base(_httpContext)
        {
            InvoiceMasterRepositoryQuery = _InvoiceMasterRepositoryQuery;
            GetAllInvoicesService = _GetAllInvoicesService;
            httpContext = _httpContext;
            this.Userinformation = Userinformation;
        }
        public async Task<ResponseResult> GetAllAdditionPermission(StoreSearchPagination parameter)
        {
            var searchCretiera = parameter.Searches.SearchCriteria;
            UserInformationModel userInfo = await Userinformation.GetUserInformation();

            var treeData = InvoiceMasterRepositoryQuery.TableNoTracking
                          .Include(x => x.store).Where(q => q.BranchId == userInfo.CurrentbranchId &&
                          ((q.InvoiceTypeId == (int)DocumentType.AddPermission && q.IsDeleted == false) ||
                            q.InvoiceTypeId == (int)DocumentType.DeleteAddPermission));


            var totalCount = treeData.Count();
            if (string.IsNullOrEmpty(searchCretiera))
            {
                treeData = treeData.OrderByDescending(x => x.Code);
            }
            else
            {
                treeData = treeData.OrderBy(x => x.Code.ToString().Contains(searchCretiera));
            }

            if (!string.IsNullOrEmpty(searchCretiera))
            {
                treeData = treeData.Where
                                     (x =>
                                             x.Code.ToString().Contains(searchCretiera) ||
                                             x.InvoiceType == searchCretiera ||
                                             x.BookIndex.Contains(searchCretiera)
                                     );
            }


            if (parameter.Searches != null)
            {
                if (parameter.Searches.InvoiceTypeId.Any())
                {
                    treeData = treeData.Where(q => parameter.Searches.InvoiceTypeId.Contains(q.InvoiceTypeId));
                }
                if (parameter.Searches.StoreId.Count() > 0)
                {
                    treeData = treeData.Where(q => parameter.Searches.StoreId.Contains(q.StoreId));
                }
                if (parameter.Searches.InvoiceDateFrom != null)
                    treeData = treeData.Where(q => q.InvoiceDate >= parameter.Searches.InvoiceDateFrom.Value.Date);
                if (parameter.Searches.InvoiceDateTo != null)
                    treeData = treeData.Where(q => q.InvoiceDate <= parameter.Searches.InvoiceDateTo.Value);
            }
            var count = treeData.Count();
            List<InvoiceMaster> response = new List<InvoiceMaster>();
            var list2 = new List<AllInvoiceDto>();

            if (parameter.PageSize > 0 && parameter.PageNumber > 0)
            {
                response = treeData.Skip((parameter.PageNumber - 1) * parameter.PageSize).Take(parameter.PageSize).ToList();
            }
            else
            {
                return new ResponseResult() { Data = null, DataCount = 0, Id = null, Result = Result.Failed };

            }

            GetAllInvoicesService.GetAllInvoices(response, list2);
            return new ResponseResult() { Id = null, Data = list2, DataCount = count, Result = list2.Count > 0 ? Result.Success : Result.NoDataFound, Note = "", TotalCount = totalCount };

            /* var searchCretiera = parameter.Searches.SearchCriteria;
             UserInformationModel userInfo = Userinformation.GetUserInformation();
             var DataFromDb = InvoiceMasterRepositoryQuery.TableNoTracking.Where(a => a.BranchId == userInfo.CurrentbranchId).ToList().Count();
             if (DataFromDb == 0)
                 return new ResponseResult() { Data = null, DataCount = 0, Id = null, Result = Result.Success };

             var treeData = await InvoiceMasterRepositoryQuery.GetAllIncludingAsync(0, 0,
                    q => ((q.InvoiceTypeId == (int)DocumentType.AddPermission  && q.IsDeleted == false) ||
                    q.InvoiceTypeId == (int)DocumentType.DeleteAddPermission )&&q.BranchId== userInfo.CurrentbranchId
                    && (searchCretiera == string.Empty || searchCretiera == null || q.InvoiceType == searchCretiera||
                    q.Code.ToString().Contains(searchCretiera) || 
               q.BookIndex.Contains(searchCretiera)),
               e => (string.IsNullOrEmpty(searchCretiera) ? e.OrderByDescending(q => q.Code) : e.OrderBy(a => (a.Code.ToString().Contains(searchCretiera)) ? 0 : 1))
               , a => a.store, a => a.InvoicesDetails);
             var list = treeData.ToList();

             if (parameter.Searches != null)
             {

                 if (parameter.Searches.StoreId.Count() > 0)
                 {
                     list = list.Where(q => parameter.Searches.StoreId.Contains(q.StoreId)).ToList();
                 }
                 if (parameter.Searches.InvoiceTypeId.Count() > 0)
                 {
                     list = list.Where(q => parameter.Searches.InvoiceTypeId.Contains(q.InvoiceTypeId)).ToList();
                 }
                
            if (parameter.Searches.InvoiceDateFrom != null)
                    list = list.Where(q => q.InvoiceDate >= parameter.Searches.InvoiceDateFrom.Value.Date).ToList();
                if (parameter.Searches.InvoiceDateTo != null)
                    list = list.Where(q => q.InvoiceDate <= parameter.Searches.InvoiceDateTo.Value.Date).ToList();
            }

        

            var count = list.Count();
            var list2 = new List<AllInvoiceDto>();

            if (parameter.PageSize > 0 && parameter.PageNumber > 0)
            {
                list = list.Skip((parameter.PageNumber - 1) * parameter.PageSize).Take(parameter.PageSize).ToList();
            }
            else
            {
                return new ResponseResult() { Data = null, DataCount = 0, Id = null, Result = Result.Failed };

            }
            var totalCount = InvoiceMasterRepositoryQuery.TableNoTracking.Where(a=> Lists.AddPermissionInvoicesList.Contains(a.InvoiceTypeId)).Count();
            GetAllInvoicesService.GetAllInvoices(list, list2);
            return new ResponseResult() { Id = null, Data = list2, DataCount = count, Result = list2.Count > 0 ? Result.Success : Result.NoDataFound, Note = "", TotalCount = totalCount };*/


        }

    }
}
