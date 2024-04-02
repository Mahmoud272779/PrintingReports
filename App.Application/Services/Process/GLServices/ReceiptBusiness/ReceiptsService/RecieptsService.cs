using App.Application.Handlers.GeneralLedger.JournalEntry;
using App.Application.Handlers.Persons;
using App.Application.Services.Printing.InvoicePrint;
using App.Application.Services.Printing.PrintFile;
using App.Domain.Models.Response.Store.Reports.Sales;
using App.Domain.Models.Security.Authentication.Response.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Security.Cryptography.Xml;

namespace App.Application.Services.Process.GLServices.ReceiptBusiness
{
    public class ReceiptsService : IReceiptsService
    {
        private readonly IRepositoryQuery<GlReciepts> receiptQuery;
        private readonly IRepositoryQuery<InvGeneralSettings> generalSettingQuery;
        private readonly IRepositoryCommand<GlReciepts> receiptCommand;
        private readonly IRepositoryCommand<GLRecieptCostCenter> costCenterREcieptCommand;
        private readonly IRepositoryQuery<GLRecieptCostCenter> CostCenterRecieptQuery;
        private readonly IGeneralAPIsService GeneralAPIsService;
        private readonly IFilesOfInvoices filesOfInvoices;
        private readonly IRepositoryQuery<GLBank> bankQuery;
        private readonly IRepositoryQuery<GLSafe> safeQuery;
        private readonly IRepositoryQuery<InvPersons> PersonQuery;
        private readonly IRepositoryQuery<InvSalesMan> SalesManQuery;
        private readonly iAuthorizationService _iAuthorizationService;
        private readonly ISystemHistoryLogsService _systemHistoryLogsService;
        private readonly IRepositoryQuery<GLFinancialAccount> FinancialAccountQuery;
        private readonly IRepositoryQuery<GLOtherAuthorities> OtherAuthorityQuery;
        private readonly IRepositoryQuery<GLJournalEntry> JournalenteryQuery;
        //private readonly IJournalEntryBusiness journalEntryBusiness;
        private readonly IRepositoryCommand<GLJournalEntryDetails> journalEntryDetailsRepositoryCommand;
        private readonly IRepositoryQuery<GLJournalEntryDetails> journalEntryDetailsRepositoryQuery;
        private readonly IRepositoryQuery<GLCurrency> CurrencyQuery;

        private readonly IHistoryReceiptsService ReceiptsHistory;
        private readonly iUserInformation Userinformation;
        private readonly IHttpContextAccessor httpContext;
        // private readonly IHelperService GeneralSetteingCashing;
        private readonly IRoundNumbers RoundNumbers;
        private readonly IPrintService _iprintService;

        private readonly IprintFileService _iPrintFileService;
        private readonly IFilesMangerService _filesMangerService;
        private readonly ICompanyDataService _CompanyDataService;
        private readonly iUserInformation _iUserInformation;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IReportFileService _iReportFileService;
        private readonly IGeneralPrint _iGeneralPrint;
        private readonly IMediator _mediator;

        public ReceiptsService(IRepositoryQuery<GlReciepts> receiptQuery,
             IRepositoryQuery<InvGeneralSettings> generalSettingQuery, IRepositoryCommand<GlReciepts> receiptCommand,
             IRepositoryCommand<GLRecieptCostCenter> costCenterREcieptCommand,
             IRepositoryQuery<GLRecieptCostCenter> costCenterRecieptQuery,
             IGeneralAPIsService GeneralAPIsService, IFilesOfInvoices filesOfInvoices,
             IRepositoryQuery<GLBank> bankQuery, IRepositoryQuery<GLSafe> safeQuery,
             //IJournalEntryBusiness JournalEntryBusiness,
             IRepositoryQuery<InvPersons> personQuery,
             IRepositoryQuery<GLCurrency> currencyQuery,
             IRepositoryQuery<InvSalesMan> salesManQuery,
             iAuthorizationService iAuthorizationService,
             ISystemHistoryLogsService systemHistoryLogsService,
             IRepositoryQuery<GLOtherAuthorities> otherAuthorityQuery,
             IRepositoryQuery<GLFinancialAccount> financialAccountQuery,
             IRepositoryQuery<GLJournalEntry> journalenteryQuery,
             IRepositoryCommand<GLJournalEntryDetails> journalEntryDetailsRepositoryCommand,
             IRepositoryQuery<GLJournalEntryDetails> journalEntryDetailsRepositoryQuery,
             IHistoryReceiptsService receiptsHistory,
             iUserInformation userinformation,
             IHttpContextAccessor ClientHttpContext,
             IHelperService generalSetteing,
             IRoundNumbers roundNumbers,
             IPrintService iprintService,
             IprintFileService iPrintFileService, IFilesMangerService filesMangerService,
             ICompanyDataService CompanyDataService,
             iUserInformation iUserInformation,
             IWebHostEnvironment webHostEnvironment,
             IReportFileService iReportFileService,
             IGeneralPrint iGeneralPrint,
             IMediator mediator)
        {
            this.receiptQuery = receiptQuery;
            this.generalSettingQuery = generalSettingQuery;
            this.receiptCommand = receiptCommand;
            this.costCenterREcieptCommand = costCenterREcieptCommand;
            this.GeneralAPIsService = GeneralAPIsService;
            this.filesOfInvoices = filesOfInvoices;
            this.bankQuery = bankQuery;
            this.safeQuery = safeQuery;
            this.journalEntryDetailsRepositoryCommand = journalEntryDetailsRepositoryCommand;
            this.journalEntryDetailsRepositoryQuery = journalEntryDetailsRepositoryQuery;
            //journalEntryBusiness = JournalEntryBusiness;
            OtherAuthorityQuery = otherAuthorityQuery;
            PersonQuery = personQuery;
            SalesManQuery = salesManQuery;
            _iAuthorizationService = iAuthorizationService;
            _systemHistoryLogsService = systemHistoryLogsService;
            FinancialAccountQuery = financialAccountQuery;
            JournalenteryQuery = journalenteryQuery;
            CostCenterRecieptQuery = costCenterRecieptQuery;
            ReceiptsHistory = receiptsHistory;
            CurrencyQuery = currencyQuery;
            Userinformation = userinformation;

            httpContext = ClientHttpContext;
            // GeneralSetteingCashing = generalSetteing;
            RoundNumbers = roundNumbers;
            _iprintService = iprintService;
            _iPrintFileService = iPrintFileService;
            _filesMangerService = filesMangerService;
            _CompanyDataService = CompanyDataService;
            _iUserInformation = iUserInformation;
            _webHostEnvironment = webHostEnvironment;
            _iReportFileService = iReportFileService;
            _iGeneralPrint = iGeneralPrint;
            _mediator = mediator;
        }
        public int Autocode(int RecieptTypeId, int BranchId)
        {
            var Code = 1;
            Code = receiptQuery.GetMaxCode(e => e.Code, a => a.RecieptTypeId == RecieptTypeId && a.BranchId == BranchId && a.ParentTypeId == null);
            if (Code != null)
                Code++;

            return Code;
        }
        public int AutoCollectioncode(int subtypeID, int BranchId)
        {
            var Code = 1;
            Code = receiptQuery.GetMaxCode(e => e.CollectionCode, a => a.SubTypeId == subtypeID && a.BranchId == BranchId);
            if (Code != null)
                Code++;

            return Code;
        }

