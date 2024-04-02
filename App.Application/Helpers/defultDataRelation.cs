using App.Domain.Entities;
using App.Domain.Entities.Process;
using App.Domain.Entities.Process.Store;
using App.Domain.Enums;
using App.Domain.Models.Response.Store;
using App.Infrastructure.Interfaces.Repository;
using App.Infrastructure.settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Domain.Enums.Enums;
using static App.Domain.Models.Shared.accountantTree;

namespace App.Application.Helpers
{
    public interface iDefultDataRelation
    {
        Task<bool> BranchsRelation(int branchId);
        Task<bool> AdministratorUserRelation(int Type, int Id);
    }
    internal class defultDataRelation : iDefultDataRelation
    {
        private readonly IRepositoryCommand<OtherSettingsStores> _otherSettingsStoresCommand;
        private readonly IRepositoryQuery<GLFinancialAccount> _GLFinancialAccountQuery;
        private readonly IRepositoryCommand<OtherSettingsSafes> _otherSettingsSafesCommand;
        private readonly IRepositoryCommand<OtherSettingsBanks> _otherSettingsBanksCommand;
        private readonly IRepositoryCommand<InvEmployeeBranch> _invEmployeeBranchCommand;
        private readonly IRepositoryCommand<InvPersons_Branches> _InvPersons_BranchesCommand;
        private readonly IRepositoryCommand<InvSalesMan_Branches> _InvSalesMan_BranchesCommand;
        private readonly IRepositoryCommand<GLIntegrationSettings> _gLIntegrationSettingsCommand;
        private readonly IRepositoryCommand<GLPurchasesAndSalesSettings> _gLPurchasesAndSalesSettingsCommand;

        public defultDataRelation(
                                    IRepositoryCommand<OtherSettingsStores> OtherSettingsStoresCommand,
                                    IRepositoryCommand<OtherSettingsSafes> OtherSettingsSafesCommand,
                                    IRepositoryCommand<OtherSettingsBanks> OtherSettingsBanksCommand,
                                    IRepositoryCommand<InvEmployeeBranch> InvEmployeeBranchCommand,
                                    IRepositoryCommand<InvPersons_Branches> InvPersons_BranchesCommand,
                                    IRepositoryCommand<InvSalesMan_Branches> InvSalesMan_BranchesCommand,
                                    IRepositoryCommand<GLIntegrationSettings> GLIntegrationSettingsCommand,
                                    IRepositoryCommand<GLPurchasesAndSalesSettings> GLPurchasesAndSalesSettingsCommand
,
                                    IRepositoryQuery<GLFinancialAccount> gLFinancialAccountQuery
                                    )
        {
            _otherSettingsStoresCommand = OtherSettingsStoresCommand;
            _otherSettingsSafesCommand = OtherSettingsSafesCommand;
            _otherSettingsBanksCommand = OtherSettingsBanksCommand;
            _invEmployeeBranchCommand = InvEmployeeBranchCommand;
            _InvPersons_BranchesCommand = InvPersons_BranchesCommand;
            _InvSalesMan_BranchesCommand = InvSalesMan_BranchesCommand;
            _gLIntegrationSettingsCommand = GLIntegrationSettingsCommand;
            _gLPurchasesAndSalesSettingsCommand = GLPurchasesAndSalesSettingsCommand;
            _GLFinancialAccountQuery = gLFinancialAccountQuery;
        }
        /// <summary>
        /// type : 
        /// 1-bank
        /// 2-safe
        /// 3-store
        /// </summary>
        public async Task<bool> AdministratorUserRelation(int Type, int Id)
        {
            bool saved = false;
            if (Type == 1)
            {
                var banks = new OtherSettingsBanks()
                {
                    gLBankId = Id,
                    otherSettingsId = 1
                };
                saved = await _otherSettingsBanksCommand.AddAsync(banks);
            }
            if (Type == 2)
            {
                var safe = new OtherSettingsSafes()
                {
                    gLSafeId = Id,
                    otherSettingsId = 1
                };
                saved = await _otherSettingsSafesCommand.AddAsync(safe);
            }
            if (Type == 3)
            {
                var stores = new OtherSettingsStores()
                {
                    InvStpStoresId = Id,
                    otherSettingsId = 1
                };
                saved = await _otherSettingsStoresCommand.AddAsync(stores);
            }
            return saved;
        }

