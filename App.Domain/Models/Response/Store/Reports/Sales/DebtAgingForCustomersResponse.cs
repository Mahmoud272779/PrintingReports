using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Response.Store.Reports.Sales
{
    public class DebtAgingForCustomersOrSupliersResponse
    {
        public double TotalInvoicesPrice { get; set; }
        public double TotalPaied { get; set; }
        public double TotalBalance { get; set; }
        public HashSet<TotalsValueOfInvoicesList> InvoicesList { get; set; }
    }
    public class InvoicesList
    {
        public string InvoiceCode { get; set; }
        public string Date { get; set; }
        public int ageOfInvoiceByDays { get; set; }
        public double Net { get; set; }
        public double Paied { get; set; } 
        public double Balance { get; set; }
        public int SalesManId { get; set; }
        public int PersonId { get; set; }
        public int groupId { get; set; }

    }
    public class TotalsValueOfInvoicesList
    {
        public int SalesmanId { get; set; }
        public string SalesManNameAr { get;set; }
        public string SalesManNameEn { get; set; }
        public int PersonID { get; set; } // Customer or Supplier
        public int PersonCode { get; set; }
        public string PersonNameAr { get; set; }
        public string PersonNameEn { get; set; }
        public string Phone { get; set; }
        public double Total { get; set; }
        public double TotalPaied { get; set; }
        public double TotalBalance { get; set; }
        public int groupId { get; set; }
        public HashSet<InvoicesList> InvoicesList { get; set; }


    }
}
