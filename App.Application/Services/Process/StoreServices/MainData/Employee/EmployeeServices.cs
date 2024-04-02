using App.Application.Handlers.AttendLeaving.AttendanceMachines.GetMachines;
using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.Models;
using App.Application.Handlers.Setup.UsersAndPermissions.GetUsersByDate;
using App.Application.Handlers.Units;
using App.Application.Helpers.Service_helper.FileHandler;
using App.Application.Helpers.Service_helper.History;
using App.Application.Services.HelperService.SecurityIntegrationServices;
using App.Application.Services.Printing.InvoicePrint;
using App.Application.Services.Process.FinancialAccounts;
using App.Application.Services.Process.GeneralServices.DeletedRecords;
using App.Domain.Entities.Process;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Shift;
using App.Domain.Entities.Process.Store;
using App.Domain.Models.Security.Authentication.Response.Store;
using App.Infrastructure;
using App.Infrastructure.settings;
using App.Infrastructure.UserManagementDB;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Spreadsheet;
using Hangfire.Storage.Monitoring;
using Microsoft.AspNetCore.Server.HttpSys;
using Org.BouncyCastle.Ocsp;
using SkiaSharp;
using System.Linq;
using System.Reflection.PortableExecutable;
using static App.Domain.Models.Response.General.AdditionalPrices;
using static App.Domain.Models.Security.Authentication.Request.EmployeesRequestDTOs;
using static App.Domain.Models.Security.Authentication.Response.Store.EmployeeResponsesDTOs;

namespace App.Application.Services.Process.Employee
{
    public class EmployeeServices : BaseClass, IEmployeeServices
    {
        private readonly IRepositoryQuery<InvEmployees> EmployeesRepositoryQuery;
        private readonly IRepositoryCommand<InvEmployees> EmployeesRepositoryCommand;

        private readonly IRepositoryCommand<GLFinancialAccount> _GLFinancialAccountCommand;
        private readonly IRepositoryCommand<ChangefulTimeGroupsEmployees> _ChangefulTimeGroupsEmployeesCommand;
        private readonly IRepositoryQuery<GLBranch> BranchRepositoryQuery;
        private readonly IRepositoryQuery<ChangefulTimeGroupsMaster> _ChangefulTimeGroupsMasterQuery;
        private readonly IRepositoryQuery<InvJobs> _InvJobsQuery;
        private readonly IRepositoryQuery<SectionsAndDepartments> _SectionsAndDepartmentsQuery;
        private readonly IRepositoryQuery<ShiftsMaster> _ShiftsMasterQuery;
        private readonly IRepositoryQuery<Projects> _ProjectsQuery;
        private readonly IRepositoryQuery<Missions> _MissionsQuery;
        private readonly IRepositoryQuery<Nationality> _NationalityQuery;
        private readonly IRepositoryQuery<religions> _religionsQuery;
        private readonly IRepositoryQuery<GLIntegrationSettings> _gLIntegrationSettingsQuery;
        private readonly IRepositoryQuery<Machines> _MachinesQuery;


        private readonly IRepositoryCommand<InvEmployeeBranch> EmployeeBranchRepositoryCommand;
        private readonly IRepositoryQuery<InvEmployeeBranch> EmployeeBranchRepositoryQuery;
        private readonly IRepositoryQuery<GLBranch> branchesRepositoryQuery;
        private readonly IRepositoryQuery<GLFinancialAccount> _gLFinancialAccountQuery;
        private readonly IRepositoryQuery<GLGeneralSetting> _gLGeneralSettingQuery;
        private readonly IRepositoryQuery<userAccount> _useraccountQuery;
        private readonly iGLFinancialAccountRelation _iGLFinancialAccountRelation;
        private readonly IRepositoryQuery<InvoiceMaster> _inviceMasterQuery;
        private readonly IRepositoryQuery<OfferPriceMaster> _OfferPriceMasterQuery;
        private readonly IDeletedRecordsServices _deletedRecords;
        private readonly IGeneralAPIsService generalAPIsService;
        private readonly IFinancialAccountBusiness _financialAccountBusiness;
        private readonly ISystemHistoryLogsService _systemHistoryLogsService;
        private readonly iUserInformation _userinformation;
        private readonly ISecurityIntegrationService _securityIntegrationService;
        private readonly IHistory<InvEmployeesHistory> history;
        private readonly IFileHandler _fileHandler;
        private readonly IPrintService _iprintService;

        private readonly IFilesMangerService _filesMangerService;
        private readonly ICompanyDataService _CompanyDataService;
        private readonly iUserInformation _iUserInformation;
        private readonly IGeneralPrint _iGeneralPrint;

        public IHttpContextAccessor HttpContext { get; }

        public EmployeeServices(IRepositoryQuery<InvEmployees> _EmployeesRepositoryQuery,
                                  IRepositoryCommand<InvEmployees> _EmployeesRepositoryCommand,
                                  IRepositoryQuery<GLBranch> _BranchRepositoryQuery,
                                  IRepositoryCommand<InvEmployeeBranch> _EmployeeBranchRepositoryCommand,
                                  IRepositoryQuery<InvEmployeeBranch> _EmployeeBranchRepositoryQuery,
                                  IRepositoryQuery<GLBranch> _BranchesRepositoryQuery,
                                  IRepositoryQuery<GLFinancialAccount> GLFinancialAccountQuery,
                                  IRepositoryQuery<GLGeneralSetting> GLGeneralSettingQuery,
                                  IRepositoryQuery<userAccount> useraccountQuery,
                                  iGLFinancialAccountRelation iGLFinancialAccountRelation,
                                  IRepositoryQuery<InvoiceMaster> InviceMasterQuery,
                                  IWebHostEnvironment _hostingEnvironment,
                                  iUserInformation Userinformation,
                                  ISecurityIntegrationService securityIntegrationService,
                                  IFinancialAccountBusiness financialAccountBusiness,
                                  ISystemHistoryLogsService systemHistoryLogsService,
                                  IHistory<InvEmployeesHistory> history,
                                  IFileHandler fileHandler,
                                  IHttpContextAccessor _httpContext,
                                  IPrintService iprintService,
                                  IFilesMangerService filesMangerService,
                                  ICompanyDataService companyDataService,
                                  iUserInformation iUserInformation,
                                  IGeneralPrint iGeneralPrint,
                                  IRepositoryQuery<OfferPriceMaster> offerPriceMasterQuery,
                                  IDeletedRecordsServices deletedRecords,
                                  IGeneralAPIsService generalAPIsService,
                                  IRepositoryQuery<InvJobs> invJobsQuery,
                                  IRepositoryQuery<ShiftsMaster> shiftsMasterQuery,
                                  IRepositoryQuery<SectionsAndDepartments> sectionsAndDepartmentsQuery,
                                  IRepositoryQuery<Projects> projectsQuery,
                                  IRepositoryQuery<Missions> missionsQuery,
                                  IRepositoryQuery<Nationality> nationalityQuery,
                                  IRepositoryQuery<religions> religionsQuery,
                                  IRepositoryCommand<GLFinancialAccount> gLFinancialAccountCommand,
                                  IRepositoryQuery<GLIntegrationSettings> gLIntegrationSettingsQuery,
                                  IRepositoryQuery<ChangefulTimeGroupsMaster> changefulTimeGroupsMasterQuery,
                                  IRepositoryCommand<ChangefulTimeGroupsEmployees> changefulTimeGroupsEmployeesCommand,
                                  IRepositoryQuery<Machines> machinesQuery) : base(_httpContext)
        {
            EmployeesRepositoryQuery = _EmployeesRepositoryQuery;
            EmployeesRepositoryCommand = _EmployeesRepositoryCommand;
            BranchRepositoryQuery = _BranchRepositoryQuery;
            EmployeeBranchRepositoryCommand = _EmployeeBranchRepositoryCommand;
            EmployeeBranchRepositoryQuery = _EmployeeBranchRepositoryQuery;
            branchesRepositoryQuery = _BranchesRepositoryQuery;
            _gLFinancialAccountQuery = GLFinancialAccountQuery;
            _gLGeneralSettingQuery = GLGeneralSettingQuery;
            _useraccountQuery = useraccountQuery;
            _iGLFinancialAccountRelation = iGLFinancialAccountRelation;
            _inviceMasterQuery = InviceMasterQuery;
            _financialAccountBusiness = financialAccountBusiness;
            _systemHistoryLogsService = systemHistoryLogsService;
            _userinformation = Userinformation;
            _securityIntegrationService = securityIntegrationService;
            this.history = history;
            _fileHandler = fileHandler;
            HttpContext = _httpContext;
            _iprintService = iprintService;
            _filesMangerService = filesMangerService;
            _CompanyDataService = companyDataService;
            _iUserInformation = iUserInformation;
            _iGeneralPrint = iGeneralPrint;
            _OfferPriceMasterQuery = offerPriceMasterQuery;
            _deletedRecords = deletedRecords;
            this.generalAPIsService = generalAPIsService;
            _InvJobsQuery = invJobsQuery;
            _ShiftsMasterQuery = shiftsMasterQuery;
            _SectionsAndDepartmentsQuery = sectionsAndDepartmentsQuery;
            _ProjectsQuery = projectsQuery;
            _MissionsQuery = missionsQuery;
            _NationalityQuery = nationalityQuery;
            _religionsQuery = religionsQuery;
            _GLFinancialAccountCommand = gLFinancialAccountCommand;
            _gLIntegrationSettingsQuery = gLIntegrationSettingsQuery;
            _ChangefulTimeGroupsMasterQuery = changefulTimeGroupsMasterQuery;
            _ChangefulTimeGroupsEmployeesCommand = changefulTimeGroupsEmployeesCommand;
            _MachinesQuery = machinesQuery;
        }


