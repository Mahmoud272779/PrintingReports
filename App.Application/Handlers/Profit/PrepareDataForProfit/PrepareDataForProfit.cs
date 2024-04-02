using App.Application.Handlers.Profit.CalculatProfit;
using App.Application.SignalRHub;
using App.Domain.Entities.Process.General;
using App.Domain.Entities.Setup;
using App.Infrastructure.settings;
using Castle.Core.Internal;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace App.Application.Handlers.Profit
{
    public class PrepareDataForProfit : IPrepareDataForProfit
    {
        private readonly IRepositoryQuery<EditedItems> EditedItemQuery;
        private readonly IRepositoryQuery<signalR> _signalRQuery;
        private readonly IMemoryCache _memoryCache;
        private readonly IRepositoryQuery<InvoiceMaster> invoiceMasterQuery;//1
        private readonly IMediator MediatorService;
        private readonly IRepositoryQuery<InvStpItemCardUnit> itemCardUnitQuery;
        private readonly IRepositoryQuery<GLJournalEntry> jounralEnteryQuery;
        private readonly IRepositoryQuery<GLJournalEntryDetails> journalEntryDetailsQuery;
        private readonly IRepositoryCommand<GLJournalEntryDetails> journalEntryDetailsCommand;
        private readonly IRepositoryQuery<InvoiceDetails> invoiceDetailsQuery;
        private readonly IRepositoryQuery<InvStpItemCardParts> InvStpItemCardPartsQuery;
        private readonly IRepositoryQuery<InvStpItemCardUnit> InvStpItemCardUnitsQuery;
        private readonly IRepositoryCommand<InvoiceDetails> InvoiceDetailsCommand;
        private readonly IRepositoryCommand<EditedItems> editedItemsCommand;
        private readonly IRepositoryQuery<GLPurchasesAndSalesSettings> GLPurchasesAndSalesSettingsQuary;
        private readonly iUserInformation Userinformation;
        private readonly IHubContext<NotificationHub> _hub;
        private UserInformationModel userData;
        private readonly IRoundNumbers roundNumbers;



        public PrepareDataForProfit(IRepositoryQuery<EditedItems> editedItemQuery,
            IRepositoryQuery<InvoiceMaster> InvoiceMasterQuery,
            IRepositoryQuery<InvStpItemCardUnit> ItemCardUnitQuery,
            IRepositoryQuery<InvoiceDetails> InvoiceDetailsQuery,
            IRepositoryCommand<InvoiceDetails> invoiceDetailsCommand,
            IRepositoryCommand<EditedItems> EditedItemsCommand,
                                  iUserInformation Userinformation,
                                  IHubContext<NotificationHub> hub,
                                  IMemoryCache memoryCache,
                                  IRepositoryQuery<InvStpItemCardParts> invStpItemCardPartsQuery,
                                  IRepositoryQuery<InvStpItemCardUnit> invStpItemCardUnitsQuery,
                                  IMediator mediatorService,
                                  IRepositoryQuery<GLJournalEntry> jounralEnteryQuery,
                                  IRepositoryQuery<GLJournalEntryDetails> journalEntryDetailsQuery,
                                  IRepositoryCommand<GLJournalEntryDetails> journalEntryDetailsCommand,
                                  IRepositoryQuery<GLPurchasesAndSalesSettings> gLPurchasesAndSalesSettingsQuary,
                                  IRoundNumbers roundNumbers,
                                  IRepositoryQuery<signalR> signalRQuery)
        {
            EditedItemQuery = editedItemQuery;
            invoiceMasterQuery = InvoiceMasterQuery;
            itemCardUnitQuery = ItemCardUnitQuery;
            invoiceDetailsQuery = InvoiceDetailsQuery;
            this.Userinformation = Userinformation;
            InvoiceDetailsCommand = invoiceDetailsCommand;
            editedItemsCommand = EditedItemsCommand;
            _hub = hub;
            _memoryCache = memoryCache;
            InvStpItemCardPartsQuery = invStpItemCardPartsQuery;
            InvStpItemCardUnitsQuery = invStpItemCardUnitsQuery;
            MediatorService = mediatorService;
            this.jounralEnteryQuery = jounralEnteryQuery;
            this.journalEntryDetailsQuery = journalEntryDetailsQuery;
            this.journalEntryDetailsCommand = journalEntryDetailsCommand;
            GLPurchasesAndSalesSettingsQuary = gLPurchasesAndSalesSettingsQuary;
            userData = Userinformation.GetUserInformation().Result;
            this.roundNumbers = roundNumbers;
            _signalRQuery = signalRQuery;
        }

        public async Task<ResponseResult> PreparingDataForProfit(int BranchIdd, int? itemId = null)
        {
            IPCS pcs = new PCS();
            //var userData = Userinformation.GetUserInformation();
            var memoryCash = _memoryCache.Get<List<SignalRCash>>(defultData.ProfitKey);
            bool isCompanyExistBefore = memoryCash != null ? memoryCash.Where(x => x.CompanyLogin == userData.companyLogin).Any() : false;
            MemoryCashHelper _cashHelper = new MemoryCashHelper(_memoryCache);
            await _cashHelper.AddSignalRCash(new SignalRCash
            {
                connectionId = userData.signalRConnectionId,
                CompanyLogin = userData.companyLogin,
                EmployeeId = userData.employeeId,
                UserID = userData.userId

            }, defultData.ProfitKey);


            var EmployeeIds = _memoryCache.Get<List<SignalRCash>>(defultData.ProfitKey).Where(x => x.CompanyLogin == userData.companyLogin).Select(x => x.EmployeeId).ToArray();
            //string[] usersSignalRConnectionId = _memoryCache.Get<List<SignalRCash>>(defultData.ProfitKey).Where(x => x.CompanyLogin == userData.companyLogin).Select(x => x.connectionId).ToArray();
            string[] usersSignalRConnectionId = _signalRQuery.TableNoTracking.Where(x => EmployeeIds.Contains(x.InvEmployeesId)).Select(x => x.connectionId).ToArray();

            if (string.IsNullOrEmpty(usersSignalRConnectionId.Find(a => a == userData.signalRConnectionId)))
            { usersSignalRConnectionId = usersSignalRConnectionId.Append(userData.signalRConnectionId).ToArray(); }
            int counter = 0;
            var currentCompanyLogin = userData.companyLogin;
            var ItemData = EditedItemQuery.TableNoTracking
                .Where(h => !Lists.itemNotRegular.Contains(h.type) && itemId == null ? 1 == 1 : h.itemId == itemId)
                .ToList();

            int hamada = 0;
            try
            {
                List<CompositeData> compositeItem = new List<CompositeData>();
                List<EditedItems> ReqularItems = ItemData.Where(h => h.type != (int)ItemTypes.Composite).ToList();
                ProfiteRequest profitParameter = new ProfiteRequest();

                if (isCompanyExistBefore)
                {
                    //await _hub.Clients.Clients(usersSignalRConnectionId).SendAsync(defultData.ProfitProgress, new porgressData { status = Aliases.ProgressStatus.InProgress });
                    return new ResponseResult()
                    {
                        Result = Result.InProgress,
                        ErrorMessageAr = " profit is running ",
                        ErrorMessageEn = " profit is running",
                        Alart = new Alart
                        {
                            AlartType = AlartType.warrning,
                            type = AlartShow.note,
                            MessageAr = "برجاء الانتظار جاري حساب الربحيه",
                            MessageEn = "Please wait the profit is in progress",
                        }
                    };
                    return new ResponseResult() { Result = Result.InProgress, ErrorMessageAr = " profit is running ", ErrorMessageEn = " profit is running" };
                }
                await _hub.Clients.Clients(usersSignalRConnectionId).SendAsync(defultData.ProfitProgress, new porgressData { percentage = 0, Count = counter, totalCount = ReqularItems.Count, status = Aliases.ProgressStatus.InProgress });

                foreach (var item in ReqularItems)
                {
                    counter++;
                    compositeItem = GetCompositeFromItsComponent(item.itemId, item.serialize, item.BranchID, compositeItem);
                    hamada = 1;
                    // save connection in cache to use in progress par
                    //usersSignalRConnectionId = _memoryCache.Get<List<SignalRCash>>(defultData.ProfitKey).Where(x => x.CompanyLogin == currentCompanyLogin).Select(x => x.connectionId).ToArray();
                    usersSignalRConnectionId = _signalRQuery.TableNoTracking.Select(x => x.connectionId).ToArray();

                    //begin collecting data
                    lastInvoiceEditedDTO lastinvoiceEdit = Getlastinvoice(item);
                    hamada = 2;
                    List<InvoicesData> invoices = GetInvoiceData(new List<int> { item.itemId }, item.sizeId, item.serialize, item.BranchID);
                    hamada = 3;
                    List<InvStpItemCardUnit> itemCard = itemCardUnitQuery.TableNoTracking.Where(h => h.ItemId == item.itemId).OrderBy(h => h.ConversionFactor).ToList();
                    profitParameter.Invoices = invoices;
                    profitParameter.ItemDataDTO = itemCard;
                    profitParameter.lastDataDTO = lastinvoiceEdit;
                    hamada = 4;
                    //calculte cost regular items
                    List<InvoicesData> Updatedinvoices = pcs.calculateItemsProfite(profitParameter);
                    hamada = 5;
                    //the last step update invoices after calculate cost for them
                    if (Updatedinvoices.Count > 0)
                    {
                        bool isupdated = await UpdateInvoiceData(Updatedinvoices);
                    }
                    hamada = 6;
                    await _hub.Clients.Clients(usersSignalRConnectionId).SendAsync(defultData.ProfitProgress, new porgressData { percentage = Percentage(counter, ReqularItems.Count), Count = counter, totalCount = ReqularItems.Count, status = Aliases.ProgressStatus.InProgress });

                }

                //calculat composte item cost and update its invoices
                if (compositeItem != null && compositeItem.Count > 0)
                {
                    bool isCalculated = await CalcualateCompositeItemProfit(compositeItem); // Calculate composite items

                }
                hamada = 7;
                //update journalEnery for sales cost 


                if (ItemData.Count > 0)
                {
                    // فى مشكله هنا تقريبا
                    await UpdateJournalEntery(ItemData, currentCompanyLogin);
                    hamada = 8;
                    editedItemsCommand.RemoveRange(ItemData);
                    await editedItemsCommand.SaveAsync();
                }
                hamada = 8;
                //end signalr and clear cashe
                if (usersSignalRConnectionId != null)
                {

                    await _hub.Clients.Clients(usersSignalRConnectionId).SendAsync(defultData.ProfitProgress, new porgressData { percentage = 100, Count = ReqularItems.Count, totalCount = ReqularItems.Count, status = Aliases.ProgressStatus.ProgressFinshed });
                }
                _cashHelper.DeleteSignalRCahedRecored(null, defultData.ProfitKey, userData.companyLogin);

                return new ResponseResult() { Result = Result.Success, ErrorMessageAr = " Done ", Note = usersSignalRConnectionId.First() };

            }
            catch (Exception e)
            {
                if (usersSignalRConnectionId != null)
                {
                    await _hub.Clients.Clients(usersSignalRConnectionId).SendAsync(defultData.ProfitProgress, new porgressData { status = Aliases.ProgressStatus.ProgressFalid });
                    _cashHelper.DeleteSignalRCahedRecored(null, defultData.ProfitKey, userData.companyLogin);
                }

                return new ResponseResult() { Result = Result.Failed, ErrorMessageAr = e.Message, ErrorMessageEn = e.Message, Note = hamada.ToString() };


            }

        }
        private double Percentage(int count, double totalCount)
        {
            double percentage = 1;
            if (totalCount != 0)
            {
                percentage = ((count / totalCount) * 100);
            }
            return percentage;
        }
        private async Task UpdateJournalEntery(List<EditedItems> itemData, string currentCompanyLogin)
        {
            List<JEnteryInvoicedata> invoices = GetInvoiceDataForJEntery(itemData.Select(a => a.itemId).ToList(), itemData.Min(h => h.serialize));
            int counter = 0;
            string[] usersSignalRConnectionId = null;
            foreach (var invoice in invoices)
            {
                counter++;
                usersSignalRConnectionId = _signalRQuery.TableNoTracking.Select(x => x.connectionId).ToArray();
                await SetJournalEntery(invoice);
                await _hub.Clients.Clients(usersSignalRConnectionId).SendAsync(defultData.ProfitProgress, new porgressData { Notes = "Update Invoices by Profit Data", percentage = Percentage(counter, invoices.Count), Count = counter, totalCount = invoices.Count, status = Aliases.ProgressStatus.InProgress });
            }
        }

        private async Task<bool> CalcualateCompositeItemProfit(List<CompositeData> compositeItem)
        {
            foreach (CompositeData item in compositeItem)
            {
                List<CompositeForDataProfit> componentItems = GetAllCompositeComponent(item.itemId);
                var InvoiceData = GetInvoiceData(new List<int> { item.itemId }, item.size, item.serialize, item.branchId);//get all invoices for each composite item 

                await CalculatecompostCost(componentItems, InvoiceData, item.branchId); // calculate cost after each invoice 
            }
            return true;
        }

        private async Task CalculatecompostCost(List<CompositeForDataProfit> componentItems, List<InvoicesData> invoicesData, int BranchId)
        {
            List<InvoicesData> NewInvoicesData = new List<InvoicesData>();
            foreach (var invoice in invoicesData)
            {
                var ComponentsCost = GetLastCostOfAllComponent(componentItems, invoice.Serialize, BranchId);//get all last cost of each component of this composite item
                invoice.Cost = ComponentsCost.Sum(h => (h.Cost * h.Quantity * h.Factor));
                bool isupdated = await UpdateInvoiceData(new List<InvoicesData> { invoice });
            }
        }
        //last cost for composit items
        private List<CompositeForDataProfit> GetLastCostOfAllComponent(List<CompositeForDataProfit> componentItems, double serialize, int BranchId)
        {
            List<CompositeForDataProfit> ComponentItemsCost = componentItems.ToList();
            foreach (var item in ComponentItemsCost)
            {
                var Itemcost = GetInvoicesCost(item.PartId, 0, serialize, BranchId);
                item.Cost = Itemcost.Cost;
                item.AvgPrice = item.AvgPrice;
                #region MyRegion
                //CompositeForDataProfit componentItemCost = new CompositeForDataProfit
                //{
                //    PartId = item.PartId,
                //    ItemId = item.ItemId,
                //    AvgPrice = Itemcost.AvgPrice,
                //    Cost = Itemcost.Cost,
                //    Quantity = item.Quantity,
                //    UnitId = item.UnitId,

                //};
                //ComponentItemsCost.Add(componentItemCost);
                #endregion

            }
            return ComponentItemsCost;
        }

        private List<CompositeForDataProfit> GetAllCompositeComponent(int itemId)
        {
            List<CompositeForDataProfit> itemsCompositeData = new List<CompositeForDataProfit>();
            var items = InvStpItemCardPartsQuery.TableNoTracking.Where(h => h.ItemId == itemId).ToList();
            var itemunits = InvStpItemCardUnitsQuery.TableNoTracking.Where(h => items.Select(a => a.PartId).Contains(h.ItemId));
            foreach (var item in items)
            {
                itemsCompositeData.Add(new CompositeForDataProfit
                {
                    PartId = item.PartId,
                    Quantity = item.Quantity,
                    UnitId = item.UnitId,
                    ItemId = item.ItemId,
                    Factor = itemunits.Where(a => a.UnitId == item.UnitId).Select(a => a.ConversionFactor).FirstOrDefault()

                });


            }
            return itemsCompositeData;
        }

        private List<CompositeData> GetCompositeFromItsComponent(int item, double serialize, int BranchId, List<CompositeData> listOfCompositItem)
        {
            // need check again 
            var items = InvStpItemCardPartsQuery.TableNoTracking.Where(h => h.PartId == item)
                .Select(h => new CompositeData { itemId = h.ItemId, serialize = serialize, size = 0, branchId = BranchId })
                .ToList();
            foreach (var obj in items)
            {
                if (!listOfCompositItem.Where(h => h.itemId == obj.itemId && h.size == obj.size && h.branchId == BranchId).Any())
                {
                    listOfCompositItem.Add(obj);
                }
            }
            return listOfCompositItem;
        }
        //update invoice with new cost
        private async Task<bool> UpdateInvoiceData(List<InvoicesData> updatedinvoices)
        {
            var invoices = await invoiceDetailsQuery.FindByAsyn((a => updatedinvoices.Select(h => h.InvoiceId).Contains(a.InvoiceId)
                                                                  && a.ItemId == updatedinvoices.FirstOrDefault().ItemId
                                                                  && a.SizeId == (updatedinvoices.FirstOrDefault().SizeId == 0 ? null : updatedinvoices.FirstOrDefault().SizeId)));
            foreach (var item in invoices)
            {
                item.Cost = updatedinvoices.Where(a => a.InvoiceId == item.InvoiceId).OrderByDescending(a => a.ItemIndex).Select(a => a.Cost).FirstOrDefault();
                item.AvgPrice = updatedinvoices.Where(a => a.InvoiceId == item.InvoiceId).OrderByDescending(a => a.ItemIndex).Select(a => a.AvgPrice).FirstOrDefault();
                item.ConversionFactor = updatedinvoices.Where(a => a.InvoiceId == item.InvoiceId && a.ItemIndex == item.indexOfItem).Select(a => a.factor).FirstOrDefault();
            }
            return await InvoiceDetailsCommand.UpdateAsyn(invoices);
            //return await InvoiceDetailsCommand.SaveChanges() == 1;
        }
        //get last invoice
        private lastInvoiceEditedDTO Getlastinvoice(EditedItems item)
        {
            #region
            //var Lastinvoices1 = invoiceDetailsQuery.TableNoTracking.Include(a=>a.InvoicesMaster)
            //    .Where(h => h.ItemId == item.itemId && h.SizeId == (item.sizeId == 0 ? null : item.sizeId) && h. InvoicesMaster.Serialize < item.serialize)                           
            //    //.Select(a=>new { Signal = a.Signal, Cost = a.Cost, AvgPrice = a.AvgPrice, invoiceId = a.InvoiceId, Quantity = a.Quantity })
            //  .OrderByDescending(a =>a.InvoiceId) 
            //  .GroupBy(a=>1)
            //    .Select(h => new lastInvoiceEditedDTO
            //    {
            //        QTyOfPurchase = h.FirstOrDefault().Signal > 0 ? h.Sum(a => (a.Signal * a.Quantity)) : 0,
            //        Cost = h.FirstOrDefault().Cost,
            //        AvgPrice = h.FirstOrDefault().AvgPrice,
            //        invoiceId= h.FirstOrDefault().InvoiceId,    
            //        LastQTY = h.Sum(a => (a.Signal * a.Quantity))
            //    });
            #endregion
            //var AllLastinvoicestest = invoiceDetailsQuery.TableNoTracking

            //      .Where(h => h.ItemId == item.itemId
            //      && h.InvoicesMaster.BranchId == item.BranchID
            //      && h.SizeId == (item.sizeId == 0 ? null : item.sizeId)
            //      && h.InvoicesMaster.Serialize < item.serialize)
            //      ;
            //factor how know if purchase or not 
            lastInvoiceEditedDTO AllLastinvoices = invoiceDetailsQuery.TableNoTracking

                .Where(h =>
                   h.ItemId == item.itemId
                && h.Quantity > 0
                && h.InvoicesMaster.BranchId == item.BranchID
                && h.SizeId == (item.sizeId == 0 ? null : item.sizeId)
                && h.InvoicesMaster.Serialize < item.serialize)
                .GroupBy(a => 1)
                .Select(h => new lastInvoiceEditedDTO
                {
                    QTyOfPurchase = h.Sum(a => (a.Signal > 0 && a.InvoicesMaster.IsDeleted == false) ? (a.Signal * a.Quantity) : 0),
                    LastQTY = h.Sum(a => (a.Signal * a.Quantity * a.ConversionFactor))// ( a.Units.CardUnits==null ? 1: a.Units.CardUnits.Select(h=>h.ConversionFactor).FirstOrDefault())))
                })
                .FirstOrDefault();
            SelectionData Lastinvoices = GetInvoicesCost(item.itemId, item.sizeId, item.serialize, item.BranchID);


            if (AllLastinvoices != null && Lastinvoices != null)
            {
                AllLastinvoices.Cost = Lastinvoices.Cost;
                AllLastinvoices.AvgPrice = Lastinvoices.AvgPrice;
                AllLastinvoices.invoiceId = Lastinvoices.invoiceId;
            }
            //var objectQuery1 = (AllLastinvoices).ToQueryString();
            //var objectQuery = (Lastinvoices).ToQueryString();

            return AllLastinvoices ?? new lastInvoiceEditedDTO() { Cost = 0, AvgPrice = 0 };
        }

        private SelectionData GetInvoicesCost(int itemId, int sizeId, double serialize, int branchId)
        {

            SelectionData Lastinvoices = invoiceDetailsQuery.TableNoTracking
                .Where(h => h.ItemId == itemId
                && h.SizeId == (sizeId == 0 ? null : sizeId)
                && h.InvoicesMaster.Serialize < serialize
                && h.InvoicesMaster.IsDeleted == false
                && h.Quantity > 0
                && h.InvoicesMaster.BranchId == branchId)
                .OrderByDescending(a => a.InvoicesMaster.Serialize)
                .Select(h => new SelectionData
                {
                    Cost = h.Cost,
                    AvgPrice = h.AvgPrice,
                    invoiceId = h.InvoiceId,
                })
                .FirstOrDefault();

            return Lastinvoices ?? new SelectionData() { Cost = 0, AvgPrice = 0 };
        }

        public async Task<bool> SetJournalEntery(JEnteryInvoicedata invoice)
        {
            try
            {


                var journalentryID = await jounralEnteryQuery.TableNoTracking.Where(q => q.InvoiceId == invoice.invoiceId).Select(a => a.Id).FirstOrDefaultAsync();
                if (journalentryID == 0)
                    return false;
                await journalEntryDetailsCommand.DeleteAsync(a => a.JournalEntryId == journalentryID && a.isCostSales == true);

                List<GLJournalEntryDetails> journalEntryDetails = new List<GLJournalEntryDetails>();

                journalEntryDetails.Add(new GLJournalEntryDetails()//add the main data of journal entery
                {
                    JournalEntryId = journalentryID,
                    FinancialAccountId = await GLPurchasesAndSalesSettingsQuary.TableNoTracking
                                          .Where(h => h.RecieptsType == (int)DocumentType.Purchase && h.branchId == userData.CurrentbranchId)
                                          .Select(a => a.FinancialAccountId)
                                          .FirstOrDefaultAsync(),// المشتريات 
                    Credit = Lists.returnSalesInvoiceList.Contains(invoice.invoiceType) ? 0 : invoice.cost, // total cost
                    Debit = !Lists.returnSalesInvoiceList.Contains(invoice.invoiceType) ? 0 : invoice.cost,
                    DescriptionAr = purAndSalesSettingNames.SalesCostAr,
                    DescriptionEn = purAndSalesSettingNames.SalesCostEn,
                    isCostSales = true

                });
                journalEntryDetails.Add(new GLJournalEntryDetails()//add the main data of journal entery
                {
                    JournalEntryId = journalentryID,
                    FinancialAccountId = await GLPurchasesAndSalesSettingsQuary.TableNoTracking.Where(h => h.RecieptsType == (int)DocumentType.Inventory && h.branchId == userData.CurrentbranchId).Select(a => a.FinancialAccountId).FirstOrDefaultAsync(),// تكلفه المبيعات  
                    Debit = Lists.returnSalesInvoiceList.Contains(invoice.invoiceType) ? 0 : invoice.cost, // total cost
                    Credit = !Lists.returnSalesInvoiceList.Contains(invoice.invoiceType) ? 0 : invoice.cost,
                    DescriptionAr = purAndSalesSettingNames.SalesCostAr,
                    DescriptionEn = purAndSalesSettingNames.SalesCostEn,
                    isCostSales = true

                });

                journalEntryDetailsCommand.AddRange(journalEntryDetails);

                var saved = await journalEntryDetailsCommand.SaveAsync();

                return saved;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public List<InvoicesData> GetInvoiceData(List<int> itemId, int? sizeId, double serialize, int BranchId)
        {
            //need refactor

            var _invoices = invoiceDetailsQuery.TableNoTracking
                   .Include(h => h.Units)
                   .ThenInclude(a => a.CardUnits)
                   .Include(c=> c.InvoicesMaster)
                   .Where(a => itemId.Contains(a.ItemId)
                   && a.InvoicesMaster.BranchId == BranchId
                   && a.ItemTypeId != (int)ItemTypes.Note
                   && a.Quantity > 0
                   && (sizeId == null ? 1 == 1 : (a.SizeId == (sizeId == 0 ? null : sizeId)))
                   && a.InvoicesMaster.Serialize >= serialize
                   && a.InvoicesMaster.IsDeleted == false)
                   .OrderBy(a => a.InvoicesMaster.Serialize)
                    .Select(h => new InvoicesData
                    {
                        InvoiceId = h.InvoiceId,
                        Serialize = h.InvoicesMaster.Serialize,
                        ItemId = h.ItemId,
                        factor = (h.Units.CardUnits.Where(c => c.UnitId == h.UnitId && c.ItemId == h.ItemId).Max(a => a.ConversionFactor)),
                        //Price = (h.TotalWithSplitedDiscount / h.Quantity) + h.OtherAdditions,//add addition price 
                        Price =  h.InvoicesMaster.InvoiceTypeId != (int)DocumentType.ReturnPurchase ? ((h.TotalWithSplitedDiscount + h.OtherAdditions) / h.Quantity): ((h.TotalWithSplitedDiscount - h.OtherAdditions) / h.Quantity),//add addition price 
                        AvgPrice = h.AvgPrice,
                        Cost = h.Cost,
                        QTY = (h.Quantity * h.Signal),
                        ItemIndex = h.indexOfItem,
                        InvoiceType = h.InvoicesMaster.InvoiceTypeId,
                        SizeId = h.SizeId,
                        PriceWithVate = (h.InvoicesMaster.PriceWithVat),
                        VatRatio = h.VatRatio,
                    }).ToList();

            return _invoices;

        }


        public List<JEnteryInvoicedata> GetInvoiceDataForJEntery(List<int> itemIds, double serialize)
        {
            //need branches
            var invoices = invoiceMasterQuery.TableNoTracking
                .Include(h => h.InvoicesDetails)
                .Where(h => h.InvoicesDetails.Where(w => itemIds.Contains(w.ItemId)).Select(a => a.InvoiceId).Contains(h.InvoiceId) && h.Serialize >= serialize && h.IsDeleted == false)
                .Where(h => Lists.SalesTransaction.Contains(h.InvoiceTypeId))
                .OrderBy(a => a.Serialize)
                .Select(h => new JEnteryInvoicedata
                {
                    invoiceId = h.InvoiceId,
                    cost = roundNumbers.GetRoundNumber(h.InvoicesDetails.Sum(a => a.Cost * a.Quantity * a.ConversionFactor)),
                    serial = h.Serialize,
                    invoiceType = h.InvoiceTypeId
                }).ToList();



            return invoices;

        }
    }
}