        public async Task<ResponseResult> AddReceipt(RecieptsRequest parameter)
        {
            //hamada must make refactor code for this class 
            try
            {
                //var setting = await GeneralSetteingCashing.GetAllGeneralSettings();
                UserInformationModel userInfo = await Userinformation.GetUserInformation();
                //bool isBalance = checkBalancenInCostCenter(parameter.costCenterReciepts, parameter.Amount);
                ValidationData ValidData = receiptsValidattion(parameter, userInfo);
                if (!string.IsNullOrEmpty(ValidData.ErrorMessageEn) || !string.IsNullOrEmpty(ValidData.ErrorMessageAr))
                    return new ResponseResult() { Result = Result.Failed, ErrorMessageAr = ValidData.ErrorMessageAr, ErrorMessageEn = ValidData.ErrorMessageEn };

                if (parameter.Amount <= 0)
                {
                    return new ResponseResult()
                    {
                        Result = Result.Failed,
                        ErrorMessageAr = "يجب ان يكون المبلغ اكبر من الصفر",
                        ErrorMessageEn = "The Amount must be greater than Zero"
                    };

                }
                parameter.BranchId = userInfo.CurrentbranchId;
                parameter.UserId = userInfo.employeeId;
                parameter.Notes = parameter.Notes ?? "";

                double totalCostcenterAmount = 0.0;
                if (parameter.costCenterReciepts != null)
                {
                    totalCostcenterAmount = parameter.costCenterReciepts.Sum(a => a.Number);

                    if (totalCostcenterAmount > parameter.Amount)
                    {
                        return new ResponseResult()
                        {
                            Result = Result.Failed,
                            ErrorMessageAr = "يجب ان يكون مجموع النسب اصغر من او يساوي المبلغ",
                            ErrorMessageEn = "The sum of the percentages must be less than or equal to the amount"
                        };
                    }
                }
                string TransActoin = HistoryActions.Add;
                // active it later
                // var setting = await generalSettingQuery.Get();
                //int decemalNum = setting.Other_Decimals;
                var vatValue = 0.0;
                //if (setting.Vat_Active && parameter.IsIncludeVat)
                //{
                //    vatValue = setting.Vat_DefaultValue;
                //}

                parameter.Amount = RoundNumbers.GetDefultRoundNumber(parameter.Amount);  // Numbers.Roundedvalues(parameter.Amount, decemalNum);
                var MappedData = new GlReciepts();
                Mapping.Mapper.Map(parameter, MappedData);

                financialData financialForBenfiteUser = await getFinantialAccIdForAuthorty(parameter.Authority, parameter.BenefitId, MappedData);
                if (financialForBenfiteUser == null)
                {
                    return new ResponseResult()
                    {
                        Result = Result.Failed,
                        ErrorMessageAr = " لا يوجد هذاالمستفيد   ",
                        ErrorMessageEn = "that Benfit is not have financial account "
                    };
                }
                if ((financialForBenfiteUser == null || financialForBenfiteUser.financialId == 0) && (parameter.ParentId == null || parameter.ParentId == 0))
                {
                    return new ResponseResult()
                    {
                        Result = Result.Failed,
                        ErrorMessageAr = "المستفيد ليس له حساب مالى ",
                        ErrorMessageEn = "that Benfit is not have financial account "
                    };
                }

                //check who is Authority 
                var data = financialForBenfiteUser.ReceiptsData;
                data.Signal = GeneralAPIsService.GetSignal(parameter.RecieptTypeId);
                var directory = "";
                // لازم يتعدل return type of SetRecieptTypeAndDirectoryAndNotes()
                var Set_RecieptType = SetRecieptTypeAndDirectoryAndNotes(parameter.RecieptTypeId, parameter.ParentTypeId);
                // manual
                if (parameter.ParentId == null)
                {
                    data.Code = Autocode(parameter.RecieptTypeId, parameter.BranchId);
                    data.IsAccredit = true;
                    directory = Set_RecieptType.Item2;
                    data.Serialize = Convert.ToDouble(GeneralAPIsService.CreateSerializeOfInvoice(parameter.RecieptTypeId, 0, parameter.BranchId).ToString());
                    data.RecieptType = parameter.BranchId + "-" + Set_RecieptType.Item1 + "-" + data.Code;
                    data.RectypeWithPayment = data.RecieptType + "-" + data.PaymentMethodId;
                    data.Creditor = data.Signal > 0 ? data.Amount : 0;
                    data.Debtor = data.Signal < 0 ? data.Amount : 0;
                }
                else
                {
                    TransActoin = HistoryActions.Accredit;
                    //data.RecieptTypeId = data.ParentTypeId.;
                    data.RecieptType = parameter.ParentType;
                    data.RectypeWithPayment = parameter.ParentType + "-" + data.PaymentMethodId;
                    if (parameter.Deferre && parameter.fromInvoice)
                        data.RectypeWithPayment = parameter.ParentType + "--1";
                    data.PaperNumber = parameter.ParentType;
                    data.Creditor = data.Debtor = parameter.Amount;
                    if (parameter.Deferre || parameter.ParentTypeId == (int)DocumentType.AcquiredDiscount || parameter.ParentTypeId == (int)DocumentType.PermittedDiscount)
                    {
                        data.Creditor = data.Signal < 0 ? data.Amount : 0;
                        data.Debtor = data.Signal > 0 ? data.Amount : 0;
                    }


                }
                // for collection receipts
                if (parameter.subTypeId > 0)
                {
                    TransActoin = HistoryActions.Add;
                    data.SubTypeId = parameter.subTypeId;
                    data.CollectionCode = AutoCollectioncode(parameter.subTypeId, parameter.BranchId);
                    data.CollectionMainCode = parameter.CollectionMainCode;
                    data.IsAccredit = true;
                    data.Creditor = data.Signal > 0 ? data.Amount : 0;
                    data.Debtor = data.Signal < 0 ? data.Amount : 0;
                }


                data.RecieptDate = GeneralAPIsService.serverDate(parameter.RecieptDate);
                data.CreationDate = GeneralAPIsService.serverDate(parameter.RecieptDate);
                data.NoteAR = string.Concat(Set_RecieptType.Item3, " _ ", data.RecieptType);
                data.NoteEN = Set_RecieptType.Item4 + " _ " + data.RecieptType;


                var saved = await receiptCommand.AddAsync(data);
                // var saved = await receiptCommand.SaveAsync();

                if (saved)
                {
                    if (parameter.ParentId == null)
                        saved = GeneralAPIsService.addSerialize(data.Serialize, data.Id, parameter.RecieptTypeId, parameter.BranchId);


                    else 
                    {
                        if (parameter.AttachedFile != null)
                            if (parameter.AttachedFile.Count() > 0)
                                saved = await filesOfInvoices.saveFilesOfInvoices(parameter.AttachedFile, parameter.BranchId, directory, data.Id, false, null, true);

                    }
                    //generate journal entry
                    // get financialId of Safe OR Bank
                    int safeOrBank = 0;
                    int? financialIdOfSafeOfBank = parameter.FA_Id;
                    if (parameter.FA_Id == 0)
                    {
                        safeOrBank = ValidData.safeBankId.Value;
                        financialIdOfSafeOfBank = ValidData.financialAccountId;

                    }
                    //discuss with frontend if finantcialid  put with personid
                    if (!parameter.ReceiptOnly)
                        await AddRecieptsInJournalEntry(parameter, data, financialIdOfSafeOfBank, financialForBenfiteUser);


                    ReceiptsHistory.AddReceiptsHistory(
                       data.BranchId, parameter.BenefitId, TransActoin, data.PaymentMethodId,
                     data.UserId, safeOrBank, data.Code,
                       data.RecieptDate, data.Id, data.RecieptType, data.RecieptTypeId
                       , data.Signal, data.IsBlock, data.IsAccredit, data.Serialize,
                       data.Authority, data.Amount, data.SubTypeId,userInfo

                       );
                    SystemActionEnum systemActionEnum = new SystemActionEnum();
                    if (parameter.RecieptTypeId == (int)DocumentType.SafeCash)
                        systemActionEnum = SystemActionEnum.addSafeCashReceipt;
                    else if (parameter.RecieptTypeId == (int)DocumentType.SafePayment)
                        systemActionEnum = SystemActionEnum.addSafePaymentReceipt;
                    else if (parameter.RecieptTypeId == (int)DocumentType.BankPayment)
                        systemActionEnum = SystemActionEnum.addBankPaymentReceipt;
                    else if (parameter.RecieptTypeId == (int)DocumentType.BankCash)
                        systemActionEnum = SystemActionEnum.addBankCashReceipt;

                    if (systemActionEnum != 0 && parameter.ParentId == null)
                        await _systemHistoryLogsService.SystemHistoryLogsService(systemActionEnum);
                }

                return new ResponseResult { Result = (saved ? Result.Success : Result.Failed), Id = data.Id };
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private ValidationData receiptsValidattion(RecieptsRequest parameter, UserInformationModel userInfo)
        {
            var validData = new ValidationData();
            if (parameter.SafeID > 0 && parameter.BankId > 0)
            {
                validData.ErrorMessageAr = "يجب ادخال خزينه او بنك";
                validData.ErrorMessageEn = "The bank or safeId must be entered";
            }
            else if ((parameter.SafeID == null || parameter.SafeID == 0) && (parameter.BankId == null || parameter.BankId == 0) && (int)DocumentType.AcquiredDiscount != parameter.ParentTypeId && (int)DocumentType.PermittedDiscount != parameter.ParentTypeId)
            {
                validData.ErrorMessageAr = "يجب ادخال خزينه او بنك";
                validData.ErrorMessageEn = "The safeId must be entered";
            }


            else if ((parameter.RecieptTypeId == (int)DocumentType.SafeCash || parameter.RecieptTypeId == (int)DocumentType.SafePayment || parameter.RecieptTypeId == (int)DocumentType.SafeFunds))
            {
                var safe = safeQuery.TableNoTracking.Where(h => h.Id == parameter.SafeID && h.BranchId == userInfo.CurrentbranchId);

                if (!safe.Any() && (parameter.ParentId == null || parameter.subTypeId!=0))
                {
                    validData.ErrorMessageAr = "هذه الخزنه ليست موجوده او لا تملك صلاحيه لها  ";
                    validData.ErrorMessageEn = "this Safe not found or not have permission on it ";
                }
                validData.financialAccountId = safe.Select(a => a.FinancialAccountId).SingleOrDefault();
                validData.safeBankId = parameter.SafeID;
            }
            else if (parameter.RecieptTypeId == (int)DocumentType.BankCash || parameter.RecieptTypeId == (int)DocumentType.BankPayment || parameter.RecieptTypeId == (int)DocumentType.BankFunds)
            {
                var bank = bankQuery.TableNoTracking.Where(h => h.Id == parameter.BankId);// && h.BankBranches.Select(a => a.BranchId).Contains(userInfo.CurrentbranchId));  اسامه اللى قال ان مفيش فروع فى البوك
                if (!bank.Any())
                {
                    validData.ErrorMessageAr = "هذا البنك ليست موجوده اولا تملك صلاحيه له  ";
                    validData.ErrorMessageEn = "this Bank not found or not have permission on it ";

                }
                validData.financialAccountId = bank.Select(a => a.FinancialAccountId).SingleOrDefault();
                validData.safeBankId = parameter.BankId;
            }

            if (parameter.Authority == 0)
            {
                validData.ErrorMessageAr = "يجب ادخال اسم الجهه";
                validData.ErrorMessageEn = "The Authority must be entered";
            }






            return validData;
        }

        private async Task<bool> AddRecieptsInJournalEntry(RecieptsRequest parameter, GlReciepts data, int? financialIdOfSafeOfBank, financialData financialForBenfiteUser)
        {
            // journal entiry of safe and bank 

            var costCenterRecieptsList = new List<GLRecieptCostCenter>();

            //need refactor for this code

            AddJournalEntryRequest JEntry = new AddJournalEntryRequest();
            JEntry.BranchId = parameter.BranchId;
            JEntry.FTDate = parameter.RecieptDate;
            JEntry.ReceiptsId = data.Id;
            JEntry.Notes = data.NoteAR;
            JEntry.isAuto = true;
            JEntry.IsAccredit = true;
            if (parameter.ParentId != null && !(parameter.ParentTypeId == (int)DocumentType.PermittedDiscount || parameter.ParentTypeId == (int)DocumentType.AcquiredDiscount))
                JEntry.IsAccredit = false;

            JEntry.JournalEntryDetails.Add(new JournalEntryDetail()
            {
                FinancialAccountId = financialIdOfSafeOfBank,
                Credit = data.Signal < 0 ? data.Amount : 0,
                Debit = data.Signal > 0 ? data.Amount : 0,
                DescriptionAr = data.NoteAR,
                DescriptionEn = data.NoteEN,
            });

            if (parameter.costCenterReciepts != null && parameter.costCenterReciepts.Count > 0)
            {
                double totalCostcenterAmount = parameter.costCenterReciepts.Sum(h => h.Number);
                foreach (var item in parameter.costCenterReciepts)
                {

                    JEntry.JournalEntryDetails.Add(new JournalEntryDetail()
                    {
                        FinancialAccountId = financialForBenfiteUser.financialId,
                        FinancialCode = financialForBenfiteUser.financialCode,
                        FinancialName = financialForBenfiteUser.FinancialName,
                        Credit = data.Signal > 0 ? item.Number : 0,
                        Debit = data.Signal < 0 ? item.Number : 0,
                        CostCenterId = item.CostCenterId,
                        DescriptionAr = data.NoteAR,
                        DescriptionEn = data.NoteEN,

                    });

                    #region save costcenter
                    var center = Mapping.Mapper.Map<CostCenterReciepts, GLRecieptCostCenter>(item);
                    center.SupportId = data.Id;
                    costCenterRecieptsList.Add(center);
                    #endregion

                }
                costCenterREcieptCommand.AddRange(costCenterRecieptsList);
                await costCenterREcieptCommand.SaveChanges();


                if (totalCostcenterAmount < data.Amount)
                {
                    double restAmount = data.Amount - totalCostcenterAmount;
                    JEntry.JournalEntryDetails.Add(new JournalEntryDetail()
                    {
                        FinancialAccountId = financialForBenfiteUser.financialId,
                        FinancialCode = financialForBenfiteUser.financialCode,
                        FinancialName = financialForBenfiteUser.FinancialName,
                        Credit = data.Signal > 0 ? restAmount : 0,
                        Debit = data.Signal < 0 ? restAmount : 0,
                        DescriptionAr = data.NoteAR,
                        DescriptionEn = data.NoteEN,

                    });
                }

            }
            else
                JEntry.JournalEntryDetails.Add(new JournalEntryDetail()
                {
                    FinancialAccountId = financialForBenfiteUser.financialId,
                    FinancialCode = financialForBenfiteUser.financialCode,
                    FinancialName = financialForBenfiteUser.FinancialName,
                    Credit = data.Signal > 0 ? data.Amount : 0,
                    Debit = data.Signal < 0 ? data.Amount : 0,
                    DescriptionAr = data.NoteAR,
                    DescriptionEn = data.NoteEN,
                });


            //var res = await journalEntryBusiness.AddJournalEntry(JEntry);
            var res = await _mediator.Send(JEntry);

            return res.Status == RepositoryActionStatus.Created;


        }

        //over load to set defult data
        public async Task<financialData> getFinantialAccIdForAuthorty(int type, int ID)
        {
            return await getFinantialAccIdForAuthorty(type, ID, new GlReciepts());
        }
        public async Task<financialData> getFinantialAccIdForAuthorty(int type, int ID, GlReciepts RData)
        {
            financialData FA = new financialData();
            if (type == AuthorityTypes.customers || type == AuthorityTypes.suppliers)
            {
                FA = await PersonQuery.TableNoTracking.Where(a => a.Id == ID && (type == AuthorityTypes.customers ? a.IsCustomer == true : a.IsSupplier == true))
                            .Include(h => h.FinancialAccount)
                            .Select(s => new financialData()
                            {
                                financialId = s.FinancialAccountId.GetValueOrDefault(0),
                                financialCode = s.FinancialAccount.AccountCode,
                                FinancialName = s.FinancialAccount.ArabicName
                            }).FirstOrDefaultAsync();

                RData.PersonId = ID;
                RData.OtherAuthorityId = null;
                RData.SalesManId = null;
                RData.FinancialAccountId = null;

            }
            else if (type == AuthorityTypes.other)
            {
                FA = await OtherAuthorityQuery.TableNoTracking.Where(a => a.Id == ID).Include(h => h.FinancialAccount)
                                    .Select(s => new financialData()
                                    {
                                        financialId = s.FinancialAccountId.GetValueOrDefault(0),
                                        financialCode = s.FinancialAccount.AccountCode,
                                        FinancialName = s.FinancialAccount.ArabicName
                                    }).FirstOrDefaultAsync();
                RData.PersonId = null;
                RData.OtherAuthorityId = ID;
                RData.SalesManId = null;
                RData.FinancialAccountId = null;

            }
            else if (type == AuthorityTypes.salesman)
            {
                FA = await SalesManQuery.TableNoTracking.Where(a => a.Id == ID).Include(h => h.FinancialAccount)
                                    .Select(s => new financialData()
                                    {
                                        financialId = s.FinancialAccountId.GetValueOrDefault(0),
                                        financialCode = s.FinancialAccount.AccountCode,
                                        FinancialName = s.FinancialAccount.ArabicName
                                    }).FirstOrDefaultAsync();
                RData.SalesManId = ID;
                RData.PersonId = null;
                RData.OtherAuthorityId = null;

                RData.FinancialAccountId = null;
            }
            else if (type == AuthorityTypes.DirectAccounts)
            {
                FA = await FinancialAccountQuery.TableNoTracking.Where(h => h.Id == ID).Select(s => new financialData()
                {
                    financialId = s.Id,
                    financialCode = s.AccountCode,
                    FinancialName = s.ArabicName
                }).FirstOrDefaultAsync();
                RData.FinancialAccountId = ID;
                RData.PersonId = null;
                RData.OtherAuthorityId = null;
                RData.SalesManId = null;

            }
            if (FA != null)
                FA.ReceiptsData = RData;
            return FA;
        }
        // غير مستخدم
        public Tuple<string, string> SetNoteOfParentReciepts(int parentTypeId)
        {
            string noteAr = "";
            string noteEn = "";
            if (parentTypeId == (int)RecieptsParentType.AcquiredDiscount)
            {
                noteAr = NotesOfReciepts.AcquiredDiscountAr;
                noteEn = NotesOfReciepts.AcquiredDiscountEN;
            }
            else if (parentTypeId == (int)RecieptsParentType.PermittedDiscount)
            {
                noteAr = NotesOfReciepts.PermittedDiscountAR;
                noteEn = NotesOfReciepts.PermittedDiscountEn;
            }
            else if (parentTypeId == (int)RecieptsParentType.Purchase)
            {
                noteAr = NotesOfReciepts.PurchaseAr;
                noteEn = NotesOfReciepts.PurchaseEn;
            }
            else if (parentTypeId == (int)RecieptsParentType.ReturnPurchase)
            {
                noteAr = NotesOfReciepts.ReturnPurchaseAr;
                noteEn = NotesOfReciepts.ReturnPurchaseEn;
            }
            else if (parentTypeId == (int)RecieptsParentType.Sales)
            {
                noteAr = NotesOfReciepts.SalesAr;
                noteEn = NotesOfReciepts.SalesEn;
            }
            else if (parentTypeId == (int)RecieptsParentType.ReturnSales)
            {
                noteAr = NotesOfReciepts.ReturnSalesAr;
                noteEn = NotesOfReciepts.ReturnSalesEn;
            }
            else if (parentTypeId == (int)RecieptsParentType.POS)
            {
                noteAr = NotesOfReciepts.PosAr;
                noteEn = NotesOfReciepts.PosEn;
            }
            else if (parentTypeId == (int)RecieptsParentType.ReturnPOS)
            {
                noteAr = NotesOfReciepts.ReturnPosAr;
                noteEn = NotesOfReciepts.ReturnPosEn;
            }
            else if (parentTypeId == (int)RecieptsParentType.commissionsPayment)
            {
                noteAr = NotesOfReciepts.commissionsPaymentAr;
                noteEn = NotesOfReciepts.commissionsPaymentEn;
            }
            else if (parentTypeId == (int)RecieptsParentType.SafesFunds)
            {
                noteAr = NotesOfReciepts.SafeFundsAr;
                noteEn = NotesOfReciepts.SafeFundsEn;
            }
            else if (parentTypeId == (int)RecieptsParentType.BanksFunds)
            {
                noteAr = NotesOfReciepts.BankFundsAr;
                noteEn = NotesOfReciepts.BankFundsEn;
            }
            else if (parentTypeId == (int)RecieptsParentType.CustomersFunds)
            {
                noteAr = NotesOfReciepts.CustomerFundsAr;
                noteEn = NotesOfReciepts.CustomerFundsEn;
            }
            else if (parentTypeId == (int)RecieptsParentType.SuppliersFunds)
            {
                noteAr = NotesOfReciepts.SupplierFundsAr;
                noteEn = NotesOfReciepts.SupplierFundsEn;
            }

            return new Tuple<string, string>(noteAr, noteEn);
        }

        public Tuple<string, string, string, string> SetRecieptTypeAndDirectoryAndNotes(int recieptTypeId, int? parentTypeId)
        {
            var recieptType = "";
            var Directory = "";
            var NoteAr = "";
            var NoteEn = "";
            if (recieptTypeId == (int)DocumentType.SafeCash)
            {
                recieptType = InvoicesCode.SafeCash;
                Directory = FilesDirectories.SafeCash;
                NoteAr = NotesOfReciepts.SafeCashAr;
                NoteEn = NotesOfReciepts.SafeCashEn;
            }
            else if (recieptTypeId == (int)DocumentType.SafePayment)
            {
                recieptType = InvoicesCode.SafePayment;
                Directory = FilesDirectories.SafePayment;
                NoteAr = NotesOfReciepts.SafePaymentAr;
                NoteEn = NotesOfReciepts.SafePaymentEn;
            }
            else if (recieptTypeId == (int)DocumentType.BankCash)
            {
                recieptType = InvoicesCode.BankCash;
                Directory = FilesDirectories.BankCash;
                NoteAr = NotesOfReciepts.BankCashAr;
                NoteEn = NotesOfReciepts.BankCashEn;
            }
            else if (recieptTypeId == (int)DocumentType.BankPayment)
            {
                recieptType = InvoicesCode.BankPayment;
                Directory = FilesDirectories.BankPayment;
                NoteAr = NotesOfReciepts.BankPaymentAr;
                NoteEn = NotesOfReciepts.BankPaymentEn;
            }
            else if (parentTypeId == (int)DocumentType.AcquiredDiscount)
            {
                //recieptType = InvoicesCode.BankPayment;
                // Directory = FilesDirectories.BankPayment;
                NoteAr = NotesOfReciepts.AcquiredDiscountAr;
                NoteEn = NotesOfReciepts.AcquiredDiscountEN;
            }
            else if (parentTypeId == (int)DocumentType.PermittedDiscount)
            {
                //recieptType = InvoicesCode.BankPayment;
                //Directory = FilesDirectories.BankPayment;
                NoteAr = NotesOfReciepts.PermittedDiscountAR;
                NoteEn = NotesOfReciepts.PermittedDiscountEn;
            }


            return new Tuple<string, string, string, string>(recieptType, Directory, NoteAr, NoteEn);

        }

        public async Task<ResponseResult> GetReceiptById(int ReceiptId, int RecieptsType, bool isPrint)
        {
            UserInformationModel userInfo = await Userinformation.GetUserInformation();
            if (userInfo == null)
                return new ResponseResult() { Data = null, Result = Result.NoDataFound, ErrorMessageAr = "no taken request", ErrorMessageEn = "no taken request" };
            //generalSetting

            // var setting = await GeneralSetteingCashing.GetAllGeneralSettings();
            //   int decemalNum = setting.Other_Decimals;

            if (ReceiptId == 0 || RecieptsType == 0)
                return new ResponseResult() { Data = null, Result = Result.NoDataFound };

            //&& a.UserId == userInfo.employeeId

            var data = receiptQuery.TableNoTracking
                .Where(a => a.Id == ReceiptId && a.RecieptTypeId == RecieptsType && a.BranchId == userInfo.CurrentbranchId && a.IsAccredit == true && a.Deferre == false)
                .Include(h => h.RecieptsFiles)
                .Include(h => h.RecieptsCostCenters)
                .Include(h => h.FinancialAccount)
                .Include(h => h.OtherAuthorities)
                .Include(h => h.person)
                .Include(h => h.SalesMan)
                .Include(h => h.Banks)
                .Include(h => h.PaymentMethods)
                .Include(h => h.Safes).ToList();



            var finalData = data.FirstOrDefault();

            GlRecieptsResopnseDTO receiptsData = new GlRecieptsResopnseDTO();
            var dataMapped = Mapping.Mapper.Map(finalData, receiptsData);

            if (dataMapped == null)
                return new ResponseResult() { Data = null, Result = Result.NoDataFound };

            dataMapped.RecieptsCostCenter.AddRange(finalData.RecieptsCostCenters
                .Select(h => new GLRecieptCostCenter
                {
                    Id = h.Id,
                    CostCenteCode = h.CostCenteCode,
                    CostCenteName = h.CostCenteName,
                    Percentage = h.Percentage,
                    Number = RoundNumbers.GetRoundNumber(h.Number),//Numbers.Roundedvalues(h.Number, decemalNum),
                    SupportId = h.SupportId,
                    CostCenter = h.CostCenter,
                    CostCenterId = h.CostCenterId
                }
                    ));
            dataMapped.Amount = RoundNumbers.GetRoundNumber(dataMapped.Amount);//Numbers.Roundedvalues(dataMapped.Amount, decemalNum);
            dataMapped.PaymentMethodNameArabic = finalData.PaymentMethods.ArabicName;
            dataMapped.PaymentMethodNameLatin = finalData.PaymentMethods.LatinName;
            dataMapped.authorityNameArabic = Lists.receiptsAuthorities.Where(h => h.Id == dataMapped.Authority).Select(h => h.arabicName).FirstOrDefault("");
            dataMapped.authorityNameLatin = Lists.receiptsAuthorities.Where(h => h.Id == dataMapped.Authority).Select(h => h.latinName).FirstOrDefault("");
            dataMapped.RecieptType = finalData.RecieptType;

            if (finalData.RecieptTypeId == (int)DocumentType.BankCash || finalData.RecieptTypeId == (int)DocumentType.BankPayment)
            {
                dataMapped.safeOrBankNameID = finalData.Banks.Id;
                dataMapped.safeOrBankNameArabic = finalData.Banks.ArabicName;
                dataMapped.safeOrBankNameLatin = finalData.Banks.LatinName;
            }
            if (finalData.RecieptTypeId == (int)DocumentType.SafeCash || finalData.RecieptTypeId == (int)DocumentType.SafePayment)
            {
                dataMapped.safeOrBankNameID = finalData.Safes.Id;
                dataMapped.safeOrBankNameArabic = finalData.Safes.ArabicName;
                dataMapped.safeOrBankNameLatin = finalData.Safes.LatinName;
            }
            //فى حاله لو هو مندوب مبيعات 
            if (finalData.Authority == AuthorityTypes.salesman)
            {

                dataMapped.BenefitId = finalData.SalesMan.Id;
                dataMapped.benefitCode = finalData.SalesMan.Code;
                dataMapped.benefitNameArabic = finalData.SalesMan.ArabicName;
                dataMapped.benefitNameLatin = finalData.SalesMan.LatinName;
            }
            //لو الجهه حسابات عامه
            else if (finalData.Authority == AuthorityTypes.DirectAccounts)
            {
                try
                {
                    dataMapped.BenefitId = finalData.FinancialAccount.Id;
                    dataMapped.benefitCode = long.Parse(finalData.FinancialAccount.AccountCode.Replace(".", string.Empty));
                    dataMapped.benefitNameArabic = finalData.FinancialAccount.ArabicName;
                    dataMapped.benefitNameLatin = finalData.FinancialAccount.LatinName;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            // لو الجهه عميل او مورد 
            else if (finalData.Authority == AuthorityTypes.customers || finalData.Authority == AuthorityTypes.suppliers)
            {
                dataMapped.BenefitId = finalData.person.Id;
                dataMapped.benefitCode = finalData.person.Code;
                dataMapped.benefitNameArabic = finalData.person.ArabicName;
                dataMapped.benefitNameLatin = finalData.person.LatinName;
            }
            //لو الجهه جهات اخرى
            else if (finalData.Authority == AuthorityTypes.other)
            {
                dataMapped.BenefitId = finalData.OtherAuthorities.Id;
                dataMapped.benefitCode = finalData.OtherAuthorities.Code;
                dataMapped.benefitNameArabic = finalData.OtherAuthorities.ArabicName;
                dataMapped.benefitNameLatin = finalData.OtherAuthorities.LatinName;
            }






            var total = GetReceiptBalanceForBenifit(dataMapped.Authority, dataMapped.BenefitId).Result.Data;
            if (isPrint)
            {
                return new ResponseResult() { Data = dataMapped, Result = Result.Success };

            }
            return new ResponseResult() { Data = new { dataMapped, total }, Result = Result.Success };

        }
        public async Task<WebReport> ReceiptPrint(int ReceiptId, int RecieptsType, exportType exportType, bool isArabic, int fileId = 0)
        {
            var receiptData = await GetReceiptById(ReceiptId, RecieptsType, true);

            var userInfo = await _iUserInformation.GetUserInformation();
            var ReceiptForHistory = (GlRecieptsResopnseDTO)(receiptData.Data);
            int screenId = 0;

            var data = (GlRecieptsResopnseDTO)receiptData.Data;

            ReportOtherData otherData = new ReportOtherData()
            {
                EmployeeName = userInfo.employeeNameAr.ToString(),
                Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                Currency = ConvertNumberToText.GetText(data.Amount.ToString(), isArabic),
                EmployeeNameEn = userInfo.employeeNameEn.ToString()
            };

            ReceiptsHistory.AddReceiptsHistory(
                    ReceiptForHistory.BranchId, ReceiptForHistory.BenefitId, exportType.ToString(), ReceiptForHistory.PaymentMethodId,
                  userInfo.userId, ReceiptForHistory.safeOrBankNameID, ReceiptForHistory.Code,
                    ReceiptForHistory.RecieptDate, ReceiptId, ReceiptForHistory.RecieptType, ReceiptForHistory.RecieptTypeId
                    , ReceiptForHistory.Signal, ReceiptForHistory.IsBlock, ReceiptForHistory.IsAccredit, ReceiptForHistory.Serialize,
                    ReceiptForHistory.Authority, ReceiptForHistory.Amount, 0
                    ,userInfo
                    );

            if (RecieptsType == 18)
            {
                screenId = (int)SubFormsIds.CashReceiptForSafe;

            }
            else if (RecieptsType == 19)
            {
                screenId = (int)SubFormsIds.PayReceiptForSafe;

            }
            else if (RecieptsType == 20)
            {
                screenId = (int)SubFormsIds.CashReceiptForBank;


            }
            else if (RecieptsType == 21)
            {
                screenId = (int)SubFormsIds.PayReceiptForBank;

            }

            var tablesNames = new TablesNames()
            {

                ObjectName = "ReceiptData",

            };
            var report = await _iGeneralPrint.PrintReport<GlRecieptsResopnseDTO, object, object>(data, null, null, tablesNames, otherData
             , screenId, exportType, isArabic, fileId);
            return report;

            //Type ReceiptType = receiptData.Data.GetType();
            //IList<PropertyInfo> propsReceipt = new List<PropertyInfo>(ReceiptType.GetProperties());
            //Type myTypeCompany = companydata.Data.GetType();

            //IList<PropertyInfo> propsCompany = new List<PropertyInfo>(myTypeCompany.GetProperties());


            //DataTable receiptDataTable = new DataTable();
            //DataRow drReceipt = receiptDataTable.NewRow();

            //DataTable otherDataTable = new DataTable();
            //DataRow drOtherData = otherDataTable.NewRow();

            //DataTable companyTable = new DataTable();
            //DataRow drCompany = companyTable.NewRow();
            //string Amount = null;
            //foreach (var Property in propsReceipt)
            //{

            //    if (Property.Name == "Amount")
            //    {
            //        Amount = Property.GetValue(receiptData.Data).ToString();
            //    }
            //    //if (Property.Name == "")
            //    //{
            //    //    safeOrBank =(int)Property.GetValue(receiptData.Data);
            //    //}
            //    receiptDataTable.Columns.Add(Property.Name);
            //    drReceipt[Property.Name] = Property.GetValue(receiptData.Data);


            //}
            //receiptDataTable.Rows.Add(drReceipt);
            //ReportOtherData otherData = new ReportOtherData()
            //{
            //    EmployeeName = userInfo.employeeNameAr.ToString(),
            //    Date = DateTime.Now.Date.ToString("yyyy/MM/dd"),
            //    Currency = ConvertNumberToText.GetText(Amount),
            //    EmployeeNameEn = userInfo.employeeNameEn.ToString()
            //};
            //Type otherDataType = otherData.GetType();
            //IList<PropertyInfo> propsOtherData = new List<PropertyInfo>(otherDataType.GetProperties());

            //foreach (var Property in propsCompany)
            //{
            //    companyTable.Columns.Add(Property.Name);

            //    if (Property.Name != "imageFile")
            //    {
            //        var value = Property.GetValue(companydata.Data);
            //        if (value == null)
            //        {
            //            drCompany[Property.Name] = "";
            //        }
            //        else
            //        {


            //            var columnData = Property.GetValue(companydata.Data).ToString();
            //            if (columnData == "null")
            //            {
            //                drCompany[Property.Name] = "";
            //            }
            //            else
            //                drCompany[Property.Name] = Property.GetValue(companydata.Data).ToString();

            //        }
            //    }

            //}
            //companyTable.Rows.Add(drCompany);

            //foreach (var Property in propsOtherData)
            //{

            //    otherDataTable.Columns.Add(Property.Name);
            //    drOtherData[Property.Name] = Property.GetValue(otherData);

            //}
            //otherDataTable.Rows.Add(drOtherData);
            //ReceiptsHistory.AddReceiptsHistory(
            //        ReceiptForHistory.BranchId, ReceiptForHistory.BenefitId, exportType.ToString(), ReceiptForHistory.PaymentMethodId,
            //      ReceiptForHistory.UserId, ReceiptForHistory.safeOrBankNameID, ReceiptForHistory.Code,
            //        ReceiptForHistory.RecieptDate, ReceiptId, ReceiptForHistory.RecieptType, ReceiptForHistory.RecieptTypeId
            //        , ReceiptForHistory.Signal, ReceiptForHistory.IsBlock, ReceiptForHistory.IsAccredit, ReceiptForHistory.Serialize,
            //        ReceiptForHistory.Authority, ReceiptForHistory.Amount

            //        );

            //receiptDataTable.TableName = "ReceiptData";

            //companyTable.TableName = "CompanyData";
            //otherDataTable.TableName = "ReportOtherData";

            //List<DataTable> tables = new List<DataTable>()
            //{
            //   // mainDataTable,
            //   receiptDataTable,
            //    companyTable,
            //    otherDataTable
            //};
            //ReportRequestDto reportRequest = new ReportRequestDto();
            //// string reportName="";

            //if (RecieptsType == 18)
            //{
            //    reportRequest.screenId = (int)SubFormsIds.CashReceiptForSafe;
            //    reportRequest.isArabic = isArabic;
            //    reportRequest.exportType = exportType;
            //    //reportName = "CashReceiptForSafe";

            //    // Currency= ConvertNumberToText.GetText(actualObject.Amount.ToString())

            //}
            //else if (RecieptsType == 19)
            //{
            //    reportRequest.screenId = (int)SubFormsIds.PayReceiptForSafe;
            //    reportRequest.isArabic = isArabic;
            //    reportRequest.exportType = exportType;
            //    // reportName = "PayReceiptForSafe";
            //}
            //else if (RecieptsType == 20)
            //{
            //    reportRequest.screenId = (int)SubFormsIds.CashReceiptForBank;
            //    reportRequest.isArabic = isArabic;
            //    reportRequest.exportType = exportType;
            //    // reportName = "CashReceiptForBank";

            //}
            //else if (RecieptsType == 21)
            //{
            //    reportRequest.screenId = (int)SubFormsIds.PayReceiptForBank;
            //    reportRequest.isArabic = isArabic;
            //    reportRequest.exportType = exportType;
            //    //reportName = "PayReceiptForBank";

            //}
            //else
            //    reportRequest = new ReportRequestDto();
            //var fileContents = await _filesMangerService.GetPrintFiles(reportRequest.screenId, reportRequest.isArabic);

            //return await _iprintService.Report(tables, fileContents.Files, exportType);


        }
        //get total balance from Receipts
        public async Task<ResponseResult> GetReceiptBalanceForBenifit(int AuthorityId, int BenefitID)
        {

            //var setting = await GeneralSetteingCashing.GetAllGeneralSettings();
            //int decemalNum = setting.Other_Decimals;

            var benfitBalance = receiptQuery.TableNoTracking
                .Where(h => h.Authority == AuthorityId
                // && BenefitID.Contains( h.BenefitId)
                && h.BenefitId == BenefitID
                && h.IsBlock == false
                ).ToList();



            double total = benfitBalance.Sum(h => h.Creditor) - benfitBalance.Sum(h => h.Debtor);
            total = RoundNumbers.GetRoundNumber(total);// Numbers.Roundedvalues(total, decemalNum);
            var curency = await CurrencyQuery.TableNoTracking.Where(h => h.IsDefault == true).FirstOrDefaultAsync();
            object financialBalance = new { total, curency.ArabicName, curency.LatinName };


            return new ResponseResult() { Data = financialBalance, Result = Result.Success, Total = total };

        }

        //get total balance from journalEntery
        public async Task<ResponseResult> GetReceiptCurrentFinancialBalance(int AuthorityId, int BenefitID)
        {

            //var setting = await GeneralSetteingCashing.GetAllGeneralSettings();
            // int decemalNum = setting.Other_Decimals;

            var financial = await getFinantialAccIdForAuthorty(AuthorityId, BenefitID);
            if (financial == null)
                return new ResponseResult() { Data = null, Result = Result.Failed, ErrorMessageAr = " ", ErrorMessageEn = "No financial account for Benefit Authority" };

            var enteryData = journalEntryDetailsRepositoryQuery.TableNoTracking.Include(a => a.journalEntry)
                                                                               .Where(s => s.FinancialAccountId == financial.financialId && s.journalEntry.IsBlock != true).ToList();

            double total = enteryData.Sum(h => h.Credit) - enteryData.Sum(h => h.Debit);
            total = RoundNumbers.GetRoundNumber(total);// Numbers.Roundedvalues(total, decemalNum);
            var curency = await CurrencyQuery.TableNoTracking.Where(h => h.IsDefault == true).FirstOrDefaultAsync();
            object financialBalance = new { total, curency.ArabicName, curency.LatinName };


            return new ResponseResult() { Data = financialBalance, Result = Result.Success, Total = total };

        }

        public async Task<ResponseResult> GetAllReceipts(GetRecieptsData parameter)
        {
            UserInformationModel userInfo = await Userinformation.GetUserInformation();

            if (userInfo == null || parameter.ReceiptsType == 0 || userInfo.CurrentbranchId == 0)
            {
                return new ResponseResult()
                {
                    Result = Result.RequiredData,
                };
            }

            //generalSetting
            // var setting = await GeneralSetteingCashing.GetAllGeneralSettings();
            //int decemalNum = setting.Other_Decimals;



            int Totalcount = receiptQuery.TableNoTracking.Count(h => h.BranchId == userInfo.CurrentbranchId
                                      && h.RecieptTypeId == parameter.ReceiptsType
                                      && h.IsAccredit == true
                                      && h.Deferre == false);
            int dataCount = receiptQuery.TableNoTracking.Count(getfilterPreidicate(parameter, userInfo.CurrentbranchId));

            if (parameter.PageSize == 0 || parameter.PageNumber == 0)
            {
                parameter.PageNumber = 1;
                parameter.PageSize = Totalcount;
            }

            bool isSafe = (parameter.ReceiptsType == (int)DocumentType.SafeCash || parameter.ReceiptsType == (int)DocumentType.SafePayment);
            var data = receiptQuery.TableNoTracking
                .Where(getfilterPreidicate(parameter, userInfo.CurrentbranchId)
                ).OrderByDescending(a => a.Id)
                .Skip((parameter.PageNumber - 1) * parameter.PageSize).Take(parameter.PageSize);



            var finalResult = data.Select(h =>
            new MainReceiptsData()
            {
                Id = h.Id,
                Code = h.Code,
                ReceiptsType = h.RecieptType,
                date = h.RecieptDate,
                financialNameAr = h.FinancialAccount.ArabicName + h.person.FinancialAccount.ArabicName + h.OtherAuthorities.FinancialAccount.ArabicName + h.SalesMan.FinancialAccount.ArabicName,
                financialNameEn = h.FinancialAccount.LatinName + h.person.FinancialAccount.LatinName + h.OtherAuthorities.FinancialAccount.LatinName + h.SalesMan.FinancialAccount.LatinName,
                Amount = RoundNumbers.GetRoundNumber(h.Amount),// Numbers.Roundedvalues(h.Amount, decemalNum),
                paymentMethodAr = h.PaymentMethods.ArabicName,
                paymentMethodEn = h.PaymentMethods.LatinName,
                safeOrBankNameAr = isSafe ? h.Safes.ArabicName : h.Banks.ArabicName,
                safeOrBankNameEn = isSafe ? h.Safes.LatinName : h.Banks.LatinName,
                benefitNameAr = h.Authority == AuthorityTypes.salesman ? h.SalesMan.ArabicName
                              : (h.Authority == AuthorityTypes.customers || h.Authority == AuthorityTypes.suppliers) ? h.person.ArabicName
                              : h.Authority == AuthorityTypes.DirectAccounts ? h.FinancialAccount.ArabicName
                              : h.Authority == AuthorityTypes.other ? h.OtherAuthorities.ArabicName : "",

                benefitNameEn = h.Authority == AuthorityTypes.salesman ? h.SalesMan.LatinName
                              : h.Authority == AuthorityTypes.customers || h.Authority == AuthorityTypes.suppliers ? h.person.LatinName
                              : h.Authority == AuthorityTypes.DirectAccounts ? h.FinancialAccount.LatinName
                              : h.Authority == AuthorityTypes.other ? h.OtherAuthorities.LatinName : "",
                isBlock = h.IsBlock,
                isAccredit = h.ParentId != null && h.SubTypeId == 0,
                isEdit = h.SubTypeId != 0 ? false : true // to disable the edit button in the front
            });

            var objectQuery = (finalResult).ToQueryString();

            var ReceiptData = await finalResult.ToListAsync();

            return new ResponseResult() { Data = ReceiptData, Result = Result.Success, TotalCount = Totalcount, DataCount = dataCount };


        }

        private Expression<Func<GlReciepts, bool>> getfilterPreidicate(GetRecieptsData parameter, int currentBranch)
        {
            return (a =>
                 (parameter.DateFrom == null || a.RecieptDate >= parameter.DateFrom)
                 && (parameter.DateTo == null || a.RecieptDate <= parameter.DateTo)
                 && a.BranchId == currentBranch
                 && a.RecieptTypeId == parameter.ReceiptsType
                 && a.IsAccredit == true
                 && a.Deferre == false
                 && (!string.IsNullOrEmpty(parameter.Code) ? (a.Code.ToString() == parameter.Code || a.RecieptType == parameter.Code) : true)
                //(a.Code.ToString().Contains(parameter.Code)
                // test here
                && (parameter.FinancialAccountId > 0 ? a.FinancialAccountId == parameter.FinancialAccountId : 1 == 1)
                && (parameter.PaymentWays > 0 ? a.PaymentMethodId == parameter.PaymentWays : 1 == 1)
                && ((parameter.AuthotityId > 0 && parameter.BenefitId != 0) ? (a.Authority == parameter.AuthotityId && a.BenefitId == parameter.BenefitId) : 1 == 1)
                && (parameter.SafeOrBankId > 0 && (parameter.ReceiptsType == (int)DocumentType.BankCash || parameter.ReceiptsType == (int)DocumentType.BankPayment)
                ? a.BankId == parameter.SafeOrBankId
                : (parameter.SafeOrBankId > 0 && (parameter.ReceiptsType == (int)DocumentType.SafeCash || parameter.ReceiptsType == (int)DocumentType.SafePayment))
                ? a.SafeID == parameter.SafeOrBankId : 1 == 1));
        }

        public async Task<ResponseResult> UpdateReceipt(UpdateRecieptsRequest parameter)
        {

            UserInformationModel userInfo = await Userinformation.GetUserInformation();
            //bool isBalance = checkBalancenInCostCenter(parameter.costCenterReciepts, parameter.Amount);
            parameter.BranchId = userInfo.CurrentbranchId;
            parameter.UserId = userInfo.employeeId;

            ValidationData ValidData = receiptsValidattion(new RecieptsRequest { ParentTypeId = parameter.ParentTypeId, Amount = parameter.Amount, RecieptTypeId = parameter.RecieptTypeId, SafeID = parameter.SafeID, BankId = parameter.BankId, Authority = parameter.Authority, }, userInfo);
            if (!string.IsNullOrEmpty(ValidData.ErrorMessageEn) || !string.IsNullOrEmpty(ValidData.ErrorMessageAr))
            {
                return new ResponseResult() { Result = Result.Failed, ErrorMessageAr = ValidData.ErrorMessageAr, ErrorMessageEn = ValidData.ErrorMessageEn };
            }
            if (parameter.Amount <= 0 && !(parameter.ParentTypeId == (int)DocumentType.CustomerFunds || parameter.ParentTypeId == (int)DocumentType.SuplierFunds))
                return new ResponseResult()
                {
                    Result = Result.Failed,
                    ErrorMessageAr = "يجب ان يكون المبلغ اكبر من الصفر",
                    ErrorMessageEn = "The Amount must be greater than Zero"
                };
            double totalCostcenterAmount = 0.0;
            parameter.Notes = parameter.Notes ?? "";
            GlReciepts NewDataExist = await receiptQuery.TableNoTracking
                .Where(a => a.Id == parameter.Id).FirstOrDefaultAsync();

            if (NewDataExist.BranchId != userInfo.CurrentbranchId)
                return new ResponseResult()
                {
                    Result = Result.Failed,
                    ErrorMessageAr = "لا يمكن التعديل على  سندات الفواتير من فروع اخرى",
                    ErrorMessageEn = "Can not Update on anthor branach Invoice Receipts "
                };

            if (((NewDataExist.ParentId != null && NewDataExist.ParentId != 0) && NewDataExist.IsAccredit == true) || NewDataExist.IsBlock)
                if (NewDataExist.SubTypeId != 0)
                    return new ResponseResult()
                    {
                        Result = Result.Failed,
                        ErrorMessageAr = "لا يمكن التعديل على  سندات سداد او تحصيل ",
                        ErrorMessageEn = "Can not Update on collection or paid Receipts "
                    };
                else
                    return new ResponseResult()
                    {
                        Result = Result.Failed,
                        ErrorMessageAr = "لا يمكن التعديل على  سندات الفواتير المعتمده او المحذوفه",
                        ErrorMessageEn = "Can not Update on Accredit Invoice Receipts "
                    };


            if (parameter.costCenterReciepts != null)
            {
                totalCostcenterAmount = parameter.costCenterReciepts.Sum(a => a.Number);


                if (totalCostcenterAmount > parameter.Amount)
                {
                    return new ResponseResult()
                    {
                        Result = Result.Failed,
                        ErrorMessageAr = "يجب ان يكون مجموع النسب اصغر من او يساوي المبلغ",
                        ErrorMessageEn = "The sum of the percentages must be less than or equal to the amount"
                    };
                }
            }
            // var setting = await GeneralSetteingCashing.GetAllGeneralSettings();
            // int decemalNum = setting.Other_Decimals;
            parameter.Amount = RoundNumbers.GetDefultRoundNumber(parameter.Amount);//Numbers.Roundedvalues(parameter.Amount, decemalNum);



            int code = NewDataExist.Code;
            int recType = NewDataExist.RecieptTypeId;
            double serialize = NewDataExist.Serialize;

            if (NewDataExist.Id <= 0)
                return new ResponseResult() { Result = Result.NotExist };

            Mapping.Mapper.Map(parameter, NewDataExist);
            NewDataExist.Code = code;
            NewDataExist.RecieptTypeId = recType;
            NewDataExist.Serialize = serialize;
            NewDataExist.CreationDate = GeneralAPIsService.serverDate(parameter.RecieptDate);
            NewDataExist.RecieptDate = GeneralAPIsService.serverDate(parameter.RecieptDate);

            //get authory data and financial account
            financialData financialForBenfiteUser = await getFinantialAccIdForAuthorty(parameter.Authority, parameter.BenefitId, NewDataExist);
            var dataExist = financialForBenfiteUser.ReceiptsData;

            if ((financialForBenfiteUser == null || financialForBenfiteUser.financialId == 0)) //&& (parameter.ParentId == null || parameter.ParentId == 0))
            {
                return new ResponseResult()
                {
                    Result = Result.Failed,
                    ErrorMessageAr = "المستفيد ليس له حساب مالى ",
                    ErrorMessageEn = "that Benfit is not have financial account "
                };
            }

            var directory = "";
            var Set_RecieptType = SetRecieptTypeAndDirectoryAndNotes(parameter.RecieptTypeId, parameter.ParentTypeId);
            directory = Set_RecieptType.Item2;

            if (parameter.ParentId == null)
            {
                dataExist.Creditor = dataExist.Signal > 0 ? dataExist.Amount : 0;
                dataExist.Debtor = dataExist.Signal < 0 ? dataExist.Amount : 0;
            }
            else if (parameter.ParentId == 0)
            {
                dataExist.Creditor = parameter.Creditor;
                dataExist.Debtor = parameter.Debtor;
            }
            else
            {
                dataExist.Creditor = dataExist.Signal < 0 ? dataExist.Amount : 0;
                dataExist.Debtor = dataExist.Signal > 0 ? dataExist.Amount : 0;
            }




            var update = await receiptCommand.UpdateAsyn(NewDataExist);
            if (update)
            {


                await filesOfInvoices.saveFilesOfInvoices(parameter.AttachedFile, parameter.BranchId, directory, dataExist.Id, true, parameter.FileId, true);
                int safeOrBank = 0;
                int? financialIdOfSafeOfBank = parameter.FA_Id;


                if (parameter.FA_Id == 0)
                {
                    safeOrBank = ValidData.safeBankId.Value;
                    financialIdOfSafeOfBank = ValidData.financialAccountId;
                }
                //    if (parameter.FA_Id <= 0)
                //{
                //    if (parameter.RecieptTypeId == (int)DocumentType.SafeCash || parameter.RecieptTypeId == (int)DocumentType.SafePayment)
                //    {
                //        safeOrBank = parameter.SafeID.Value;
                //        financialIdOfSafeOfBank = await safeQuery.TableNoTracking.Where(a => a.Id == parameter.SafeID).Select(h => h.FinancialAccountId).FirstOrDefaultAsync();
                //    }
                //    else if (parameter.RecieptTypeId == (int)DocumentType.BankCash || parameter.RecieptTypeId == (int)DocumentType.BankPayment)
                //    {
                //        financialIdOfSafeOfBank = await bankQuery.TableNoTracking.Where(a => a.Id == parameter.BankId).Select(h => h.FinancialAccountId).FirstOrDefaultAsync();
                //        safeOrBank = parameter.BankId.Value;
                //    }
                // }
                //discuss with frontend if finantcialid  put with personid
                if (!parameter.ReceiptOnly)
                {
                    await updateRecieptsInJournalEntry(parameter, dataExist, financialIdOfSafeOfBank, financialForBenfiteUser);
                }

                ReceiptsHistory.AddReceiptsHistory(
                   dataExist.BranchId, parameter.BenefitId, HistoryActions.Update, dataExist.PaymentMethodId,
                 dataExist.UserId, safeOrBank, dataExist.Code,
                   dataExist.RecieptDate, dataExist.Id, dataExist.RecieptType, dataExist.RecieptTypeId
                   , dataExist.Signal, dataExist.IsBlock, dataExist.IsAccredit, dataExist.Serialize,
                   dataExist.Authority, dataExist.Amount, 0,userInfo);
                SystemActionEnum systemActionEnum = new SystemActionEnum();
                if (parameter.RecieptTypeId == (int)DocumentType.SafeCash)
                    systemActionEnum = SystemActionEnum.editSafeCashReceipt;
                else if (parameter.RecieptTypeId == (int)DocumentType.SafePayment)
                    systemActionEnum = SystemActionEnum.editSafePaymentReceipt;
                else if (parameter.RecieptTypeId == (int)DocumentType.BankPayment)
                    systemActionEnum = SystemActionEnum.editBankPaymentReceipt;
                else if (parameter.RecieptTypeId == (int)DocumentType.BankCash)
                    systemActionEnum = SystemActionEnum.editBankCashReceipt;

                if (systemActionEnum != 0 && parameter.ParentId == null)
                    await _systemHistoryLogsService.SystemHistoryLogsService(systemActionEnum);
            }

            return new ResponseResult() { Result = Result.Success };

        }



        private async Task<bool> updateRecieptsInJournalEntry(UpdateRecieptsRequest parameter, GlReciepts data, int? financialIdOfSafeOfBank, financialData financialForBenfiteUser)
        {
            // update journal entiry of safe and bank 
            try
            {
                var costCenterRecieptsList = new List<GLRecieptCostCenter>();
                var costOldData = CostCenterRecieptQuery.GetAll(h => h.SupportId == parameter.Id).ToList();
                List<int> OldCostId = costOldData.Select(h => h.Id).ToList();

                //deleted all costdata that id = this reciept id
                if (costOldData.Count > 0)
                {
                    bool CostDeleted = await costCenterREcieptCommand.DeleteAsync(h => h.SupportId == parameter.Id);
                    if (!CostDeleted)
                        return false;
                }

                //need refactor for this code
                int journalEntryId = 0;
                var journalEntry = JournalenteryQuery.TableNoTracking.FirstOrDefault(h => h.ReceiptsId == data.Id);

                if (journalEntry != null)
                {
                    journalEntryId = journalEntry.Id;
                }

                UpdateJournalEntryRequest JEntry = new UpdateJournalEntryRequest();
                JEntry.BranchId = parameter.BranchId;
                JEntry.FTDate = parameter.RecieptDate;
                JEntry.Id = journalEntryId;
                JEntry.Notes = data.NoteAR;
                JEntry.fromSystem = true;
                if (parameter.ParentId != null && !(parameter.ParentTypeId == (int)DocumentType.PermittedDiscount || parameter.ParentTypeId == (int)DocumentType.AcquiredDiscount))
                    JEntry.IsAccredit = false;




                JEntry.journalEntryDetails.Add(new JournalEntryDetail()//add the main data of journal entery
                {
                    FinancialAccountId = financialIdOfSafeOfBank,
                    Credit = data.Signal < 0 ? data.Amount : 0,
                    Debit = data.Signal > 0 ? data.Amount : 0,
                    DescriptionAr = data.NoteAR,
                    DescriptionEn = data.NoteEN,

                });

                //فى حاله لو فيه مراكز تكلفه  هينفذ التالى
                if (parameter.costCenterReciepts != null)
                {
                    double totalCostcenterAmount = parameter.costCenterReciepts.Sum(h => h.Number);
                    foreach (var item in parameter.costCenterReciepts)
                    {
                        //add new journal enitity details for every costcenter
                        JEntry.journalEntryDetails.Add(new JournalEntryDetail()
                        {
                            FinancialAccountId = financialForBenfiteUser.financialId,
                            FinancialCode = financialForBenfiteUser.financialCode,
                            FinancialName = financialForBenfiteUser.FinancialName,
                            Credit = data.Signal > 0 ? item.Number : 0,
                            Debit = data.Signal < 0 ? item.Number : 0,
                            CostCenterId = item.CostCenterId,
                            DescriptionAr = data.NoteAR,
                            DescriptionEn = data.NoteEN,
                        });
                        //add costcent in table to use it un get data
                        #region save costcenter
                        var center = Mapping.Mapper.Map<UpdateCostCenterReciepts, GLRecieptCostCenter>(item);
                        center.SupportId = data.Id;
                        costCenterRecieptsList.Add(center);
                        #endregion

                    }
                    bool Costsave = await costCenterREcieptCommand.AddAsync(costCenterRecieptsList);

                    if (totalCostcenterAmount < data.Amount)
                    {
                        double restAmount = data.Amount - totalCostcenterAmount;
                        JEntry.journalEntryDetails.Add(new JournalEntryDetail()
                        {
                            FinancialAccountId = financialForBenfiteUser.financialId,
                            FinancialCode = financialForBenfiteUser.financialCode,
                            FinancialName = financialForBenfiteUser.FinancialName,
                            Credit = data.Signal > 0 ? restAmount : 0,
                            Debit = data.Signal < 0 ? restAmount : 0,
                            DescriptionAr = data.NoteAR,
                            DescriptionEn = data.NoteEN,
                        });
                    }

                }
                else
                    JEntry.journalEntryDetails.Add(new JournalEntryDetail()
                    {
                        FinancialAccountId = financialForBenfiteUser.financialId,
                        FinancialCode = financialForBenfiteUser.financialCode,
                        FinancialName = financialForBenfiteUser.FinancialName,
                        Credit = data.Signal > 0 ? data.Amount : 0,
                        Debit = data.Signal < 0 ? data.Amount : 0,
                        DescriptionAr = data.NoteAR,
                        DescriptionEn = data.NoteEN,
                    });


                //var res = await journalEntryBusiness.UpdateJournalEntry(JEntry);
                var res = await _mediator.Send(JEntry);


                return res.Status == RepositoryActionStatus.Created;
            }
            catch (Exception e)
            {

                string x = e.Message;
                return false;
            }

        }


        public async Task<ResponseResult> GetReceiptsAuthortyDropDown()
        {
            return new ResponseResult() { Data = Lists.receiptsAuthorities, Result = Result.Success };
        }

        public async Task<ResponseResult> DeleteReciepts(List<int?> Ids)
        {

            UserInformationModel userInfo = await Userinformation.GetUserInformation();

            var RecieptsDel = await receiptCommand.Get(q => Ids.Contains(q.Id) && q.IsBlock == false && q.BranchId == userInfo.CurrentbranchId);

            //Check data
            ResponseResult valid = await deleteValidate(RecieptsDel);
            if (valid.Result != Result.Success)
                return valid;

            RecieptsDel.Select(e => { e.IsBlock = true; return e; }).ToList();
            var save = await receiptCommand.UpdateAsyn(RecieptsDel);
            if (save)
            {

                List<int> journalEntryId = await JournalenteryQuery.TableNoTracking.Where(h => Ids.Contains(h.ReceiptsId)).Select(h => h.Id).ToListAsync();
                //await journalEntryBusiness.BlockJournalEntry(new BlockJournalEntry() { Ids = journalEntryId.ToArray() });
                await _mediator.Send(new BlockJournalEntryReqeust { Ids = journalEntryId.ToArray() });


                // await costCenterREcieptCommand.DeleteAsync(h => Ids.Contains(h.SupportId));

                foreach (var item in RecieptsDel)
                {
                    ReceiptsHistory.AddReceiptsHistory(
                                         item.BranchId, item.BenefitId, HistoryActions.Delete, item.PaymentMethodId,
                                         item.UserId, item.BankId != null ? item.BankId.Value : item.SafeID.Value,
                                         item.Code, item.RecieptDate, item.Id, item.RecieptType, item.RecieptTypeId,
                                         item.Signal, item.IsBlock, item.IsAccredit, item.Serialize,
                                         item.Authority, item.Amount, item.SubTypeId, userInfo);

                }
                var receiptsType = RecieptsDel.GroupBy(x => x.RecieptTypeId).Select(y => y.FirstOrDefault());
                foreach (var item in receiptsType)
                {
                    SystemActionEnum systemActionEnum = new SystemActionEnum();
                    if (item.RecieptTypeId == (int)DocumentType.SafeCash)
                        systemActionEnum = SystemActionEnum.editSafeCashReceipt;
                    else if (item.RecieptTypeId == (int)DocumentType.SafePayment)
                        systemActionEnum = SystemActionEnum.editSafePaymentReceipt;
                    else if (item.RecieptTypeId == (int)DocumentType.BankPayment)
                        systemActionEnum = SystemActionEnum.editBankPaymentReceipt;
                    else if (item.RecieptTypeId == (int)DocumentType.BankCash)
                        systemActionEnum = SystemActionEnum.editBankCashReceipt;
                    await _systemHistoryLogsService.SystemHistoryLogsService(systemActionEnum);
                }

            }
            else
                return new ResponseResult() { Result = Result.Failed };


            return new ResponseResult() { Result = Result.Success };

        }

        private async Task<ResponseResult> deleteValidate(List<GlReciepts> RecieptsDel)
        {
            //Safe Cash 
            if (RecieptsDel.Where(x => x.RecieptTypeId == (int)DocumentType.SafeCash).Any())
            {
                var isAuthorized = await _iAuthorizationService.isAuthorized((int)MainFormsIds.safes, (int)SubFormsIds.CashReceiptForSafe, Opretion.Delete);
                if (isAuthorized != null)
                    return isAuthorized;
            }
            //SafePayment
            if (RecieptsDel.Where(x => x.RecieptTypeId == (int)DocumentType.SafePayment).Any())
            {
                var isAuthorized = await _iAuthorizationService.isAuthorized((int)MainFormsIds.safes, (int)SubFormsIds.PayReceiptForSafe, Opretion.Delete);
                if (isAuthorized != null)
                    return isAuthorized;
            }
            //BankCash
            if (RecieptsDel.Where(x => x.RecieptTypeId == (int)DocumentType.BankCash).Any())
            {
                var isAuthorized = await _iAuthorizationService.isAuthorized((int)MainFormsIds.banks, (int)SubFormsIds.CashReceiptForBank, Opretion.Delete);
                if (isAuthorized != null)
                    return isAuthorized;
            }
            //BankCash
            if (RecieptsDel.Where(x => x.RecieptTypeId == (int)DocumentType.BankPayment).Any())
            {
                var isAuthorized = await _iAuthorizationService.isAuthorized((int)MainFormsIds.banks, (int)SubFormsIds.PayReceiptForBank, Opretion.Delete);
                if (isAuthorized != null)
                    return isAuthorized;
            }

            if (RecieptsDel.Where(h => h.ParentId != null && h.IsAccredit == true && h.SubTypeId == 0).Count() > 0)
                return new ResponseResult()
                {
                    Result = Result.Failed,
                    ErrorMessageAr = "لا يمكن حذف على  سندات الفواتير المعتمده",
                    ErrorMessageEn = "Can not ]Delete  Accredit Invoice Receipts "
                };
            if (RecieptsDel.Count <= 0)
                return new ResponseResult() { Result = Result.NotFound, ErrorMessageAr = "لا يوجد هذا العنصر للحذف  ", ErrorMessageEn = "this item not found to delete" };

            return new ResponseResult() { Result = Result.Success };
        }
    }


}
