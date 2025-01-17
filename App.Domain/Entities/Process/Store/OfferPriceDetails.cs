﻿using App.Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process
{
   public class OfferPriceDetails
    {
        //إذن الاضافة
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ItemId { get; set; }
        public int? UnitId { get; set; }
        public int? SizeId { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; } // سعر شراء او بيع
     //   public double PurchasePrice { get; set; }//سعر الشراء
        public double TotalWithSplitedDiscount { get; set; } // for reports total with splited discount
        public double TotalWithOutSplitedDiscount { get; set; }  // for front total without splited discount just discount value on item
        [Column(TypeName = "date")]
        public DateTime? ExpireDate { get; set; }
        public int Signal { get; set; }
        public int ItemTypeId { get; set; }

        //المشتريات
        public double DiscountValue { get; set; }//قيمه الخصم على مستوى الصنف
        public double DiscountRatio { get; set; }// نسبه الخصم على مستوى الصنف
        public double VatRatio { get; set; }// الضريبه على مستوى الصنف 
        public double VatValue { get; set; }// الضريبه على مستوى الصنف 
      
        // public double SalePrice { get; set; }//سعر البيع 
        public double  TransQuantity{ get; set; }//الكمية المحولة
        public double ReturnQuantity { get; set; }//الكمية المرتجعة saved in the small unit
        public int StatusOfTrans { get; set; }//التحويلات
        public double SplitedDiscountValue { get; set; } // قيمة الخصم الاجمالى الموزع 
        public double SplitedDiscountRatio { get; set; } //  نسبة الخصم الاجمالى الموزع
        public double AvgPrice { get; set; } // متوسط السعر 
        public double Cost { get; set; }// الربحية
        public double AutoDiscount { get; set; }// خصومات الاصناف
        public int PriceList { get; set; }// قائمه اسعار البيع (enum)
        public double MinimumPrice { get; set; }// اقل سعر بيع 
        public double ConversionFactor { get; set; }
        public int indexOfSerialNo { get; set; } // لو دخلت نفس الصنف السيريال ف نفس الفاتورة يحدد كل سيريال تبع اى ريكورد
        public int indexOfItem { get; set; } // لترتيب الاصناف بنفس ترتيب اضافتها لو بيساوي زيرو يبقا ده من مكونات صنف مركب
        public string balanceBarcode { get; set; }
        public int? parentItemId { get; set; } // رقم الصنف المركب
        public virtual InvStpUnits Units { get; set; }
        public virtual InvSizes Sizes { get; set; }
        public virtual InvStpItemCardMaster Items { get; set; }
        public virtual OfferPriceMaster OfferPriceMaster { get; set; }
        // public virtual InvStpItemCardUnit ItemUnits { get; set; }

    }
}