        private async Task<ResponseResult> IsValied(EmployeesRequestDTOs.Add request, companyInfomation security, int Id = 0, bool isEditMode = false)
        {

            if (!security.isInfinityNumbersOfEmployees)
            {
                var employeeCount = EmployeesRepositoryQuery
                    .TableNoTracking
                    .Where(c => c.Status != (int)Status.newElement)
                    .Where(c => Id != 0 ? c.Id != Id : true)
                    .Count();
                if (employeeCount >= security.AllowedNumberOfEmployee)
                    return new ResponseResult()
                    {
                        Result = Result.MaximumLength,
                        ErrorMessageAr = "تجاوزت الحد الاقصي من عدد الموظفين",
                        ErrorMessageEn = "You Cant add a new employee because you have the maximum of employees for your bunlde",
                        Note = Actions.YouHaveTheMaxmumOfEmployees
                    };
            }

            if (string.IsNullOrEmpty(request.ArabicName.Trim()))
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        MessageAr = ErrorMessagesAr.arabicNameIsRequired,
                        MessageEn = ErrorMessagesEn.arabicNameIsRequired,
                        type = AlartShow.popup,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };


            var branches = BranchRepositoryQuery.TableNoTracking;
            var ShiftsMaster = _ShiftsMasterQuery.TableNoTracking;
            var sectionsAndDepartmentsQuery = _SectionsAndDepartmentsQuery.TableNoTracking;
            var projects = _ProjectsQuery.TableNoTracking;
            var missions = _MissionsQuery.TableNoTracking;
            var Nationality = _NationalityQuery.TableNoTracking;
            var religions = _religionsQuery.TableNoTracking;
            var employees = EmployeesRepositoryQuery.TableNoTracking;


            if (security.apps.Any(c => c.Id == (int)applicationIds.AttendLeaving))
            {
                if (isEditMode)
                {
                    if (employees.Any(c => c.Code == request.Code && c.Id != Id))
                        return new ResponseResult
                        {
                            Result = Result.Failed,
                            Alart = new Alart
                            {
                                AlartType = AlartType.error,
                                MessageAr = ErrorMessagesAr.CodeExist,
                                MessageEn = ErrorMessagesEn.CodeExist,
                                type = AlartShow.popup,
                                titleAr = "خطأ",
                                titleEn = "Error"
                            }
                        };
                }
                else
                {
                    if (employees.Any(c => c.Code == request.Code))
                        return new ResponseResult
                        {
                            Result = Result.Failed,
                            Alart = new Alart
                            {
                                AlartType = AlartType.error,
                                MessageAr = ErrorMessagesAr.CodeExist,
                                MessageEn = ErrorMessagesEn.CodeExist,
                                type = AlartShow.popup,
                                titleAr = "خطأ",
                                titleEn = "Error"
                            }
                        };
                }

                //if (request.gLBranchId == null)
                //    return new ResponseResult
                //    {
                //        Result = Result.Failed,
                //        Alart = new Alart
                //        {
                //            AlartType = AlartType.error,
                //            MessageAr = ErrorMessagesAr.DataRequired,
                //            MessageEn = ErrorMessagesEn.DataRequired,
                //            type = AlartShow.popup,
                //            titleAr = "خطأ",
                //            titleEn = "Error"
                //        }
                //    };
                if (request.gLBranchId != null)
                {
                    if (!branches.Any(c => c.Id == request.gLBranchId))
                        return new ResponseResult
                        {
                            Result = Result.Failed,
                            Alart = new Alart
                            {
                                AlartType = AlartType.error,
                                MessageAr = ErrorMessagesAr.branchNotExist,
                                MessageEn = ErrorMessagesEn.branchNotExist,
                                type = AlartShow.popup,
                                titleAr = "خطأ",
                                titleEn = "Error"
                            }
                        };

                }


                if (string.IsNullOrEmpty(request.Code.ToString()))
                    return new ResponseResult
                    {
                        Result = Result.Failed,
                        Alart = new Alart
                        {
                            AlartType = AlartType.error,
                            MessageAr = ErrorMessagesAr.CodeIsRequired,
                            MessageEn = ErrorMessagesEn.CodeIsRequired,
                            type = AlartShow.popup,
                            titleAr = "خطأ",
                            titleEn = "Error"
                        }
                    };
                if (request.shiftsMasterId == null)
                    return new ResponseResult
                    {
                        Result = Result.Failed,
                        Alart = new Alart
                        {
                            AlartType = AlartType.error,
                            MessageAr = ErrorMessagesAr.shiftIsRequest,
                            MessageEn = ErrorMessagesEn.shiftIsRequest,
                            type = AlartShow.popup,
                            titleAr = "خطأ",
                            titleEn = "Error"
                        }
                    };
                if (!ShiftsMaster.Any(c => c.Id == request.shiftsMasterId))
                    return new ResponseResult
                    {
                        Result = Result.Failed,
                        Alart = new Alart
                        {
                            AlartType = AlartType.error,
                            MessageAr = ErrorMessagesAr.shiftIsRequest,
                            MessageEn = ErrorMessagesEn.shiftIsRequest,
                            type = AlartShow.popup,
                            titleAr = "خطأ",
                            titleEn = "Error"
                        }
                    };
                if (request.SectionsId != null)
                    if (!sectionsAndDepartmentsQuery.Any(c => c.Id == request.SectionsId))
                        return new ResponseResult
                        {
                            Result = Result.Failed,
                            Alart = new Alart
                            {
                                AlartType = AlartType.error,
                                MessageAr = ErrorMessagesAr.sectionIsNotExist,
                                MessageEn = ErrorMessagesEn.sectionIsNotExist,
                                type = AlartShow.popup,
                                titleAr = "خطأ",
                                titleEn = "Error"
                            }
                        };
                if (request.DepartmentsId != null)
                    if (!sectionsAndDepartmentsQuery.Any(c => c.Id == request.DepartmentsId))
                        return new ResponseResult
                        {
                            Result = Result.Failed,
                            Alart = new Alart
                            {
                                AlartType = AlartType.error,
                                MessageAr = ErrorMessagesAr.DepartmentsNotExist,
                                MessageEn = ErrorMessagesEn.DepartmentsNotExist,
                                type = AlartShow.popup,
                                titleAr = "خطأ",
                                titleEn = "Error"
                            }
                        };
                if (request.ManagerId != null)
                    if (!employees.Any(c => c.Id == request.ManagerId))
                        return new ResponseResult
                        {
                            Result = Result.Failed,
                            Alart = new Alart
                            {
                                AlartType = AlartType.error,
                                MessageAr = ErrorMessagesAr.ManagerIsNotExist,
                                MessageEn = ErrorMessagesEn.ManagerIsNotExist,
                                type = AlartShow.popup,
                                titleAr = "خطأ",
                                titleEn = "Error"
                            }
                        };
                if (request.projectsId != null)
                    if (!projects.Any(c => c.Id == request.projectsId))
                        return new ResponseResult
                        {
                            Result = Result.Failed,
                            Alart = new Alart
                            {
                                AlartType = AlartType.error,
                                MessageAr = ErrorMessagesAr.projectIsNotExist,
                                MessageEn = ErrorMessagesEn.projectIsNotExist,
                                type = AlartShow.popup,
                                titleAr = "خطأ",
                                titleEn = "Error"
                            }
                        };
                if (request.missionsId != null)
                    if (!missions.Any(c => c.Id == request.missionsId))
                        return new ResponseResult
                        {
                            Result = Result.Failed,
                            Alart = new Alart
                            {
                                AlartType = AlartType.error,
                                MessageAr = ErrorMessagesAr.missionIsNotExist,
                                MessageEn = ErrorMessagesEn.missionIsNotExist,
                                type = AlartShow.popup,
                                titleAr = "خطأ",
                                titleEn = "Error"
                            }
                        };
                if (request.nationalityId != null)
                    if (!Nationality.Any(c => c.Id == request.nationalityId))
                        return new ResponseResult
                        {
                            Result = Result.Failed,
                            Alart = new Alart
                            {
                                AlartType = AlartType.error,
                                MessageAr = ErrorMessagesAr.NationalityIsNotExist,
                                MessageEn = ErrorMessagesEn.NationalityIsNotExist,
                                type = AlartShow.popup,
                                titleAr = "خطأ",
                                titleEn = "Error"
                            }
                        };
                if (request.religionsId != null)
                    if (!religions.Any(c => c.Id == request.religionsId))
                        return new ResponseResult
                        {
                            Result = Result.Failed,
                            Alart = new Alart
                            {
                                AlartType = AlartType.error,
                                MessageAr = ErrorMessagesAr.religionIsNotExist,
                                MessageEn = ErrorMessagesEn.religionIsNotExist,
                                type = AlartShow.popup,
                                titleAr = "خطأ",
                                titleEn = "Error"
                            }
                        };
                if (request.shiftType == (int)shiftTypes.ChangefulTime)
                    if (request.groupId != null && request.groupId != 0)
                    {
                        if (!_ChangefulTimeGroupsMasterQuery.TableNoTracking.Any(c => c.Id == request.groupId.Value))
                            return new ResponseResult
                            {
                                Result = Result.Failed,
                                Alart = new Alart
                                {
                                    AlartType = AlartType.error,
                                    MessageAr = ErrorMessagesAr.shiftGroupNotExist,
                                    MessageEn = ErrorMessagesEn.shiftGroupNotExist,
                                    type = AlartShow.popup,
                                    titleAr = "خطأ",
                                    titleEn = "Error"
                                }
                            };
                    }
            }


