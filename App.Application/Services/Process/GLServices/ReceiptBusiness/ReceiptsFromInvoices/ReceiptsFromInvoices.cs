
using App.Domain.Entities.Process;
using App.Domain.Models.Shared;
using App.Infrastructure.Helpers;
using App.Infrastructure.Interfaces.Repository;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using App.Application.Helpers;
using static App.Domain.Enums.Enums;
using System.Collections.Generic;
using App.Domain;
using App.Application.Services.Process.GLServices.ReceiptBusiness;
using App.Domain.Entities;

namespace App.Application.Services.Process.StoreServices.Invoices.AccrediteInvoice
{
    public class ReceiptsFromInvoices : BaseClass, IReceiptsFromInvoices
    {


        private readonly IRepositoryQuery<InvPaymentMethods> PaymentsMethodsQuery;

        private readonly IRepositoryCommand<GlReciepts> ReceiptCommand;
        private readonly IRepositoryQuery<GlReciepts> ReceiptQuery;
        private readonly IRepositoryCommand<GLJournalEntry> JournalenteryCommand;
        private readonly IRepositoryCommand<GLJournalEntryDetails> JournalenteryDetailCommand;
        private readonly IRepositoryQuery<GLJournalEntry> JournalenteryQuery;

        private readonly IReceiptsService ReceiptsServices;


        public ReceiptsFromInvoices(IRepositoryQuery<InvoiceMaster> _InvoiceMasterRepositoryQuery,


                          IReceiptsService receiptsServices,
                              IRepositoryQuery<GLJournalEntry> journalenteryQuery,
                              IRepositoryCommand<GLJournalEntry> journalenteryCommand,
                              IRepositoryCommand<GLJournalEntryDetails> journalenteryDetailCommand,
                              IRepositoryQuery<GlReciepts> receiptQuery,


                              IRepositoryCommand<GlReciepts> receiptCommand,
           

                              IRepositoryQuery<InvPaymentMethods> paymentsMethodsQuery,
        IHttpContextAccessor _httpContext) : base(_httpContext)
        {

            ReceiptsServices = receiptsServices;
            ReceiptCommand = receiptCommand;
            ReceiptQuery = receiptQuery;
            JournalenteryCommand = journalenteryCommand;
            JournalenteryQuery = journalenteryQuery;

           
          
            PaymentsMethodsQuery = paymentsMethodsQuery;
            
            JournalenteryDetailCommand = journalenteryDetailCommand;
        }

        public async Task<ResponseResult> updateReceiptsFromInvoices(InvoiceMaster Invoice, List<InvoicePaymentsMethods>? InvPaymentMethod)
        {

            var delInvoice = await DeleteInvoicesReceipts(new List<int>() { Invoice.InvoiceId });
            if (delInvoice.Result == Result.Success)
                return await AddReceiptsFromInvoices(Invoice, InvPaymentMethod);

            return new ResponseResult() { Result = Result.Failed };

        }

        private int GetReceiptsType(int? safeID, int? bankId, int? ParentTypeId)
        {
            if (ParentTypeId == null)
                return 0;
            if (ParentTypeId == (int)DocumentType.Purchase || ParentTypeId == (int)DocumentType.ReturnSales || ParentTypeId == (int)DocumentType.ReturnPOS || ParentTypeId == (int)DocumentType.wov_purchase)
            {
                if (safeID != null && safeID > 0)//recepttype = سند صرف خزينه
                    return (int)DocumentType.SafePayment;
                else if (bankId != null && bankId > 0)
                    return (int)DocumentType.BankPayment;

            }
            if (ParentTypeId == (int)DocumentType.ReturnPurchase || ParentTypeId == (int)DocumentType.Sales || ParentTypeId == (int)DocumentType.POS|| ParentTypeId==(int)DocumentType.ReturnWov_purchase)
            {
                if (safeID != null && safeID > 0)//recepttype = سند قبض خزينه
                    return (int)DocumentType.SafeCash;
                else if (bankId != null && bankId > 0)
                    return (int)DocumentType.BankCash;
            }
            return 0;
        }

