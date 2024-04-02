using App.Application.Helpers;
using App.Application.Services.Process.Invoices.General_APIs;
using App.Application.Services.Process.StoreServices.Invoices.General_APIs;
using App.Domain.Entities.Process;
using App.Domain.Entities.Process.Store;
using App.Domain.Entities.Setup;
using App.Domain.Models.Security.Authentication.Request.Reports;
using App.Infrastructure;
using App.Infrastructure.Helpers;
using App.Infrastructure.Interfaces.Repository;
using DocumentFormat.OpenXml.Wordprocessing;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Domain.Enums.Enums;
using DocumentType = App.Domain.Enums.Enums.DocumentType;

namespace App.Application
{
    public  class RedefineInvoiceRequestService : IRedefineInvoiceRequestService
    {
        private readonly IRepositoryQuery<InvStpItemCardMaster> itemMasterQuery;
        private readonly IRepositoryQuery<InvStpItemCardUnit> itemUnitQuery;
        private readonly IRepositoryQuery<InvStpUnits> UnitQuery;
        private readonly IBalanceBarcodeProcs balanceBarcode;
        private readonly IGeneralAPIsService generalAPIsService;
        private readonly IRepositoryQuery<InvPersonLastPrice> personLastPriveQuery;
        public RedefineInvoiceRequestService(IRepositoryQuery<InvStpItemCardMaster> itemMasterQuery,
            IRepositoryQuery<InvStpItemCardUnit> itemUnitQuery, IRepositoryQuery<InvStpUnits> UnitQuery
            , IBalanceBarcodeProcs balanceBarcode
            , IGeneralAPIsService generalAPIsService, IRepositoryQuery<InvPersonLastPrice> personLastPriveQuery)
        {
            this.itemMasterQuery = itemMasterQuery;
            this.itemUnitQuery = itemUnitQuery;
            this.UnitQuery = UnitQuery;
            this.balanceBarcode = balanceBarcode;
            this.generalAPIsService = generalAPIsService;
            this.personLastPriveQuery = personLastPriveQuery;
        }
        public async Task<Tuple<InvoiceMasterRequest, string, string>> setInvoiceRequest(InvoiceMasterRequest sentRequest,InvGeneralSettings  setting ,int invoiceTypeId , string? parentInvoiceType)
       {

            // check if Items exist
            var itemsInRequest = sentRequest.InvoiceDetails.Select(e => e.ItemId).Distinct().ToList();
            var itemsInDb = itemMasterQuery.TableNoTracking.Where(a => itemsInRequest.Contains(a.Id)).ToList();
            //check if any item not exist in DB
            if (itemsInRequest.Count() > itemsInDb.Count())
            {
                var itemsNotExist = string.Join(",", itemsInRequest.Where(a => !itemsInDb.Select(a => a.Id).Contains(a)).ToArray());
                return new Tuple<InvoiceMasterRequest, string, string>(sentRequest, string.Concat(" الاصناف", itemsNotExist, "غير موجوده "), 
                    string.Concat("Items ", itemsNotExist, " not exist"));
            }

            // check if ItemS exist
            var unitsInRequest = sentRequest.InvoiceDetails.Where(a=>a.ItemTypeId!=(int) ItemTypes.Note).Select(e => e.UnitId.Value).Distinct().ToList();
            var unitsInDb = UnitQuery.TableNoTracking.Where(a => unitsInRequest.Contains(a.Id)).ToList();
            //check if any item not exist in DB
            if (unitsInRequest.Count() > unitsInDb.Count())
            {
                var unitsNotExist = string.Join(",", unitsInRequest.Where(a => !unitsInDb.Select(e => e.Id).Contains(a)).ToArray());
                return new Tuple<InvoiceMasterRequest, string, string>(sentRequest, string.Concat( " الوحدات", unitsNotExist, "غير موجوده "),
                    string.Concat("Units ", unitsNotExist, " not exist"));
            }

           
            var itemsIdInRequest = sentRequest.InvoiceDetails.Select(e => e.ItemId).ToList();
            var itemUnitsInDb = itemUnitQuery.TableNoTracking
                                  .Where(a => itemsIdInRequest.Contains(a.ItemId) && a.Item.TypeId != (int)ItemTypes.Note).ToList();

            // last price
            //old query  // will refactor later
            /* var LastPrice = invoiceDetailsQuery.TableNoTracking.Where(a => a.InvoicesMaster.InvoiceTypeId == invoiceTypeId
                   && a.InvoicesMaster.PersonId == sentRequest.PersonId && itemsInRequest.Contains(a.ItemId)
                   && unitsInRequest.Contains(a.UnitId.Value));*/

            //new query 
            /*  var _LastPrice = (LastPrice.Count() == 0 ? null : (LastPrice.GroupBy(a => new { a.ItemId, a.UnitId })
                 .Select(a => new
                 {

                     a.Key.ItemId,
                     a.Key.UnitId,
                     gg = a.OrderByDescending(b => b.ItemId).ThenByDescending(s => s.UnitId)
                     .ThenByDescending(s => s.InvoiceId).Count(),
                     price = a.First().Price
                 })));*/


            var lastPrice = personLastPriveQuery.TableNoTracking.Where(a =>a.invoiceTypeId== invoiceTypeId &&  a.personId == sentRequest.PersonId
                        && itemsInRequest.Contains(a.itemId)   && unitsInRequest.Contains(a.unitId));

            // check details of invoice
            foreach (var item in sentRequest.InvoiceDetails)
            {
               
                var itemMasterDb = itemsInDb.First(a => a.Id == item.ItemId);
                item.ItemTypeId = itemMasterDb.TypeId;
                item.ApplyVat = itemMasterDb.ApplyVAT;
                item.VatRatio = itemMasterDb.VAT;
                item.ItemCode = itemMasterDb.ItemCode;
                if(Lists.POSInvoicesList.Contains( invoiceTypeId) && item.ItemTypeId == (int)ItemTypes.Note)
                {
                    return new Tuple<InvoiceMasterRequest, string, string>(sentRequest, " لا يمكن اضافة صنف ملاحظة " , " Can not add item note " );

                }
                if (item.ItemTypeId == (int)ItemTypes.Note)
                    continue;
                // check if units exist with selected items
                var unitMachedWithItem = itemUnitsInDb.Where(a => a.ItemId == item.ItemId && a.UnitId == item.UnitId);
                if (unitMachedWithItem.Count() == 0 && itemUnitsInDb.Count() > 0)
                    return new Tuple<InvoiceMasterRequest, string, string>(sentRequest,
                                     string.Concat(" الوحدة ", item.UnitId, " غير موجوده للصنف", item.ItemId),
                                      string.Concat(" Unit ", item.UnitId, " not exist with item ", item.ItemId));

                item.ConversionFactor = unitMachedWithItem.First().ConversionFactor;
                // check if this item is balance barcode or not
                if (Lists.salesInvoicesList.Contains(invoiceTypeId) || Lists.POSInvoicesList.Contains(invoiceTypeId) || invoiceTypeId==(int)DocumentType.OfferPrice)
                     if(!string.IsNullOrEmpty(item.balanceBarcode))
                     {
                          BalanceBarcodeResult getitemBalanceCode = balanceBarcode.getItem(new BalanceBarcodeInput()
                          { FullCode = item.balanceBarcode ,
                              ItemCodestart = setting.Barcode_ItemCodestart ? 1 : 0,
                          });
                          if (getitemBalanceCode.ItemCode == item.ItemCode)
                             item.isBalanceBarcode = true;
                          else
                             item.balanceBarcode = null;
                    }

                // ممنوع التعديل على السعر 
                // ModifyPrices , UseLastPrice  , UseLastPrice ,ExceedPrices
                double price = 0;


                //if ( ((Lists.salesInvoicesList.Contains(invoiceTypeId) || invoiceTypeId == (int)DocumentType.OfferPrice)
                //    && setting.Sales_ExceedPrices)
                //   || (Lists.POSInvoicesList.Contains(invoiceTypeId) && setting.Pos_ExceedPrices))
                //{
                //    continue;
                //}
                double salePrice4 = 0;
 // اخر سعر بيع وشراء
               if ((Lists.purchasesInvoicesList.Contains(invoiceTypeId)&& setting.Purchases_UseLastPrice )
                    || (Lists.salesInvoicesList.Contains(invoiceTypeId) && setting.Sales_UseLastPrice )
                    || (Lists.POSInvoicesList.Contains(invoiceTypeId) && setting.Pos_UseLastPrice))
                {
                    // new query
                    //var priceFromInvoices = (_LastPrice == null ? null : _LastPrice.Where(a => a.ItemId == item.ItemId &&
                    //                                          a.UnitId == item.UnitId));

                    // old query
                    var priceFromInvoices = lastPrice.Where(a => a.itemId == item.ItemId &&
                                                              a.unitId == item.UnitId);
                    //if(priceFromInvoices.Count()>0)
                    if (priceFromInvoices != null && priceFromInvoices.ToList().Count() > 0)
                    {
                        price = priceFromInvoices.First().price;
                        salePrice4 = price;
                    }
                    else
                    {
                        if (Lists.InvoicesTypeForAddStore_Setting.Contains(invoiceTypeId))
                            price = itemUnitsInDb.Where(a => a.ItemId == item.ItemId && a.UnitId == item.UnitId).First().PurchasePrice;
                        else if (Lists.InvoicesTypeForExtractStore_Setting.Contains(invoiceTypeId) || Lists.POSInvoicesList.Contains(invoiceTypeId)
                            || invoiceTypeId == (int)DocumentType.OfferPrice)
                        {
                            price = itemUnitsInDb.Where(a => a.ItemId == item.ItemId && a.UnitId == item.UnitId).First().SalePrice1;
                            salePrice4 = itemUnitsInDb.Where(a => a.ItemId == item.ItemId && a.UnitId == item.UnitId).First().SalePrice4;

                        }
                    }
                }
                else
                {
                    if (Lists.InvoicesTypeForAddStore_Setting.Contains(invoiceTypeId))
                        price = itemUnitsInDb.Where(a => a.ItemId == item.ItemId && a.UnitId == item.UnitId).First().PurchasePrice;
                    else if (Lists.InvoicesTypeForExtractStore_Setting.Contains(invoiceTypeId) || Lists.POSInvoicesList.Contains(invoiceTypeId) 
                        || invoiceTypeId==(int)DocumentType.OfferPrice)
                    {
                        price = itemUnitsInDb.Where(a => a.ItemId == item.ItemId && a.UnitId == item.UnitId).First().SalePrice1;
                        salePrice4 = itemUnitsInDb.Where(a => a.ItemId == item.ItemId && a.UnitId == item.UnitId).First().SalePrice4;

                    }
                }
                   
                // التعديل على سعر البيع
                if (((Lists.purchasesInvoicesList.Contains(invoiceTypeId)|| invoiceTypeId==(int)DocumentType.PurchaseOrder || Lists.purchasesWithoutVatInvoicesList.Contains(invoiceTypeId))
                    && !setting.Purchases_ModifyPrices)
                     || ((Lists.salesInvoicesList.Contains(invoiceTypeId) || invoiceTypeId == (int)DocumentType.OfferPrice) && !setting.Sales_ModifyPrices)
                     || (Lists.POSInvoicesList.Contains(invoiceTypeId) && !setting.Pos_ModifyPrices))
                {
                   if(item.Price != price)
                    {
                        var itemMaster = itemsInDb.Where(a => a.Id == item.ItemId).First();
                        return new Tuple<InvoiceMasterRequest, string, string>(sentRequest, ErrorMessagesAr.cantEditPrice+" "+ itemMaster.ArabicName, ErrorMessagesEn.cantEditPrice +" "+ itemMaster.LatinName);

                    }

                }
                
                // تجاوز سعر البيع
                if (((Lists.salesInvoicesList.Contains(invoiceTypeId) || invoiceTypeId == (int)DocumentType.OfferPrice)
                    && !setting.Sales_ExceedPrices)
                   || (Lists.POSInvoicesList.Contains(invoiceTypeId) && !setting.Pos_ExceedPrices))
                {
                    if (salePrice4 > item.Price)
                    {
                        var itemMaster = itemsInDb.Where(a => a.Id == item.ItemId).First();
                        return new Tuple<InvoiceMasterRequest, string, string>(sentRequest, ErrorMessagesAr.itemExceedSalePrice 
                            + " " + itemMaster.ArabicName, ErrorMessagesEn.itemExceedSalePrice + " " + itemMaster.LatinName);

                    }
                }
            }
            // check 0 in price and quantity
            if(invoiceTypeId!= (int) DocumentType.IncomingTransfer)
            {
                 var zeroInPriceOrQuantity = sentRequest.InvoiceDetails.Where(a => a.ItemTypeId!= (int)ItemTypes.Note &&
                                                  (a.Price == 0 || a.Quantity == 0));
                    if(zeroInPriceOrQuantity.Any())
                    {
                        var zeroItems = String.Join(",", zeroInPriceOrQuantity.Select(a => a.ItemCode));
                        return new Tuple<InvoiceMasterRequest, string, string>(sentRequest, ErrorMessagesAr.CanNotEnterQuantityORPriceZero + zeroItems, ErrorMessagesEn.CanNotEnterQuantityORPriceZero + zeroItems);
                    }
            }
        
            if(invoiceTypeId !=(int) DocumentType.OfferPrice && invoiceTypeId != (int)DocumentType.PurchaseOrder)
            {
                // expiary date
                var expirayOfInvoice = sentRequest.InvoiceDetails.Where(a => a.ItemTypeId == (int)ItemTypes.Expiary && a.ExpireDate == null);
                if (expirayOfInvoice.Count() > 0)
                {
                    var expiryItems = String.Join(",", expirayOfInvoice.Select(a => a.ItemCode));
                    return new Tuple<InvoiceMasterRequest, string, string>(sentRequest, " يجب ادخال تاريخ الصلاحيه للأصناف" + expiryItems, "Expiry Date should be entered for items " + expiryItems);
                }

                //serials
                var serialsOfInvoice = sentRequest.InvoiceDetails.Where(a => a.ItemTypeId == (int)ItemTypes.Serial && (a.ListSerials == null || a.ListSerials.Count() == 0));
                if (serialsOfInvoice.Count() > 0 && invoiceTypeId != (int)DocumentType.IncomingTransfer)
                {

                    var serialsItems = String.Join(",", serialsOfInvoice.Select(a => a.ItemCode));
                    return new Tuple<InvoiceMasterRequest, string, string>(sentRequest, " يجب ادخال السيريالات للأصناف " + serialsItems, "Serials should be entered for items " + serialsItems);
                }
                serialsOfInvoice = sentRequest.InvoiceDetails.Where(a => a.ItemTypeId == (int)ItemTypes.Serial && (a.ListSerials.Count() != a.Quantity));
                if (serialsOfInvoice.Count() > 0 && invoiceTypeId != (int)DocumentType.IncomingTransfer)
                {

                    var serialsItems = String.Join(",", serialsOfInvoice.Select(a => a.ItemCode));
                    return new Tuple<InvoiceMasterRequest, string, string>(sentRequest, " يجب تساوى الكمية مع السيريالات للصنف  " + serialsItems, "Serials should be equal to quantity for items " + serialsItems);
                }

                var ListOfcomparingSerials = new List<int> { (int)DocumentType.IncomingTransfer, (int)DocumentType.ReturnPOS, (int)DocumentType.ReturnSales, (int)DocumentType.ReturnPurchase ,(int)DocumentType.ReturnWov_purchase};
                if (ListOfcomparingSerials.Contains(invoiceTypeId) && !string.IsNullOrEmpty(parentInvoiceType))
                {
                    var theSameSerials = generalAPIsService.CompareSerialsWithMainInvoice(sentRequest.InvoiceDetails, parentInvoiceType, invoiceTypeId);
                    if (!theSameSerials)
                        return new Tuple<InvoiceMasterRequest, string, string>(sentRequest, ErrorMessagesAr.notTheSameSerials, ErrorMessagesEn.notTheSameSerials);

                }
            }
          
            var compositeItems = sentRequest.InvoiceDetails.Where(a => a.ItemTypeId == (int)ItemTypes.Composite);
            if(Lists.CompositeItemOnInvoice.Contains(invoiceTypeId) && compositeItems.Count()>0)
            {
                foreach(var item in compositeItems.ToList())
                {
                       var componnentITems =generalAPIsService.setCompositItem( item.ItemId, item.UnitId.Value, item.Quantity);
                    sentRequest.InvoiceDetails.AddRange(componnentITems);
                }
            }

            return new Tuple<InvoiceMasterRequest, string, string>(sentRequest, "","");

        }
    

    }
}
