using App.Application.Handlers.Invoices.InvCollectionReceipt;
using App.Application.Helpers;
using App.Domain.Entities.Process;
using App.Domain.Models.Request.GeneralLedger;
using App.Domain.Models.Security.Authentication.Request.Reports;
using App.Domain.Models.Security.Authentication.Response.Store;
using App.Infrastructure.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Application
{
    public  class PaymentMethodsForInvoiceService : IPaymentMethodsForInvoiceService
    {
        private readonly IRepositoryCommand<InvoicePaymentsMethods> PaymentsMethodsCommand;
        private readonly IRepositoryQuery<InvoicePaymentsMethods> PaymentsMethodsQuery;

        public PaymentMethodsForInvoiceService(IRepositoryCommand<InvoicePaymentsMethods> PaymentsMethodsCommand
            , IRepositoryQuery<InvoicePaymentsMethods> PaymentsMethodsQuery) 
        {
            this.PaymentsMethodsCommand = PaymentsMethodsCommand;
            this.PaymentsMethodsQuery = PaymentsMethodsQuery;
        }

        public async Task<bool> SavePaymentMethods(int invoiceType , List<PaymentList> PaymentMethods , int InvoiceId,int BranchId,double Paid , bool isUpdate , int roundNumber)
        {
            var saved = true;
            if (Lists.SalesWithOutDeleteInvoicesList.Contains(invoiceType) || Lists.purchasesWithOutDeleteInvoicesList.Contains(invoiceType))
            {


                // Delete Payment methods
                if(isUpdate)
                {
                   await  PaymentsMethodsCommand.DeleteAsync(a => a.InvoiceId == InvoiceId);
                /*    var invoicePaymentList = PaymentsMethodsQuery.FindAll(q => q.InvoiceId == InvoiceId).ToList();
                   
                    if(invoicePaymentList.Count > 0)
                       PaymentsMethodsCommand.RemoveRange(invoicePaymentList);*/

                }

                if (PaymentMethods != null && PaymentMethods.Count() > 0)
                {
                    if (PaymentMethods.Count() > 0)
                    {
                        
                       
                        var invoicePaymentMethodList = new List<InvoicePaymentsMethods>();
                        foreach (var item in PaymentMethods)
                        {
                            var invoicePaymentMethod = new InvoicePaymentsMethods();
                            invoicePaymentMethod.InvoiceId = InvoiceId;
                            invoicePaymentMethod.BranchId = BranchId;
                            invoicePaymentMethod.Cheque = item.Cheque;
                            invoicePaymentMethod.PaymentMethodId = item.PaymentMethodId;
                            invoicePaymentMethod.Value =Math.Round( item.Value,roundNumber);
                            invoicePaymentMethodList.Add(invoicePaymentMethod);
                        }
                        PaymentsMethodsCommand.AddRange(invoicePaymentMethodList);
                        saved =await  PaymentsMethodsCommand.SaveAsync();
                    }

                }
                else if (Paid > 0)
                {
                    var invoicePaymentMethod = new InvoicePaymentsMethods();
                    invoicePaymentMethod.InvoiceId = InvoiceId;
                    invoicePaymentMethod.BranchId = BranchId;
                    invoicePaymentMethod.Cheque = "";
                    invoicePaymentMethod.PaymentMethodId = 1;//نقدي
                    invoicePaymentMethod.Value =Math.Round( Paid,roundNumber);
                    saved= PaymentsMethodsCommand.Add(invoicePaymentMethod);
               //     saved = await PaymentsMethodsCommand.SaveAsync();

                }
            }
            return saved;
        }


        public async Task<bool> updatePaymentMethodsForCollectionReceipt(List<CollectionPaymentMethods> request)
        {

            var paymentList = request.GroupBy(a => new { a.branchId,a.invoiceId, a.PaymentMethodId,a.signal })
                     .Select(a => new { a.Key.branchId, a.Key.invoiceId, a.Key.PaymentMethodId,a.Key.signal, Cheque = a.First().Cheque, Value = a.Sum(e => e.Value) });
            var existMethods = PaymentsMethodsQuery.TableNoTracking
                         .Where(a => paymentList.Select(e => e.invoiceId).Contains(a.InvoiceId)).ToList();
         
            var invoicePaymentMethodList = new List<InvoicePaymentsMethods>();

            foreach (var method in paymentList)
            {
               var exist = existMethods .Where(a => a.InvoiceId == method.invoiceId && a.PaymentMethodId == method.PaymentMethodId)
                     .Select(a => a.Value = a.Value +(method.signal* method.Value)).ToList();
                if(exist == null || exist.Count()==0)
                {
                    
                        var invoicePaymentMethod = new InvoicePaymentsMethods();
                        invoicePaymentMethod.InvoiceId = method.invoiceId;
                        invoicePaymentMethod.BranchId = method.branchId;
                        invoicePaymentMethod.Cheque = method.Cheque;
                        invoicePaymentMethod.PaymentMethodId = method.PaymentMethodId;
                        invoicePaymentMethod.Value = method.Value;
                        invoicePaymentMethodList.Add(invoicePaymentMethod);
                   
                }
              
            }
            var methodsWillUpdated = existMethods.Where(a => a.Value > 0).ToList();
            var methodsWillDeleted = existMethods.Where(a => a.Value <= 0).ToList();

            try
            {
                if(methodsWillUpdated.Count() > 0)
                    await PaymentsMethodsCommand.UpdateAsyn(methodsWillUpdated);
                if(methodsWillDeleted.Count() > 0)
                {
                    PaymentsMethodsCommand.RemoveRange(methodsWillDeleted);
                   await PaymentsMethodsCommand.SaveAsync();
                }
                     
                if(invoicePaymentMethodList.Count()>0)
                  PaymentsMethodsCommand.AddRangeAsync(invoicePaymentMethodList);
            }
            catch (Exception e)
            {

                throw;
            }
     
           //var  saved = await PaymentsMethodsCommand.SaveAsync();
            return true;
        }
    }
}
