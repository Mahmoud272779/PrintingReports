﻿using App.Application.Handlers.GeneralLedger.JournalEntry.JournalEntryHelper;
using App.Infrastructure.settings;
using MediatR;
using System.Threading;

namespace App.Application.Handlers.GeneralLedger.JournalEntry
{
    public class getJournalEntryServHandler : IRequestHandler<getJournalEntryServRequest, getAllJournalEntryResponse>
    {
        private readonly IRepositoryQuery<GLJournalEntry> journalEntryRepositoryQuery;
        private readonly IRepositoryQuery<InvGeneralSettings> generalSettingsRepositoryQuery;
        private readonly IRepositoryQuery<GLBranch> branchRepositoryQuery;
        private readonly IRepositoryQuery<GLCostCenter> costCenterRepositoryQuery;
        private readonly IRepositoryQuery<GLFinancialAccount> financialAccountRepositoryQuery;
        private readonly IRoundNumbers roundNumbers;

        public getJournalEntryServHandler(IRepositoryQuery<GLJournalEntry> journalEntryRepositoryQuery, IRepositoryQuery<InvGeneralSettings> generalSettingsRepositoryQuery, IRepositoryQuery<GLBranch> branchRepositoryQuery, IRepositoryQuery<GLCostCenter> costCenterRepositoryQuery, IRepositoryQuery<GLFinancialAccount> financialAccountRepositoryQuery, IRoundNumbers roundNumbers)
        {
            this.journalEntryRepositoryQuery = journalEntryRepositoryQuery;
            this.generalSettingsRepositoryQuery = generalSettingsRepositoryQuery;
            this.branchRepositoryQuery = branchRepositoryQuery;
            this.costCenterRepositoryQuery = costCenterRepositoryQuery;
            this.financialAccountRepositoryQuery = financialAccountRepositoryQuery;
            this.roundNumbers = roundNumbers;
        }



        public async Task<getAllJournalEntryResponse> Handle(getJournalEntryServRequest paramters, CancellationToken cancellationToken)
        {
            var list = new List<JournalEntryDto>();

            var journalEntry = journalEntryRepositoryQuery.TableNoTracking
                                                            .Include(x => x.JournalEntryDetails)
                                                            .OrderByDescending(x => x.Code)
                                                            .Where(x => x.JournalEntryDetails.Any() && x.IsAccredit == true)
                                                            .Where(q => paramters.Search.From != null ? q.FTDate.Value.Date >= paramters.Search.From.Value.Date : true)
                                                            .Where(q => paramters.Search.To != null ? q.FTDate.Value.Date <= paramters.Search.To.Value.Date : true)
                                                            .Where(q => paramters.Search.Code != 0 ? q.Code == paramters.Search.Code : true)
                                                            .Where(q => paramters.Search.IsTransfer != null ? q.IsTransfer == (paramters.Search.IsTransfer.Value == 1 ? true : false) : true);
            var settings = generalSettingsRepositoryQuery.TableNoTracking.FirstOrDefault();

            var _result = !paramters.isPrint ? journalEntry.Skip((paramters.PageNumber - 1) * paramters.PageSize).Take(paramters.PageSize) : journalEntry;
            var result = _result.ToList();
            var branches = branchRepositoryQuery.TableNoTracking;
            var costs = costCenterRepositoryQuery.TableNoTracking;
            var allAccounts = journalEntryHelper.getAllAccounts(financialAccountRepositoryQuery);
            var ActionsList = Actions.fundLists();
            var allFinancialAccounts = journalEntryHelper.getAllAccounts(financialAccountRepositoryQuery);
            var data = result
            .Select(x => new getAllJournalEntryPrepering
            {
                Id = x.Id,
                Code = x.Code,
                CreditTotal = roundNumbers.GetRoundNumber(x.JournalEntryDetails.Sum(d => d.Credit)    /*x.CreditTotal*/),
                DebitTotal = roundNumbers.GetRoundNumber(x.JournalEntryDetails.Sum(d => d.Debit)     /*x.DebitTotal*/),
                CurrencyId = x.CurrencyId,
                FTDate = x.FTDate != null ? x.FTDate.Value.ToString(defultData.datetimeFormat) : "",
                Notes = x.Code < 0 ? ActionsList.Where(d => d.id == x.Id).FirstOrDefault().descAr : x.Notes,
                BranchId = x.BranchId,
                BranchNameAr = branches.Where(c => c.Id == x.BranchId).Select(c => c.ArabicName).FirstOrDefault(),
                BranchNameEn = branches.Where(c => c.Id == x.BranchId).Select(c => c.LatinName).FirstOrDefault(),
                IsBlock = x.IsBlock,
                IsDraft = false,
                IsTransfer = x.IsTransfer,
                Auto = x.Auto,
                canDelete = x.JournalEntryDetails.FirstOrDefault().isStoreFund,
                journalEntryDetailsDtos = x.JournalEntryDetails
                            .ToList()
                            .Select(c => new journalEntryDetailsDtosResponse
                            {
                                Id = c.Id,
                                FinancialAccountId = c.FinancialAccountId,
                                JournalEntryId = c.JournalEntryId,
                                DescriptionAr = c.DescriptionAr,
                                DescriptionEn = c.DescriptionEn,
                                CostCenterId = c.CostCenterId,
                                Credit = roundNumbers.GetRoundNumber(c.Credit    /*x.CreditTotal*/),
                                Debit = roundNumbers.GetRoundNumber(c.Debit     /*x.DebitTotal*/),
                                financialAccountNameAr = allAccounts.Where(d => d.Id == c.FinancialAccountId).FirstOrDefault().ArabicName,
                                financialAccountNameEn = allAccounts.Where(d => d.Id == c.FinancialAccountId).FirstOrDefault().LatinName,
                                financialAccountCode = allAccounts.Where(d => d.Id == c.FinancialAccountId).FirstOrDefault().AccountCode.Replace(".", string.Empty),
                                CostCenterName = costs.Where(d => d.Id == c.CostCenterId).Select(d => d.ArabicName).FirstOrDefault(),
                                CostCenterNameEn = costs.Where(d => d.Id == c.CostCenterId).Select(d => d.LatinName).FirstOrDefault(),

                            })
            });
            return new getAllJournalEntryResponse()
            {
                data = data,
                journalEntry = journalEntry
            };
        }
    }
}
