﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Security.Authentication.Response.Store
{
   public class InvGeneralSettingsResponse
    {
        public Purchases purchase { get; set; } = new Purchases();
        public POS pos { get; set; } 

        public Sales sales { get; set; }
        public Other other { get; set; }
        public Funds funds { get; set; }
        public Barcode barcode { get; set; }
        public VAT vat { get; set; }
        public Accredite accredite { get; set; }
        public CustomerDisplay customerDisplay { get; set; }
 
    }

   public class Purchases
    {
        public bool Purchases_ModifyPrices { get; set; }
        public bool Purchases_PayTotalNet { get; set; }
        public bool Purchases_UseLastPrice { get; set; }
        public bool Purchases_PriceIncludeVat { get; set; }
        public bool Purchases_PrintWithSave { get; set; }
        public bool Purchases_ReturnWithoutQuantity { get; set; }
        public bool Purchases_ActiveDiscount { get; set; }
        public bool Purchase_UpdateItemsPricesAfterInvoice { get; set; }

    }

    public class POS
    {
        public bool Pos_ModifyPrices { get; set; }
        public bool Pos_ExceedPrices { get; set; }
        public bool Pos_ExceedDiscountRatio { get; set; }
        public bool Pos_UseLastPrice { get; set; }
        public bool Pos_ActivePricesList { get; set; }
        public bool Pos_ExtractWithoutQuantity { get; set; }
        public bool Pos_PriceIncludeVat { get; set; }
        public bool Pos_ActiveDiscount { get; set; }
        public bool Pos_DeferredSale { get; set; }
        public bool Pos_IndividualCoding { get; set; }
        public bool Pos_PreventEditingRecieptFlag { get; set; }
        public int Pos_PreventEditingRecieptValue { get; set; }
        public bool Pos_ActiveCashierCustody { get; set; }
        public bool Pos_PrintPreview { get; set; }
        public bool Pos_PrintWithEnding { get; set; }
        public bool Pos_EditingOnDate { get; set; }
        public PosTouch PosTochSettings { get; set; }
    }

    public class PosTouch
    {
        public double fontSize { get; set; }
        public double categoryImageWidth { get; set; }
        public double categoryImageHeight { get; set; }
        public bool displayItemPrice { get; set; }
        public double itemsImageWidth { get; set; }
        public double itemsImageHeight { get; set; }
    }

    public class Sales
    {
        public bool Sales_ModifyPrices { get; set; }
        public bool Sales_ExceedPrices { get; set; }
        public bool Sales_ExceedDiscountRatio { get; set; }
        public bool Sales_PayTotalNet { get; set; }
        public bool Sales_UseLastPrice { get; set; }
        public bool Sales_ExtractWithoutQuantity { get; set; }
        public bool Sales_PriceIncludeVat { get; set; }
        public bool Sales_PrintWithSave { get; set; }
        public bool Sales_ActiveDiscount { get; set; }
        public bool Sales_LinkRepresentCustomer { get; set; }
        public bool Sales_ActivePricesList { get; set; }
    }

    public class Other
    {
        public bool Other_MergeItems { get; set; }
        public string otherMergeItemMethod { get; set; }
        public bool Other_ItemsAutoCoding { get; set; }
        public bool Other_ZeroPricesInItems { get; set; }
        public bool Other_PrintSerials { get; set; }
        public bool Other_AutoExtractExpireDate { get; set; }
        public bool Other_ViewStorePlace { get; set; }
        public bool Other_ConfirmeSupplierPhone { get; set; }
        public bool Other_ConfirmeCustomerPhone { get; set; }
        public bool Other_DemandLimitNotification { get; set; }
        public bool Other_ExpireNotificationFlag { get; set; }
        public int Other_ExpireNotificationValue { get; set; }
        public int Other_Decimals { get; set; }
        public bool Other_useRoundNumber { get; set; }
        public int autoLogoutInMints { get; set; }
        public bool Other_ShowBalanceOfPerson { get; set; }
        public bool isCollectionReceipt { get; set; }
    }

    public class Funds
    {
        public bool Funds_Items { get; set; }
        public bool Funds_Supplires { get; set; }
        public bool Funds_Customers { get; set; }
        public bool Funds_Safes { get; set; }
        public bool Funds_Banks { get; set; }
    }
    public class Barcode
    {
        public string barcodeType { get; set; }
        public bool Barcode_ItemCodestart { get; set; }
    }
    public class VAT
    {
        public bool Vat_Active { get; set; }
        public double Vat_DefaultValue { get; set; }
    }
    public class Accredite
    {
        public DateTime Accredite_StartPeriod { get; set; }
        public DateTime Accredite_EndPeriod { get; set; }
    }
    public class CustomerDisplay
    {
        public bool CustomerDisplay_Active { get; set; }
        public string CustomerDisplay_PortNumber { get; set; }
        public int CustomerDisplay_Code { get; set; }
        public int CustomerDisplay_LinesNumber { get; set; }
        public int CustomerDisplay_CharNumber { get; set; }
        public string CustomerDisplay_DefaultWord { get; set; }
        public int CustomerDisplay_ScreenType { get; set; }
    }
    public class ElectronicInvoice
    {
 
        public string OTP { get; set; }
        public bool ActiveElectronicInvoice { get; set; }
    }
}
