using App.Api.Controllers.BaseController;
using App.Application.Services.HelperService.authorizationServices;
using App.Application.Services.Printing.PrintFile;
using App.Application.Services.Process.Employee;
using App.Application.Services.Process.FundsCustomerSupplier;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Security.Authentication.Request;
using App.Domain.Models.Shared;
using Attendleave.Erp.Core.APIUtilities;
using FastReport.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process
{
    public class FundsCustomersAndSuppliersController : ApiStoreControllerBase
    {
        private readonly IFundsCustomerSupplierService FundsCustomerSupplierService;
        private readonly iAuthorizationService _iAuthorizationService;
        private readonly IprintFileService _printFileService;

        public FundsCustomersAndSuppliersController(IFundsCustomerSupplierService _FundsCustomerSupplierService, iAuthorizationService iAuthorizationService,
                        IActionResultResponseHandler ResponseHandler, IprintFileService printFileService) : base(ResponseHandler)
        {
            FundsCustomerSupplierService = _FundsCustomerSupplierService;
            _iAuthorizationService = iAuthorizationService;
            _printFileService = printFileService;
        }

        [HttpGet("GetListOfFundsCustomers")]

        public async Task<ResponseResult> GetListOfFundsCustomers([FromQuery]FundsCustomerandSupplierSearch parameters)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized((int)MainFormsIds.ItemsFund, (int)SubFormsIds.Customres_Fund, Opretion.Open);
            if (isAuthorized != null)
                return isAuthorized;
            var add = await FundsCustomerSupplierService.GetListOfFundsCustomer(parameters);
            return add;
        }
        
        [HttpGet("GetListOfFundsSuppliers")]

        public async Task<ResponseResult> GetListOfFundsSuppliers([FromQuery] FundsCustomerandSupplierSearch parameters)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized((int)MainFormsIds.ItemsFund, (int)SubFormsIds.Suppliers_Fund, Opretion.Open);
            if (isAuthorized != null)
                return isAuthorized;
            var add = await FundsCustomerSupplierService.GetListOfFundsSuppliers(parameters);
            return add;
        }
        [HttpGet("SuppliersCustomersFundsReport")]

        public async Task<IActionResult> SuppliersCustomersFundsReport([FromQuery] FundsCustomerandSupplierSearch parameters,bool isSupplier,string? ids,bool isSearchData,exportType exportType,bool isArabic,int fileId=0)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized((int)MainFormsIds.ItemsFund, (int)SubFormsIds.Suppliers_Fund, Opretion.Print);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            WebReport report = new WebReport();
            report = await FundsCustomerSupplierService.PersonsReport(parameters, isSupplier, ids,isSearchData, exportType, isArabic,fileId);
            string reportName = "";
            if (isSupplier)
            {
                reportName = "Supplier-Funds-Report";
            }
            else
                reportName = "Customer-Funds-Report";

            return Ok(_printFileService.PrintFile(report, reportName, exportType));
        }

        [HttpPut(nameof(UpdateFundsSuppliers))]
        public async Task<ResponseResult> UpdateFundsSuppliers(FundsCustomerandSupplierParameter parameters)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized((int)MainFormsIds.ItemsFund, (int)SubFormsIds.Suppliers_Fund, Opretion.Edit);
            if (isAuthorized != null)
                return isAuthorized;
            var add = await FundsCustomerSupplierService.UpdateFundsSuppliersAndCustomers(parameters,false);
            return add;
        }[HttpPut(nameof(UpdateFundsCustomers))]
        public async Task<ResponseResult> UpdateFundsCustomers(FundsCustomerandSupplierParameter parameters)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized((int)MainFormsIds.ItemsFund, (int)SubFormsIds.Customres_Fund, Opretion.Edit);
            if (isAuthorized != null)
                return isAuthorized;
            var add = await FundsCustomerSupplierService.UpdateFundsSuppliersAndCustomers(parameters,true);
            return add;
        }
        
        [HttpGet("GetFundsById")]
        public async Task<ResponseResult> GetFundsById(int Id)
        {
            var add = await FundsCustomerSupplierService.GetFundsById(Id);
            return add;

        }
    }
}
