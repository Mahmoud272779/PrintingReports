using App.Application.Handlers.Invoices.InvCollectionReceipt;
using App.Domain.Models.Security.Authentication.Request.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application
{
    public interface IPaymentMethodsForInvoiceService
    {
         Task<bool> SavePaymentMethods(int invoiceType, List<PaymentList> PaymentMethods, int InvoiceId, int BranchId, double Paid, bool isUpdate ,int roundNumber);
        Task<bool> updatePaymentMethodsForCollectionReceipt(List<CollectionPaymentMethods> request);


    }
}