        public async Task<ResponseResult> AddReceiptsFromInvoices(InvoiceMaster Invoice, List<InvoicePaymentsMethods> InvPaymentMethod)
        {
            int authorty = 0;
            ResponseResult result = new ResponseResult();
            if (Lists.purchasesInvoicesList.Contains(Invoice.InvoiceTypeId) ||Lists.purchasesWithoutVatInvoicesList.Contains(Invoice.InvoiceTypeId))
                authorty = AuthorityTypes.suppliers;
            else if (Lists.salesInvoicesList.Contains(Invoice.InvoiceTypeId) || Lists.POSInvoicesList.Contains(Invoice.InvoiceTypeId))
                authorty = AuthorityTypes.customers;
            if (InvPaymentMethod != null)
            {
                var safeOrBanks = PaymentsMethodsQuery.TableNoTracking
                    .Where(h => (InvPaymentMethod.Select(a => a.PaymentMethodId)).Contains(h.PaymentMethodId)).ToList();

                foreach (var item in safeOrBanks)
                {
                    var paymentmethod = InvPaymentMethod.Where(h => h.PaymentMethodId == item.PaymentMethodId).FirstOrDefault();
                    RecieptsRequest ReceiptsData = new RecieptsRequest()
                    {
                        SafeID = item.SafeId,
                        BankId = item.BankId,
                        Amount = Invoice.Net < paymentmethod.Value ?Invoice.Net : paymentmethod.Value,
                        RecieptDate = Invoice.InvoiceDate,
                        PaymentMethodId = item.PaymentMethodId,
                        ChequeBankName = "",
                        ChequeNumber = paymentmethod.Cheque,
                        ChequeDate = Invoice.InvoiceDate,
                        ParentType = Invoice.InvoiceType,
                        ParentTypeId = Invoice.InvoiceTypeId,
                        Creditor = Invoice.Net < paymentmethod.Value ? Invoice.Net : paymentmethod.Value,
                        Debtor = Invoice.Net < paymentmethod.Value ? Invoice.Net : paymentmethod.Value,
                        RecieptTypeId = GetReceiptsType(item.SafeId, item.BankId, Invoice.InvoiceTypeId),
                        IsAccredit = false,
                        ParentId = Invoice.InvoiceId,
                        Serialize = Invoice.Serialize,
                        Code = Invoice.Code,
                        BenefitId = Invoice.PersonId.Value,
                        Authority = authorty,
                        ReceiptOnly = true,
                        fromInvoice = true,

                        isPartialPaid = Invoice.PaymentType,
                        Notes=Invoice.Notes

                    };
                    //if (Lists.purchasesInvoicesList.Contains(ReceiptsData.ParentTypeId.Value))
                    //    ReceiptsData.Authority = AuthorityTypes.suppliers;
                    //else if (Lists.salesInvoicesList.Contains(ReceiptsData.ParentTypeId.Value))
                    //    ReceiptsData.Authority = AuthorityTypes.customers;

                    result = await ReceiptsServices.AddReceipt(ReceiptsData);

                }
            }
            //
            if (Invoice.Remain > 0)
            {
                RecieptsRequest DeferreReceiptsData = new RecieptsRequest()
                {
                    SafeID = 1,
                    BankId = null,
                    Amount = Invoice.Remain,
                    RecieptDate = Invoice.InvoiceDate,
                    PaymentMethodId = 1,
                    ChequeBankName = "",
                    ChequeNumber = "",
                    ChequeDate = Invoice.InvoiceDate,
                    ParentType = Invoice.InvoiceType,
                    ParentTypeId = Invoice.InvoiceTypeId,
                    Creditor = Invoice.Remain,
                    Debtor = Invoice.Remain,
                    RecieptTypeId = GetReceiptsType(1, null, Invoice.InvoiceTypeId),
                    IsAccredit = false,
                    ParentId = Invoice.InvoiceId,
                    Serialize = Invoice.Serialize,
                    Code = Invoice.Code,
                    BenefitId = Invoice.PersonId.Value,
                    Deferre = true
                    , fromInvoice = true
                    , Authority = authorty,
                    ReceiptOnly = true,
                    Notes=Invoice.Notes

                };
                //

                result = await ReceiptsServices.AddReceipt(DeferreReceiptsData);
            }
            return new ResponseResult() { Result = result.Result == Result.Success ? Result.Success : Result.Failed };
        }
        public async Task<ResponseResult> DeleteInvoicesReceipts(List<int> InvoiceIds)
        {
            List<int?> invoicelist = Lists.invoiceAccredit.ConvertAll<int?>(h => h);
            var RecieptsDel = ReceiptQuery.TableNoTracking
                .Where(h => h.ParentId != null
                && InvoiceIds.Contains(h.ParentId.Value)
                && h.IsBlock == false
                && invoicelist.Contains(h.ParentTypeId)).ToList();

            //
            if (RecieptsDel.Where(h => h.ParentId != null && h.IsAccredit == true).Count() > 0)
                return new ResponseResult()
                {
                    Result = Result.Failed,
                    ErrorMessageAr = "لا يمكن حذف على  سندات الفواتير المعتمده",
                    ErrorMessageEn = "Can not ]Delete  Accredit Invoice Receipts "
                };

            if (RecieptsDel.Count <= 0)
                return new ResponseResult() { Result = Result.NotFound, ErrorMessageAr = "لا يوجد هذا العنصر للحذف  ", ErrorMessageEn = "this item not found to delete" };

            var Ids = RecieptsDel.Select(a => a.Id).ToList().ConvertAll<int?>(h => h);
            //RecieptsDel.Select(e => { e.IsBlock = true; return e; }).ToList();
            ReceiptCommand.RemoveRange(RecieptsDel);
            var save = await ReceiptCommand.SaveAsync();

            if (save)
            {

                List<GLJournalEntry> journalEntry = JournalenteryQuery.TableNoTracking.Where(h => Ids.Contains(h.ReceiptsId)).ToList();
                var EnteryIds = journalEntry.Select(h => h.Id).ToList();
                save = await JournalenteryDetailCommand.DeleteAsync(a => EnteryIds.Contains(a.JournalEntryId));

                save = await JournalenteryCommand.DeleteAsync(a => EnteryIds.Contains(a.Id));

            }
            else
                return new ResponseResult() { Result = Result.Failed };


            return new ResponseResult() { Result = save ? Result.Success : Result.Success };

            //

            //return await DeleteReciepts(receiptsID.ConvertAll<int?>(h=> h));

        }
        public async Task<ResponseResult> DeleteReciepts(List<int?> Ids)
        {

            var RecieptsDel = await ReceiptCommand.Get(q => Ids.Contains(q.Id) && q.IsBlock == false);
            if (RecieptsDel.Where(h => h.ParentId != null && h.IsAccredit == true).Count() > 0)
                return new ResponseResult()
                {
                    Result = Result.Failed,
                    ErrorMessageAr = "لا يمكن حذف على  سندات الفواتير المعتمده",
                    ErrorMessageEn = "Can not ]Delete  Accredit Invoice Receipts "
                };
            if (RecieptsDel.Count <= 0)
                return new ResponseResult() { Result = Result.NotFound, ErrorMessageAr = "لا يوجد هذا العنصر للحذف  ", ErrorMessageEn = "this item not found to delete" };

            //RecieptsDel.Select(e => { e.IsBlock = true; return e; }).ToList();

            var save = await ReceiptCommand.DeleteAsync(RecieptsDel);
            if (save)
            {

                List<GLJournalEntry> journalEntry = JournalenteryQuery.TableNoTracking.Where(h => Ids.Contains(h.ReceiptsId)).ToList();
                var ids = journalEntry.Select(h => h.Id).ToList();
                save = await JournalenteryDetailCommand.DeleteAsync(a => ids.Contains(a.JournalEntryId));

                save = await JournalenteryCommand.DeleteAsync(a => ids.Contains(a.Id));

            }
            else
                return new ResponseResult() { Result = Result.Failed };


            return new ResponseResult() { Result = save ? Result.Success : Result.Success };


        }
    }

}