        public async Task<bool> BranchsRelation(int branchId)
        {
            bool saved = false;
            //Emplyees
            var empBranch = new InvEmployeeBranch()
            {
                BranchId = branchId,
                EmployeeId = 1
            };
            saved = await _invEmployeeBranchCommand.AddAsync(empBranch);
            //persons
            var personBranchs = new List<InvPersons_Branches>();
            personBranchs.AddRange(new[]
            {
                new InvPersons_Branches
                {
                    BranchId = branchId,
                    PersonId = 1
                },
                new InvPersons_Branches
                {
                    BranchId = branchId,
                    PersonId = 2
                }
            });
            _InvPersons_BranchesCommand.AddRange(personBranchs);
            saved = await _InvPersons_BranchesCommand.SaveAsync();
            //salesman
            var salesManBranches = new InvSalesMan_Branches()
            {
                BranchId = branchId,
                SalesManId = 1
            };
            _InvSalesMan_BranchesCommand.Add(salesManBranches);
            await _InvSalesMan_BranchesCommand.SaveAsync();

            var list = new List<GLIntegrationSettings>();
            list.AddRange(new[]
            {
                new GLIntegrationSettings
                {
                    GLBranchId= branchId,
                    screenId = (int)SubFormsIds.Safes_MainData,
                    GLFinancialAccountId = (int)FinanicalAccountDefultIds.Mainbranchtreasury,
                    linkingMethodId = 2,
                },
                new GLIntegrationSettings
                {
                    GLBranchId= branchId,
                    screenId = (int)SubFormsIds.Banks_MainData,
                    GLFinancialAccountId = (int)FinanicalAccountDefultIds.AlRajhiBank,
                    linkingMethodId = 2,
                },
                new GLIntegrationSettings
                {
                    GLBranchId= branchId,
                    screenId = (int)SubFormsIds.Suppliers_Purchases,
                    GLFinancialAccountId = (int)FinanicalAccountDefultIds.Suppliers,
                    linkingMethodId = 2,
                },
                new GLIntegrationSettings
                {
                    GLBranchId= branchId,
                    screenId = (int)SubFormsIds.Customers_Sales,
                    GLFinancialAccountId = (int)FinanicalAccountDefultIds.Customers,
                    linkingMethodId = 2,
                },
                new GLIntegrationSettings
                {
                    GLBranchId= branchId,
                    screenId = (int)SubFormsIds.Employees_MainUnits,
                    GLFinancialAccountId = (int)FinanicalAccountDefultIds.StaffSalariesAndWages,
                    linkingMethodId = 2,
                },
                new GLIntegrationSettings
                {
                    GLBranchId= branchId,
                    screenId = (int)SubFormsIds.Salesmen_Sales,
                    GLFinancialAccountId = (int)FinanicalAccountDefultIds.salesMan,
                    linkingMethodId = 2,
                },
                new GLIntegrationSettings
                {
                    GLBranchId= branchId,
                    screenId = (int)SubFormsIds.OtherAuthorities_MainData,
                    GLFinancialAccountId = (int)FinanicalAccountDefultIds.OtherAuthorities,
                    linkingMethodId = 2,
                }
            });



            _gLIntegrationSettingsCommand.AddRange(list);
            saved = await _gLIntegrationSettingsCommand.SaveAsync();


            var GLPurchasesAndSalesSettingsList = defultData.New_getlistOfGLPurchasesAndSalesSettingsTable();
            List<GLPurchasesAndSalesSettings> forSeeding = new List<GLPurchasesAndSalesSettings>();
            var allAccounts = _GLFinancialAccountQuery.TableNoTracking;
            GLPurchasesAndSalesSettingsList.ForEach(x =>
            {
                if(allAccounts.Where(c=> c.Id == x.FinancialAccountId).Any())
                {
                    forSeeding.Add(new GLPurchasesAndSalesSettings
                    {
                        branchId = branchId,
                        ArabicName = x.ArabicName,
                        LatinName = x.LatinName,
                        MainType = x.MainType,
                        FinancialAccountId = x.FinancialAccountId,
                        ReceiptElemntID = x.ReceiptElemntID,
                        RecieptsType = x.RecieptsType,

                    });
                }
            });
            _gLPurchasesAndSalesSettingsCommand.AddRange(forSeeding);
            saved = await _gLPurchasesAndSalesSettingsCommand.SaveAsync();

            return saved;

        }

    }
}
