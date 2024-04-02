﻿using App.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process
{
   public class InvoiceMaster 
    {

        //إذن الاضافة
        public int BranchId { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceType { get; set; }
        public int Code { get; set; }
        public double Serialize { get; set; }
        public string ParentInvoiceCode { get; set; }
        public bool IsDeleted { get; set; }
        public string BookIndex { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int StoreId { get; set; }
        public int? StoreIdTo { get; set; }
        public int transferStatus { get; set; }
        public double TotalPrice { get; set; } = 0;
        public string Notes { get; set; }
        public string TransferNotesAR { get; set; }
        public string TransferNotesEN { get; set; }
        public int InvoiceTypeId { get; set; } // enum of invoiceType
        public int InvoiceSubTypesId { get; set; } // enum of invoice sub Types for main types in InvoiceTypeId مرتجع كلى او جزئي ....
        public string BrowserName { get; set; }
        public bool IsAccredite { get; set; } = false;
        public int EmployeeId { get; set; } 

        //المشتريات
        public int? PersonId { get; set; }//المورد
        public double TotalDiscountValue { get; set; } = 0;//قيمه الخصم
        public double TotalDiscountRatio { get; set; } = 0;//نسبة الخصم
        public double Net { get; set; } = 0;//الصافي
        public double Paid { get; set; } = 0;//المدفوع 
        public double Remain { get; set; } = 0;//المتبقي
        public double VirualPaid { get; set; } = 0;//المدوفوع من العميل 
        public double TotalAfterDiscount { get; set; } = 0; //اجمالي بعد الخصم
        public double TotalVat { get; set; } = 0;//اجمالي قيمه الضريبه 
        public bool ApplyVat { get; set; }//يخضع للضريبه ام لا
        public bool PriceWithVat { get; set; }//السعر شامل الضريبه ام لا
        public int DiscountType { get; set; } //نوع الخصم (اجمالي او على الصنف)
        public int PaymentType { get; set; }
        public bool ActiveDiscount { get; set; }
        public bool IsReturn { get; set; } // تم عمل مرتجع للفاتورة
        public int? SalesManId { get; set; } //مندوب المبيعات
        public int? PriceListId { get; set; } // قائمة الاسعار
        public string InvoiceTransferType { get; set; }

        public int RoundNumber { get; set; } //  رقم التقريب المسجل عند حفظ الفاتورة
        //Common relations with other tables
        public int? POSSessionId { get; set; }
        public double ActualNet { get; set; }  // الصافي بدون تقريب
        public string CodeOfflinePOS { get; set; } 
        public double  TotalOtherAdditions { get; set; } // مجموع الاضافات الاخرى (بتسمع ف الربحية مش الفاتوره نفسها)
        public bool IsCollectionReceipt { get; set; } // تمنع التعديل على الفاتورة لان تم اضافة سند تحصيل عليها
        public bool eInvoiceReported { get; set; } = false;
        public string eInvoiceCode { get; set; }
        public virtual InvPersons Person { get; set; } //one to may with persons table
        public virtual ICollection<InvoiceFiles> InvoiceFiles { get; set; }//Relation with invoice files table which is one to many
        public virtual GLBranch Branch { get; set; }
        public virtual InvEmployees Employee { get; set; }
        public virtual InvStpStores store { get; set; }
        public virtual InvStpStores storeTo { get; set; }
        public virtual ICollection<InvoiceDetails> InvoicesDetails { get; set; }
        public virtual InvSalesMan salesMan { get; set; }
        public virtual ICollection<InvPurchaseAdditionalCostsRelation> InvPurchaseAdditionalCostsRelations { get; set; }
        public virtual ICollection<InvoicePaymentsMethods> InvoicePaymentsMethods { get; set; }
    //    public virtual ICollection<InvSerialTransaction> Serials { get; set; }
    //    public virtual ICollection<InvoiceSerialize> InvoiceSerializes { get; set; }
    }
}
