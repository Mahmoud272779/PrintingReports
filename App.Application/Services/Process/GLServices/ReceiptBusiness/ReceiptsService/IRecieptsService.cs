using App.Domain.Models.Shared;
using App.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Models.Security.Authentication.Response;
using App.Domain.Models.Response.General;
using App.Domain.Enums;
using FastReport.Web;
using App.Application.Handlers.Persons;
using App.Domain.Models.Security.Authentication.Response.Store;

namespace App.Application.Services.Process.GLServices.ReceiptBusiness
{
    public  interface IReceiptsService
    {
        int Autocode(int RecieptTypeId, int BranchId);
        Task<ResponseResult> AddReceipt(RecieptsRequest parameter);
        Task<ResponseResult> GetReceiptById(int ReceiptId, int RecieptsType, bool isPrint=false);
        Task<ResponseResult> GetAllReceipts(GetRecieptsData parameter);
        Task<ResponseResult> UpdateReceipt(UpdateRecieptsRequest parameter);
       Task<ResponseResult> GetReceiptsAuthortyDropDown();
        Task<ResponseResult> DeleteReciepts(List<int?> Ids);
        Task<ResponseResult> GetReceiptCurrentFinancialBalance(int AuthorityId, int BenefitID);
        Task<ResponseResult> GetReceiptBalanceForBenifit(int AuthorityId, int BenefitID);
        Task<WebReport> ReceiptPrint(int ReceiptId, int RecieptsType, exportType exportType, bool isArabic, int fileId = 0);
        public Tuple<string, string, string, string> SetRecieptTypeAndDirectoryAndNotes(int recieptTypeId, int? parentTypeId);

    }
}