            var jobs = _InvJobsQuery.TableNoTracking;
            if (!jobs.Any(c => c.Id == request.JobId))
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        MessageAr = ErrorMessagesAr.jobisNotExist,
                        MessageEn = ErrorMessagesEn.jobisNotExist,
                        type = AlartShow.popup,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };


            return null;
        }
        public async Task<ResponseResult> UpdateEmployees(EmployeesRequestDTOs.Update request)
        {
            var security = await _securityIntegrationService.getCompanyInformation();
            var isValied = await IsValied(request, security, Id: request.Id, isEditMode: true);
            if (isValied != null)
                return isValied;

            if (request.LatinName != null)
            {

                if (string.IsNullOrEmpty(request.LatinName.Trim()))
                    request.LatinName = request.ArabicName;
            }
            else
                request.LatinName = request.ArabicName;

            var imagePath = string.Empty;
            if (request.Image != null)
            {

                imagePath = _fileHandler.SaveImage(request.Image, "Employees", true);
            }


            var userInfo = await _iUserInformation.GetUserInformation();

            var linkingMethodId = _gLIntegrationSettingsQuery
                .TableNoTracking
                .Where(x => x.GLBranchId == userInfo.CurrentbranchId)
                .Where(x => x.screenId == (int)SubFormsIds.Employees_MainUnits).FirstOrDefault().linkingMethodId;

            EmployeesRepositoryCommand.StartTransaction();
            var element = EmployeesRepositoryQuery.TableNoTracking.Where(c => c.Id == request.Id).FirstOrDefault();
            if (element == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        MessageAr = ErrorMessagesAr.employeeNotExist,
                        MessageEn = ErrorMessagesEn.employeeNotExist,
                        type = AlartShow.popup,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            var newElement = new InvEmployees
            {
                Id = element.Id,


                //Employee Definition
                Code = security.apps.Any(c => c.Id == (int)applicationIds.AttendLeaving) ? request.Code.Value : element.Code,
                ArabicName = request.ArabicName.Trim(),
                LatinName = request.LatinName.Trim(),
                Status = request.Status,
                JobId = request.JobId,
                ImagePath = imagePath,
                Image = request.Image,
                Notes = request.Notes,
                address=request.address??"",
                //Hiring Information.
                gLBranchId = request.gLBranchId != null ? request.gLBranchId.Value : 1,
                shiftsMasterId = request.shiftsMasterId,
                SectionsId = request.SectionsId,
                DepartmentsId = request.DepartmentsId,
                employeesGroupId = request.employeesGroupId,
                ManagerId = request.ManagerId,
                projectsId = request.projectsId,
                missionsId = request.missionsId,

                //General Leadger
                FinancialAccountId = linkingMethodId == 1 ? request.FinancialAccountId : element.FinancialAccountId,


                //Offline POS
                UTime = DateTime.Now,

                //Person Information
                nationalityId = request.nationalityId,
                IDNumber = request.IDNumber,
                religionsId = request.religionsId,
                birthday = request.birthday,
                phone = request.phone,
                email = request.email,


                //Attend Leaving Settings 
                Deduction_of_delay_from_additional_time = request.Deduction_of_delay_from_additional_time,
                Calculating_extra_time_before_work = request.Calculating_extra_time_before_work,
                Adding_working_hours_on_vacations = request.Adding_working_hours_on_vacations,
                Auto_Dismissal_registration = request.Auto_Dismissal_registration,
                Calculating_extra_time_after_work = request.Calculating_extra_time_after_work
            };
            var saved = await EmployeesRepositoryCommand.UpdateAsyn(newElement);
            if (saved)
            {
                var branchesDeleted = true;
                if (element.Status != (int)Status.newElement)
                    branchesDeleted = await EmployeeBranchRepositoryCommand.DeleteAsync(c => c.EmployeeId == newElement.Id);

                if (branchesDeleted)
                {
                    int[] branches = null;
                    var allBranchesIds = branchesRepositoryQuery.TableNoTracking.Select(c => c.Id).ToArray();
                    if (!string.IsNullOrEmpty(request.branches.Trim()) && request.branches != "0")
                    {
                        var arrbranches = request.branches.Split(',').Select(c => int.Parse(c)).ToArray();
                        branches = allBranchesIds.Where(c => arrbranches.Contains(c)).ToArray();
                    }
                    else if (request.branches == "0")
                        branches = allBranchesIds;

                    var listOfEmployeeBranches = new List<InvEmployeeBranch>();
                    foreach (var item in branches)
                    {
                        listOfEmployeeBranches.Add(new InvEmployeeBranch
                        {
                            BranchId = item,
                            EmployeeId = newElement.Id
                        });

                    }
                    var branchesSaved = await EmployeeBranchRepositoryCommand.AddAsync(listOfEmployeeBranches);
                    if (branchesSaved)
                    {
                        await _ChangefulTimeGroupsEmployeesCommand.DeleteAsync(c => c.invEmployeesId == newElement.Id);
                        if (request.shiftType == (int)shiftTypes.ChangefulTime)
                        {
                            _ChangefulTimeGroupsEmployeesCommand.Add(new ChangefulTimeGroupsEmployees
                            {
                                changefulTimeGroupsMasterId = request.groupId.Value,
                                invEmployeesId = newElement.Id
                            });
                            var ChangefulTimeGroupsSaved = await _ChangefulTimeGroupsEmployeesCommand.SaveAsync();
                            if (!ChangefulTimeGroupsSaved)
                            {
                                EmployeesRepositoryCommand.Rollback();
                                return new ResponseResult() { Result = Result.Failed, Note = "Can not save changeful shit" };
                            }
                        }
                        history.AddHistory(newElement.Id, newElement.LatinName, newElement.ArabicName, Aliases.HistoryActions.Update, Aliases.TemporaryRequiredData.UserName);
                        await _systemHistoryLogsService.SystemHistoryLogsService(Domain.Enums.SystemActionEnum.editEmployee);

                        try
                        {

                            EmployeesRepositoryCommand.CommitTransaction();

                            return new ResponseResult()
                            {
                                Data = null,
                                Id = newElement.Id,
                                Result = branchesSaved ? Result.Success : Result.Failed,
                                Alart = new Alart
                                {
                                    AlartType = AlartType.success,
                                    type = AlartShow.note,
                                    MessageAr = ErrorMessagesAr.SaveSuccessfully,
                                    MessageEn = ErrorMessagesEn.SaveSuccessfully,
                                    titleAr = "save",
                                    titleEn = "save"
                                }
                            };



                        }
                        catch (Exception ex)
                        {
                            EmployeesRepositoryCommand.Rollback();
                            return new ResponseResult() { Result = Result.Failed, Note = ex.Message };

                        }
                    }
                    else
                    {
                        EmployeesRepositoryCommand.Rollback();
                        return new ResponseResult() { Result = Result.Failed };
                    }
                }
                {
                    EmployeesRepositoryCommand.Rollback();
                    return new ResponseResult() { Result = Result.Failed };
                }
            }
            else
            {
                EmployeesRepositoryCommand.Rollback();
                return new ResponseResult() { Result = Result.Failed };
            }

            return null;


        }
        public async Task<ResponseResult> AddEmployee(EmployeesRequestDTOs.Add request)
        {
            var security = await _securityIntegrationService.getCompanyInformation();
            var isValied = await IsValied(request, security);
            if (isValied != null)
                return isValied;
            if (request.LatinName != null)
            {

                if (string.IsNullOrEmpty(request.LatinName.Trim()))
                    request.LatinName = request.ArabicName;
            }
            else
                request.LatinName = request.ArabicName;



            var imagePath = string.Empty;
            if (request.Image != null)
            {

                imagePath = _fileHandler.SaveImage(request.Image, "Employees", true);
            }

            if (!security.apps.Any(c => c.Id == (int)applicationIds.AttendLeaving))
                request.Code = EmployeesRepositoryQuery.GetMaxCode(e => e.Code) + 1;

            int[] branches = null;
            var allBranchesIds = branchesRepositoryQuery.TableNoTracking.Select(c => c.Id).ToArray();
            if (!string.IsNullOrEmpty(request.branches.Trim()) && request.branches != "0")
            {
                var arrbranches = request.branches.Split(',').Select(c => int.Parse(c)).ToArray();
                branches = allBranchesIds.Where(c => arrbranches.Contains(c)).ToArray();
            }
            else if (request.branches == "0")
                branches = allBranchesIds;

            EmployeesRepositoryCommand.StartTransaction();

            //var FinancialAccoun = await _iGLFinancialAccountRelation.GLRelation(GLFinancialAccountRelation.employee, request.FinancialAccountId ?? 0, branches, request.ArabicName, request.LatinName);
            //if (FinancialAccoun.Result != Result.Success)
            //    return FinancialAccoun;
            //request.FinancialAccountId = FinancialAccoun.Id;

            var table = new InvEmployees
            {
                //Employee Definition
                Code = request.Code.Value,
                Status = request.Status,
                ArabicName = request.ArabicName.Trim(),
                LatinName = request.LatinName.Trim(),
                JobId = request.JobId,
                ImagePath = imagePath,
                Image = request.Image,
                Notes = request.Notes,




                //Hiring Information.
                gLBranchId = request.gLBranchId != null ? request.gLBranchId.Value : 1,
                shiftsMasterId = request.shiftsMasterId,
                SectionsId = request.SectionsId,
                DepartmentsId = request.DepartmentsId,
                employeesGroupId = request.employeesGroupId,
                ManagerId = request.ManagerId,
                projectsId = request.projectsId,
                missionsId = request.missionsId,


                //General Leadger
                /*FinancialAccountId = request.FinancialAccountId*/

                address=request.address,

                //Offline POS
                UTime = DateTime.Now,

                //Person Information
                nationalityId = request.nationalityId,
                IDNumber = request.IDNumber,
                religionsId = request.religionsId,
                birthday = request.birthday,
                phone = request.phone,
                email = request.email,

                
                //Attend Leaving Settings 
                Deduction_of_delay_from_additional_time = request.Deduction_of_delay_from_additional_time,
                Calculating_extra_time_before_work = request.Calculating_extra_time_before_work,
                Calculating_extra_time_after_work = request.Calculating_extra_time_after_work,
                Adding_working_hours_on_vacations = request.Adding_working_hours_on_vacations,
                Auto_Dismissal_registration = request.Auto_Dismissal_registration
            };
            var saved = await EmployeesRepositoryCommand.AddAsync(table);

            if (saved)
            {
                var listOfEmployeeBranches = new List<InvEmployeeBranch>();
                foreach (var item in branches)
                {
                    listOfEmployeeBranches.Add(new InvEmployeeBranch
                    {
                        BranchId = item,
                        EmployeeId = table.Id
                    });

                }
                var branchesSaved = await EmployeeBranchRepositoryCommand.AddAsync(listOfEmployeeBranches);
                if (branchesSaved)
                {
                    history.AddHistory(table.Id, table.LatinName, table.ArabicName, Aliases.HistoryActions.Add, Aliases.TemporaryRequiredData.UserName);
                    await _systemHistoryLogsService.SystemHistoryLogsService(Domain.Enums.SystemActionEnum.addEmployee);
                }
                await _ChangefulTimeGroupsEmployeesCommand.DeleteAsync(c => c.invEmployeesId == table.Id);
                if (request.shiftType == (int)shiftTypes.ChangefulTime)
                {
                    _ChangefulTimeGroupsEmployeesCommand.Add(new ChangefulTimeGroupsEmployees
                    {
                        changefulTimeGroupsMasterId = request.groupId.Value,
                        invEmployeesId = table.Id
                    });
                    var ChangefulTimeGroupsSaved = await _ChangefulTimeGroupsEmployeesCommand.SaveAsync();
                    if (!ChangefulTimeGroupsSaved)
                    {
                        EmployeesRepositoryCommand.Rollback();
                        return new ResponseResult() { Result = Result.Failed, Note = "Can not save changeful shit" };
                    }
                }
                try
                {
                    EmployeesRepositoryCommand.CommitTransaction();
                }
                catch (Exception ex)
                {
                    EmployeesRepositoryCommand.Rollback();
                    return new ResponseResult() { Data = null, Id = table.Id, Result = Result.Failed, Note = ex.Message };
                }
            }
            else
            {
                EmployeesRepositoryCommand.Rollback();
                //if (FinancialAccoun.Code == 2)
                //    _GLFinancialAccountCommand.DeleteAsync(c => c.Id == FinancialAccoun.Id);
                //return new ResponseResult() { Data = null, Id = table.Id, Result = Result.Failed };
            }

            return new ResponseResult()
            {
                Result = Result.Success,
                Alart = new Alart
                {
                    AlartType = AlartType.success,
                    type = AlartShow.note,
                    MessageAr = ErrorMessagesAr.SaveSuccessfully,
                    MessageEn = ErrorMessagesEn.SaveSuccessfully,
                    titleAr = "save",
                    titleEn = "save"
                }
            };
        }
        public async Task<ResponseResult> RaoufTest(EmployeesRequestDTOs.Search parameters, bool isPrint = false)
        {
            var userInfo = await _userinformation.GetUserInformation();
            var res = Enumerable.Empty<InvEmployees>().AsQueryable();
            if (parameters.IsSearcheData)
            {
                res = EmployeesRepositoryQuery.TableNoTracking
                                             .Include(navigationPropertyPath: e => e.EmployeeBranches)
                                             .Include(s => s.Job)
                                             .Include(x => x.FinancialAccount)
                                             .Where(a => parameters.EmployeeId > 0 ? (a.Id == parameters.EmployeeId) : true)
                                             .Where(a => !string.IsNullOrEmpty(parameters.Name) ? (a.Code.ToString().Contains(parameters.Name) || a.ArabicName.Contains(parameters.Name) || a.LatinName.Contains(parameters.Name)) : true)
                                             .Where(a => parameters.Status != 0 ? a.Status == parameters.Status : true);

            }

            else
            {
                if (parameters.Ids != null)
                {


                    string[] ids = parameters.Ids.Split(",");
                    foreach (var id in ids)
                    {
                        var item = EmployeesRepositoryQuery.TableNoTracking
                                                                .Include(navigationPropertyPath: e => e.EmployeeBranches)
                                                                .Include(s => s.Job)
                                                                .Include(x => x.FinancialAccount)
                                                                .Where(a => (Convert.ToInt32(id) > 0 ? (a.Id == Convert.ToInt32(id)) : false)).FirstOrDefault();
                        res = res.Append(item);
                    }
                }
                else return new ResponseResult { Result = Result.RequiredData };
            }



            res = res.Where(x => x.EmployeeBranches.Any(d => d.BranchId == 1));

            var allFAs = _gLFinancialAccountQuery.TableNoTracking;

            if (parameters.JobList != null && parameters.JobList.Count() > 0)
            {
                res = res.Where(u => parameters.JobList.Contains(u.JobId.Value));
            }
            if (parameters.BranchList != null && parameters.BranchList.Count() > 0)
            {
                res = res.Where(x => x.EmployeeBranches.Any(y => parameters.BranchList.Contains(y.BranchId)));
            }
            if (userInfo.employeeId != 1)
                res = res.Where(x => x.Id != 1);

            var result = res.ToList();
            var count = result.Count();

            result.Where(a => a.Id != 1).Select(a => { a.CanDelete = true; return a; }).ToList();

            var List = Mapping.Mapper.Map<List<InvEmployees>, List<EmployeeResponsesDTOs.GetAll>>(result.ToList());
            var offerPrices = _OfferPriceMasterQuery.TableNoTracking;
            foreach (var employee in List)
            {
                //employee.CanDelete = (Branches2.Select(e => e.ManagerId).ToArray().Contains(employee.Id))||employee.Code==1? false : true;
                employee.CanDelete = offerPrices.Where(x => x.EmployeeId == employee.Id).Any() ? false : true;
            }

            var _List = List.Select(x => new
            {
                x.Id,
                x.Code,
                x.ArabicName,
                x.LatinName,
                x.Status,
                x.Notes,
                x.ImagePath,
                x.Branches,
                x.BranchNameAr,
                x.BranchNameEn,
                x.JobId,
                x.JobNameAr,
                x.JobNameEn,
                x.JobStatus,
                x.CanDelete,
                FinancialAccountId = allFAs.Where(Acc => Acc.Id == x.FinancialAccountId).Select(Acc => new { Acc.Id, Acc.ArabicName, Acc.LatinName }).FirstOrDefault()
            }).ToList();
            if (isPrint)
            {
                return new ResponseResult() { Data = List, DataCount = count, Id = null, Result = List.Any() ? Result.Success : Result.Failed };

            }
            return new ResponseResult() { Data = _List, DataCount = count, Id = null, Result = List.Any() ? Result.Success : Result.Failed };









        }
        public async Task<ResponseResult> GetListOfEmployees(EmployeesRequestDTOs.Search parameters, bool isPrint = false
            ,bool getNonRegist=false)
        {
            var userInfo = await _userinformation.GetUserInformation();
            var res = Enumerable.Empty<InvEmployees>().AsQueryable();
            if (parameters.IsSearcheData)
            {
                res = EmployeesRepositoryQuery.TableNoTracking
                                             .Include(navigationPropertyPath: e => e.EmployeeBranches)
                                             .Include(s => s.Job)
                                             .Include(s => s.GLBranch)
                                             .Include(x => x.FinancialAccount)
                                             .Include(x=>x.religions)
                                             .Include(x=>x.shiftsMaster)
                                             .Include(x=>x.nationality)
                                             .Include(x=>x.projects)
                                             .Include(x=>x.missions)
                                             .Include(x=>x.Sections)
                                             .Include(x=>x.Departments)
                                             .Include(x=>x.employeesGroup)
                                             .Include(x=>x.FirstLogmachine)
                                             .Where(c => !getNonRegist? c.Status != (int)Status.newElement : c.Status == (int)Status.newElement)
                                             .Where(a => parameters.EmployeeId > 0 ? (a.Id == parameters.EmployeeId) : true)
                                             .Where(a => !string.IsNullOrEmpty(parameters.Name) ? (a.Code.ToString().Contains(parameters.Name) || a.ArabicName.Contains(parameters.Name) || a.LatinName.Contains(parameters.Name)) : true)
                                             .Where(a => parameters.Status != 0 ? a.Status == parameters.Status : true);

            }

            else
            {
                if (parameters.Ids != null)
                {


                    string[] ids = parameters.Ids.Split(",");
                    foreach (var id in ids)
                    {
                        var item = EmployeesRepositoryQuery.TableNoTracking
                                                                .Include(navigationPropertyPath: e => e.EmployeeBranches)
                                                                .Include(s => s.Job)
                                                                .Include(x => x.FinancialAccount)
                                                                .Where(a => (Convert.ToInt32(id) > 0 ? (a.Id == Convert.ToInt32(id)) : false)).FirstOrDefault();
                        res = res.Append(item);
                    }
                }
                else return new ResponseResult { Result = Result.RequiredData };
            }


            int[] shiftIds = null, depIds = null,secIds=null;
            if (!string.IsNullOrEmpty(parameters.ShiftList))
                shiftIds = Array.ConvertAll(parameters.ShiftList.Split(','), s => int.Parse(s));
            if (!string.IsNullOrEmpty(parameters.DepartmentList))
                depIds = Array.ConvertAll(parameters.DepartmentList.Split(','), s => int.Parse(s));
            if (!string.IsNullOrEmpty(parameters.SectiontList))
                secIds = Array.ConvertAll(parameters.SectiontList.Split(','), s => int.Parse(s));


            if (!getNonRegist)
                res = res.Where(x => x.EmployeeBranches.Select(d => d.BranchId).Any(c => userInfo.employeeBranches.Contains(c)));
            var allFAs = _gLFinancialAccountQuery.TableNoTracking;

            if (parameters.JobList != null && parameters.JobList.Count() > 0)
            {
                res = res.Where(u => parameters.JobList.Contains(u.JobId.Value));
            }
            if (parameters.BranchList != null && parameters.BranchList.Count() > 0)
            {
                res = res.Where(x => x.EmployeeBranches.Any(y => parameters.BranchList.Contains(y.BranchId)));
            }

            if (shiftIds != null && parameters.ShiftList!="0")
            {
                res = res.Where(u => shiftIds.Contains(u.shiftsMasterId.Value));
            }

            if (depIds != null && parameters.DepartmentList != "0")
            {
                res = res.Where(u => depIds.Contains(u.DepartmentsId.Value));
            }

            if (secIds != null && parameters.SectiontList != "0")
            {
                res = res.Where(u => secIds.Contains(u.SectionsId.Value));
            }

            if (userInfo.employeeId != 1)
                res = res.Where(x => x.Id != 1);
            res = (string.IsNullOrEmpty(parameters.Name) ? res.OrderByDescending(q => q.Code) : res.OrderBy(a => (a.Code.ToString().Contains(parameters.Name)) ? 0 : 1));
            var result = res.ToList();
            var count = result.Count();

            result = isPrint ? res.ToList() : res.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToList();


            if (parameters.PageNumber <= 0 && parameters.PageNumber <= 0 && isPrint == false)
            {
                return new ResponseResult() { Data = null, DataCount = 0, Id = null, Result = Result.Failed };
            }
            result.Where(a => a.Id != 1).Select(a => { a.CanDelete = true; return a; }).ToList();


            //mapping and handle conditions with mapping profile
            var List = Mapping.Mapper.Map<List<InvEmployees>, List<EmployeeResponsesDTOs.GetAll>>(result.ToList());

            //var List=result.ToList().Select(x=> new {
            //   Id= x.Id,
            //    code=x.Code,
            //    x.ArabicName,
            //    x.LatinName,
            //    x.Status,
            //    x.Notes,
            //    x.ImagePath,
            //    x.Branches,
            //    x.BranchNameAr,
            //    x.BranchNameEn,
            //    x.JobId,
            //    x.JobNameAr,
            //    x.JobNameEn,
            //    x.JobStatus,
            //    x.CanDelete,
            //    x.shiftsMasterId,

            //});
            var security = await _securityIntegrationService.getCompanyInformation();
            var offerPrices = _OfferPriceMasterQuery.TableNoTracking;
            foreach (var employee in List)
            {
                //employee.GLBranchDTO.GLBranchId = employee.GLBranch.Id;
                //employee.GLBranchDTO.GLBranchArabicName = employee.GLBranch.ArabicName;
                //employee.GLBranchDTO.GLBranchLatinName = employee.GLBranch.LatinName;
                var Branches2 = branchesRepositoryQuery.GetAll(e => employee.Branches.Contains(e.Id))
                                    .Select(e => new { e.ArabicName, e.LatinName, e.ManagerId }).ToList();
                employee.BranchNameAr = employee.Id == 1 ? "الكل" : string.Join(',', Branches2.Select(e => e.ArabicName).ToArray());
                employee.BranchNameEn = employee.Id == 1 ? "All" : string.Join(',', Branches2.Select(e => e.LatinName).ToArray());
                //employee.CanDelete = (Branches2.Select(e => e.ManagerId).ToArray().Contains(employee.Id))||employee.Code==1? false : true;
                employee.CanDelete = _useraccountQuery.TableNoTracking.Where(d => d.employeesId == employee.Id).Any() || branchesRepositoryQuery.TableNoTracking.Where(e => e.ManagerId == employee.Id).Any() || employee.Id == 1 || offerPrices.Where(x => x.EmployeeId == employee.Id).Any() ? false : true;

            }
            var _List = List.Select(x => new
            {
                x.Id,
                x.Code,
                x.ArabicName,
                x.LatinName,
                x.Status,
                x.Notes,
                x.ImagePath,
                x.Branches,
                x.BranchNameAr,
                x.BranchNameEn,
                x.JobId,
                x.JobNameAr,
                x.JobNameEn,
                x.JobStatus,
                x.CanDelete,
                x.shiftsMasterId,
               birthday= x.birthday?.ToString("yyyy-MM-dd")??"",
                x.address,
                x.phone,
                x.email,
                x.IDNumber,
                x.Adding_working_hours_on_vacations,
                x.Auto_Dismissal_registration,
                x.Deduction_of_delay_from_additional_time,
                x.Calculating_extra_time_after_work,
                x.Calculating_extra_time_before_work,

                DirectManger= EmployeesRepositoryQuery.TableNoTracking.Any(c=>c.Id==x.ManagerId)? new GLBranchDTO
                {
                    Id = EmployeesRepositoryQuery.TableNoTracking.FirstOrDefault(c => c.Id == x.ManagerId).Id,
                    ArabicName = EmployeesRepositoryQuery.TableNoTracking.FirstOrDefault(c => c.Id == x.ManagerId).ArabicName
               ,
                    LatinName = EmployeesRepositoryQuery.TableNoTracking.FirstOrDefault(c => c.Id == x.ManagerId).LatinName
                } : null,

                GLBranchDTO = security.apps.Any(c => c.Id == (int)applicationIds.AttendLeaving)? new GLBranchDTO { Id= x.GLBranch.Id,ArabicName=x.GLBranch.ArabicName
               ,LatinName=x.GLBranch.LatinName  
               }:null ,

               NationalityDTO= security.apps.Any(c => c.Id == (int)applicationIds.AttendLeaving) ? new GLBranchDTO
               {
                   Id = x.nationality?.Id??null,
                   ArabicName = x.nationality?.arabicName??""
               ,
                   LatinName = x.nationality?.latinName??""
               } : null ,
                ShifMasterDTO = security.apps.Any(c => c.Id == (int)applicationIds.AttendLeaving) ? new GLBranchDTO
                {
                    Id = x.shiftsMaster?.Id??null,
                    ArabicName = x.shiftsMaster?.arabicName??""
               ,
                    LatinName = x.shiftsMaster?.latinName??""
                } : null,
                ProjectsDTO = security.apps.Any(c => c.Id == (int)applicationIds.AttendLeaving)  ? new GLBranchDTO
                {
                    Id = x.projects?.Id??null,
                    ArabicName = x.projects?.arabicName??""
               ,
                    LatinName = x.projects?.latinName??""
                } : null,
               MissionsDTO = security.apps.Any(c => c.Id == (int)applicationIds.AttendLeaving) ? new GLBranchDTO
               {
                   Id = x.missions?.Id??null,
                   ArabicName = x.missions?.arabicName??""
               ,
                   LatinName = x.missions?.latinName??""
               } : null,
                SectionsDTO = security.apps.Any(c => c.Id == (int)applicationIds.AttendLeaving) ? new GLBranchDTO
                {
                    Id = x.Sections?.Id??null,
                    ArabicName = x.Sections?.arabicName??""
               ,
                    LatinName = x.Sections?.latinName??""
                } : null,
                DepartmentsDTO = security.apps.Any(c => c.Id == (int)applicationIds.AttendLeaving) ? new GLBranchDTO
                {
                    Id = x.Departments?.Id??null,
                    ArabicName = x.Departments?.arabicName??""
               ,
                    LatinName = x.Departments?.latinName??""
                } : null,
                ReligionDTO = security.apps.Any(c => c.Id == (int)applicationIds.AttendLeaving) ? new GLBranchDTO
                {
                    Id = x.religions?.Id??null,
                    ArabicName = x.religions?.arabicName??""
               ,
                    LatinName = x.religions?.latinName??""
                } : null,
                EmpGroupDTO = security.apps.Any(c => c.Id == (int)applicationIds.AttendLeaving) ? new GLBranchDTO
                {
                    Id = x.employeesGroup?.Id??null,
                    ArabicName = x.employeesGroup?.arabicName??""
               ,
                    LatinName = x.employeesGroup?.latinName??""
                } : null,
                Machine = getNonRegist ? new GetNonRegisteredEmployeesResponseDTO_Machine
                            {
                                arabicName = x.FirstLogmachine.arabicName,
                                latinName = x.FirstLogmachine.latinName,
                                Id = x.FirstLogmachine.Id
                            } : null,



                FinancialAccountId = allFAs.Where(Acc => Acc.Id == x.FinancialAccountId).Select(Acc => new { Acc.Id, Acc.ArabicName, Acc.LatinName }).FirstOrDefault()
            }).ToList();
            if (isPrint)
            {
                return new ResponseResult() { Data = List, DataCount = count, Id = null, Result = List.Any() ? Result.Success : Result.Failed };

            }
            return new ResponseResult() { Data = _List, DataCount = count, Id = null, Result = List.Any() ? Result.Success : Result.Failed };
            #region
            //var resData = await EmployeesRepositoryQuery.GetAllIncludingAsync(parameters.PageNumber, parameters.PageSize,
            //      a => ((a.Code.ToString().Contains(parameters.name) || string.IsNullOrEmpty(parameters.name) 
            //      || a.ArabicName.Contains(parameters.name) || a.LatinName.Contains(parameters.name))
            //      && (parameters.active == 0 || a.Active == parameters.active)),
            //      e => (parameters.name == "" ? e.OrderByDescending(q => q.EmploeeId) : e.OrderBy(a => a.EmploeeId)), e => e.branch, x => x.job);

            // var list = new List<EmployeeWithImgDto>();

            //foreach (var item in result)
            //{
            //    var dataDto = new EmployeeWithImgDto();
            //    dataDto.EmploeeId = item.EmployeeId;
            //    dataDto.ArabicName = item.ArabicName;
            //    dataDto.LatinName = item.LatinName;

            //    // var branch = BranchRepositoryQuery.Get( item.branch_Id);
            //    dataDto.BranchId = item.branch.BranchId;
            //    dataDto.BranchNameAr = item.branch.NameAr;
            //    dataDto.BranchNameEn = item.branch.NameEn;

            //    // var job = JobRepositoryQuery.Get(item.job_Id);
            //    dataDto.JobId = item.job.JobId;
            //    dataDto.JobNameAr = item.job.ArabicName;
            //    dataDto.JobNameEn = item.job.LatinName;
            //    dataDto.Active = item.Active;
            //    dataDto.Code = item.Code;
            //    dataDto.CanDelete = item.EmployeeId != 1;
            //    if (item.job != null || item.branch != null)
            //        dataDto.CanDelete = false ;

            //    if (parameters.EmployeeId>0)
            //    { 
            //        dataDto.Image = item.Image;
            //        dataDto.ImageName = item.ImageName; 
            //    } 


            //    list.Add(dataDto);
            //}
            //var count = EmployeesRepositoryQuery.Count(
            //    a => ((a.Code.ToString().Contains(parameters.name) || string.IsNullOrEmpty(parameters.name)
            //     || a.ArabicName.Contains(parameters.name) || a.LatinName.Contains(parameters.name))
            //     && (parameters.active == 0 || a.Active == parameters.active)) );
            #endregion
        }
        public async Task<WebReport> EmployeesReport(EmployeesRequestDTOs.Search parameters, exportType exportType, bool isArabic, int fileId = 0)
        {

            var data = await GetListOfEmployees(parameters, true);

            var userInfo = await _iUserInformation.GetUserInformation();


            var mainData = (List<EmployeeResponsesDTOs.GetAll>)data.Data;
            foreach (var item in mainData)
            {
                if (item.Status == 1)
                {
                    item.StatusAr = "نشط";
                    item.StatusEn = "Active";

                }
                else if (item.Status == 2)
                {
                    item.StatusAr = "غير نشط";
                    item.StatusEn = "InActive";
                }
            }

            var otherdata = new ReportOtherData()
            {
                EmployeeName = userInfo.employeeNameAr.ToString(),
                EmployeeNameEn = userInfo.employeeNameEn.ToString(),
                Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")

            };




            int screenId = (int)SubFormsIds.Employees_MainUnits;
            var tablesNames = new TablesNames()
            {
                FirstListName = "Employees"
            };

            var report = await _iGeneralPrint.PrintReport<object, EmployeeResponsesDTOs.GetAll, object>(null, mainData, null, tablesNames, otherdata
                , screenId, exportType, isArabic, fileId);
            return report;

        }
        public async Task<ResponseResult> UpdateStatus(SharedRequestDTOs.UpdateStatus parameters)
        {

            if (parameters.Id.Count() == 0)
                return new ResponseResult() { Data = null, Id = null, Result = Result.RequiredData, Note = Actions.IdIsRequired };

            if (parameters.Status < (int)Status.Active || parameters.Status > (int)Status.Inactive)
            {
                return new ResponseResult { Result = Result.Failed, Note = Actions.InvalidStatus };
            }


            var Employees = EmployeesRepositoryQuery.TableNoTracking.Where(e => parameters.Id.Contains(e.Id));
            var EmployeeList = Employees.ToList();
            EmployeeList.Select(e => { e.Status = parameters.Status; return e; }).ToList();
            if (parameters.Id.Contains(1))
                EmployeeList.Where(q => q.Id == 1).Select(e => { e.Status = (int)Status.Active; return e; }).ToList();
            var rssult = await EmployeesRepositoryCommand.UpdateAsyn(EmployeeList);
            foreach (var Employee in EmployeeList)
            {
                history.AddHistory(Employee.Id, Employee.LatinName, Employee.ArabicName, Aliases.HistoryActions.Add, Aliases.TemporaryRequiredData.UserName);
            }
            await _systemHistoryLogsService.SystemHistoryLogsService(Domain.Enums.SystemActionEnum.editEmployee);
            return new ResponseResult() { Data = null, Id = null, Result = Result.Success };

        }
        public async Task<ResponseResult> DeleteEmployees(SharedRequestDTOs.Delete ListCode)
        {

            UserInformationModel userInfo = await _userinformation.GetUserInformation();
            if (userInfo == null)
                return new ResponseResult()
                {
                    Note = Actions.JWTError,
                    Result = Result.Failed
                };


            var EmployeesCanBeDeleted = EmployeesRepositoryQuery.TableNoTracking
                .Include(x => x.OfferPriceMaster)
                .Where(x => ListCode.Ids.Contains(x.Id) && !x.userAccount.Any())
                .Where(x => !x.OfferPriceMaster.Any());


            var branches = await branchesRepositoryQuery.Get(e => e.ManagerId, a => a.ManagerId != null && a.ManagerId != 0);
            List<int> deletedList = new List<int>();
            List<int> FA_Ids = new List<int>();
            EmployeesCanBeDeleted.ToList().ForEach(emp =>
            {
                if (!branches.Contains(emp.Id))
                {
                    FA_Ids.Add(emp.FinancialAccountId ?? 0);
                    deletedList.Add(emp.Id);
                    EmployeesRepositoryCommand.DeleteAsync(emp.Id).Wait();
                    EmployeesRepositoryCommand.SaveAsync().Wait();
                }
            });

            var FA_Deleted = await _financialAccountBusiness.DeleteFinancialAccountAsync(new SharedRequestDTOs.Delete()
            {
                userId = userInfo.userId,
                Ids = FA_Ids.ToArray()
            });
            if (deletedList.Any())
                await _systemHistoryLogsService.SystemHistoryLogsService(Domain.Enums.SystemActionEnum.deleteEmployee);

            //Fill The DeletedRecordTable
            _deletedRecords.SetDeletedRecord(deletedList.ToList(), 7);

            return new ResponseResult() { Data = deletedList, Id = null, Result = deletedList.Any() ? Result.Success : Result.NotFound };
        }
        public async Task<ResponseResult> GetEmployeeHistory(int EmployeeId)
        {
            return await history.GetHistory(a => a.EntityId == EmployeeId);
        }
        public async Task<ResponseResult> GetEmployeeDropDown(bool isInUserPage, int? employeeId, string SearchCriteria, int pageSize, int pageNumber, bool isReport = false)
        {
            var userInfo = await _userinformation.GetUserInformation();
            var dropdownlist = EmployeesRepositoryQuery.TableNoTracking
                                            .Include(x => x.EmployeeBranches)
                                            .Where(c => c.Status != (int)Status.newElement)
                                            .Where(e => !isReport ? e.Status == (int)Status.Active : true)
                                            .Where(x => x.EmployeeBranches.Select(d => d.BranchId).Any(c => userInfo.employeeBranches.Contains(c)))
                                            .Select(e => new { e.Id, e.ArabicName, e.LatinName, e.Status,e.Code });
            if (!string.IsNullOrEmpty(SearchCriteria))
            {
                dropdownlist = dropdownlist.Where(x => x.Code.ToString().Contains(SearchCriteria) || x.ArabicName.Contains(SearchCriteria) || x.LatinName.Contains(SearchCriteria));
            }



            double MaxPageNumber = dropdownlist.Count() / Convert.ToDouble(pageSize);
            var countofFilter = Math.Ceiling(MaxPageNumber);
            var EndOfData = (countofFilter == pageNumber ? Actions.EndOfData : "");
            dropdownlist = dropdownlist.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            if (isInUserPage)
            {
                var employeeUsedForUsers = _useraccountQuery.TableNoTracking.Where(x => x.employeesId != employeeId).Select(x => x.employeesId).ToList();

                dropdownlist = dropdownlist.Where(x => !employeeUsedForUsers.Contains(x.Id) && x.Status == 1);
            }
            return new ResponseResult() { Note = EndOfData, Data = dropdownlist, DataCount = dropdownlist.Count(), Result = dropdownlist.Any() ? Result.Success : Result.Failed };
        }
        public async Task<ResponseResult> GetEmployeesByDate(DateTime date, int PageNumber, int PageSize)
        {
            try
            {
                var resData = await EmployeesRepositoryQuery.TableNoTracking
                    .Where(c => c.Status != (int)Status.newElement)
                    .Where(q => q.UTime >= date).ToListAsync();

                return await generalAPIsService.Pagination(resData, PageNumber, PageSize);

            }
            catch (Exception ex)
            {
                return new ResponseResult() { Data = null, Id = null, Result = Result.NotFound };


            }
        }
        public async Task<ResponseResult> GetEmployeesById(int Id)
        {
            var element = EmployeesRepositoryQuery
                   .TableNoTracking
                   .Include(c => c.Job)
                   .Include(c => c.EmployeeBranches)
                   .Include(c => c.GLBranch)
                   .Include(c => c.shiftsMaster)
                   .ThenInclude(c => c.changefulTimeGroups)
                   .Include(c => c.Sections)
                   .Include(c => c.Departments)
                   .Include(c => c.employeesGroup)
                   .Include(c => c.missions)
                   .Include(c => c.projects)
                   .Include(c => c.nationality)
                   .Include(c => c.religions)
                   .Where(c => c.Id == Id)
                   .FirstOrDefault();
            if (element == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        MessageAr = ErrorMessagesAr.employeeNotExist,
                        MessageEn = ErrorMessagesEn.employeeNotExist,
                        titleAr = "خطأ",
                        titleEn = "Error",
                        type = AlartShow.popup
                    }
                };
            var manager =
                EmployeesRepositoryQuery
                   .TableNoTracking.Where(c => c.Id == element.ManagerId).FirstOrDefault();
            GetEmployeeByIdResponseDTO response = new GetEmployeeByIdResponseDTO
            {
                EmployeeDefinition = new GetEmployeeByIdResponseDTO_EmployeeDefinition
                {
                    Id = element.Id,
                    ArabicName = element.ArabicName,
                    LatinName = element.LatinName,
                    Code = element.Code,
                    Status = element.Status,
                    ImageURL = element.ImagePath,
                    Notes = element.Notes,
                    Job = element.Job != null ? new EmployeeDefinition_Job
                    {
                        Id = element.JobId.Value,
                        arabicName = element.Job.ArabicName,
                        latinName = element.Job.LatinName
                    } : null,
                    FinancialAccount = element.FinancialAccount != null ? new EmployeeDefinition_FinancialAccoun
                    {
                        Id = element.FinancialAccount.Id,
                        arabicName = element.FinancialAccount.ArabicName,
                        latinName = element.FinancialAccount.LatinName
                    } : null
                },
                HiringInformation = new GetEmployeeByIdResponseDTO_HiringInformation
                {
                    branch = element.GLBranch != null ? new HiringInformation_Branch
                    {
                        Id = element.gLBranchId,
                        arabicName = element.GLBranch.ArabicName,
                        latinName = element.GLBranch.LatinName,
                    } : null,
                    shift = element.shiftsMaster != null ? new HiringInformation_shift
                    {
                        Id = element.shiftsMaster.Id,
                        groupId = element.shiftsMaster.shiftType != (int)shiftTypes.ChangefulTime ? 0 : element.shiftsMaster.changefulTimeGroups.FirstOrDefault().Id,
                        arabicName = element.shiftsMaster.shiftType != (int)shiftTypes.ChangefulTime ? element.shiftsMaster.arabicName : element.shiftsMaster.arabicName + " - " + element.shiftsMaster.changefulTimeGroups.FirstOrDefault().arabicName,
                        latinName = element.shiftsMaster.shiftType != (int)shiftTypes.ChangefulTime ? element.shiftsMaster.latinName : element.shiftsMaster.latinName + " - " + element.shiftsMaster.changefulTimeGroups.FirstOrDefault().latinName,
                    } : null,
                    Section = element.Sections != null ? new HiringInformation_Section
                    {
                        Id = element.Sections.Id,
                        arabicName = element.Sections.arabicName,
                        latinName = element.Sections.latinName
                    } : null,
                    Department = element.Departments != null ? new HiringInformation_Department
                    {
                        Id = element.Departments.Id,
                        arabicName = element.Departments.arabicName,
                        latinName = element.Departments.latinName
                    } : null,
                    employeesGroup = element.employeesGroup != null ? new HiringInformation_employeesGroup
                    {
                        Id = element.employeesGroup.Id,
                        arabicName = element.employeesGroup.arabicName,
                        latinName = element.employeesGroup.latinName
                    } : null,
                    Manager = manager != null ? new HiringInformation_Manager
                    {
                        Id = manager.Id,
                        arabicName = manager.ArabicName,
                        latinName = manager.LatinName
                    } : null,
                    projects = element.projects != null ? new HiringInformation_project
                    {
                        Id = element.projects.Id,
                        arabicName = element.projects.arabicName,
                        latinName = element.projects.latinName

                    } : null,
                    missions = element.missions != null ? new HiringInformation_missions
                    {
                        Id = element.missions.Id,
                        arabicName = element.missions.arabicName,
                        latinName = element.missions.latinName
                    } : null,

                },
                PersonInformation = new GetEmployeeByIdResponseDTO_PersonInformation
                {
                    nationality = element.nationality != null ? new PersonInformation_nationality
                    {
                        Id = element.nationality.Id,
                        arabicName = element.nationality.arabicName,
                        latinName = element.nationality.latinName
                    } : null,
                    religions = element.religions != null ? new PersonInformation_religions
                    {
                        Id = element.religions.Id,
                        arabicName = element.religions.arabicName,
                        latinName = element.religions.latinName
                    } : null,
                    birthday = element.birthday,
                    email = element.email,
                    IDNumber = element.IDNumber,
                    phone = element.phone
                },
                AttendLeavingSettings = new GetEmployeeByIdResponseDTO_AttendLeavingSettings
                {
                    Adding_working_hours_on_vacations = element.Adding_working_hours_on_vacations,
                    Auto_Dismissal_registration = element.Auto_Dismissal_registration,
                    Calculating_extra_time_after_work = element.Calculating_extra_time_after_work,
                    Calculating_extra_time_before_work = element.Calculating_extra_time_before_work,
                    Deduction_of_delay_from_additional_time = element.Deduction_of_delay_from_additional_time
                }
            };
            return new ResponseResult
            {
                Result = Result.Success,
                Data = response
            };
        }
        public async Task<ResponseResult> GetNonRegisteredEmployees(NonRegisteredEmployeesDTO request)
        {
            var userInfo = await _userinformation.GetUserInformation();
            var machines = _MachinesQuery.TableNoTracking;
            int[] machineIds = null;
            if (!string.IsNullOrEmpty(request.machineName))
            {
                machineIds = machines.Where(c => request.machineName.Contains(c.arabicName) || request.machineName.Contains(c.latinName)).Select(c => c.Id).ToArray();
            }
            var AllData = EmployeesRepositoryQuery.TableNoTracking
                                         .Include(navigationPropertyPath: e => e.EmployeeBranches)
                                         .Include(s => s.Job)
                                         .Include(s => s.FirstLogmachine)
                                         .Include(x => x.FinancialAccount)
                                         .Where(c => c.Status == (int)Status.newElement);

            var totalCount = AllData.Count();

            var filteredData = AllData.Where(c => !string.IsNullOrEmpty(request.searchCriteria) ? c.Code.ToString().Contains(request.searchCriteria) : true)
                                      .Where(c => machineIds != null ? (c.FirstLogmachineId != null ? machineIds.Contains(c.FirstLogmachineId.Value) : true) : true);

            var dataCount = filteredData.Count();
            var response = filteredData
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .Select(c => new GetNonRegisteredEmployeesResponseDTO
                        {
                            canDelete = true,
                            Employee = new GetNonRegisteredEmployeesResponseDTO_Employee
                            {
                                Id = c.Id,
                                arabicName = c.ArabicName,
                                latinName = c.LatinName,
                                code = c.Code
                            },
                            Machine = new GetNonRegisteredEmployeesResponseDTO_Machine
                            {
                                Id = c.FirstLogmachine.Id,
                                arabicName = c.FirstLogmachine.arabicName ?? "",
                                latinName = c.FirstLogmachine.latinName ?? "",
                            }
                        });
            return new ResponseResult
            {
                Data = response,
                TotalCount = totalCount,
                DataCount = dataCount,
                Note = Aliases.GetEndOfData(request.PageSize, dataCount, request.PageNumber)
            };
        }
        public async Task<ResponseResult> DeleteNonRegisteredEmployees(string Ids)
        {
            if (string.IsNullOrEmpty(Ids.Trim()))
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.DataRequired,
                        MessageEn = ErrorMessagesEn.DataRequired,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };

            int[] EmployeeIds = Ids.Split(',').Select(c => int.Parse(c)).ToArray();
            var deleted = await EmployeesRepositoryCommand.DeleteAsync(c => EmployeeIds.Contains(c.Id));
            return new ResponseResult
            {
                Result = deleted ? Result.Success : Result.Failed,
                Alart = deleted ? new Alart
                {
                    AlartType = AlartType.success,
                    type = AlartShow.note,
                    MessageAr = ErrorMessagesAr.DeletedSuccessfully,
                    MessageEn = ErrorMessagesEn.DeletedSuccessfully
                } :
                new Alart
                {
                    AlartType = AlartType.error,
                    type = AlartShow.popup,
                    MessageAr = ErrorMessagesAr.ErrorSaving,
                    MessageEn = ErrorMessagesEn.ErrorSaving
                }
            };
        }
    }
}
