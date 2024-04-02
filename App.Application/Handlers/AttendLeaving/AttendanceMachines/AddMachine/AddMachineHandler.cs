using App.Application.Services.HelperService.SecurityIntegrationServices;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Data.SqlClient;


namespace App.Application.Handlers.AttendLeaving.AttendanceMachines.AddMachine
{
    public class AddMachineHandler : IRequestHandler<AddMachineRequest,ResponseResult>
    {
        private readonly IRepositoryCommand<Machines> _MachinesCommand;
        private readonly IRepositoryQuery<Machines> _MachinesQuery;
        private readonly IRepositoryCommand<MachineTransactions> _MachineTransactionsCommand;
        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;
        private readonly IRepositoryCommand<InvEmployees> _InvEmployeesCommand;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly ISecurityIntegrationService securityIntegration;
        public AddMachineHandler(IRepositoryCommand<Machines> machinesCommand, IRepositoryQuery<GLBranch> gLBranchQuery, IConfiguration configuration, ISecurityIntegrationService securityIntegration, IRepositoryCommand<MachineTransactions> machineTransactionsCommand, IRepositoryQuery<Machines> machinesQuery, IMediator mediator, IRepositoryCommand<InvEmployees> invEmployeesCommand, IRepositoryQuery<InvEmployees> invEmployeesQuery = null)
        {
            _MachinesCommand = machinesCommand;
            _GLBranchQuery = gLBranchQuery;
            _configuration = configuration;
            this.securityIntegration = securityIntegration;
            _MachineTransactionsCommand = machineTransactionsCommand;
            _MachinesQuery = machinesQuery;
            _mediator = mediator;
            _InvEmployeesCommand = invEmployeesCommand;
            _InvEmployeesQuery = invEmployeesQuery;
        }

        

        public async Task<ResponseResult> Handle(AddMachineRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.MachineSN))
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "يجب ادخال الرقم المسلسل الخاص بجهاز الحضور و الانصراف",
                        MessageEn = "You have to add machine serial number",
                        titleAr = "خطأ",
                        titleEn = "Error" 
                    }
                };
            var checkBranch = _GLBranchQuery.TableNoTracking.Where(c => c.Id == request.branchId);
            if(checkBranch == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "هذا الفرع غير موجود",
                        MessageEn = "This branch is not exist",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            var companyInfo = await securityIntegration.getCompanyInformation();
            SqlConnection con = new SqlConnection(_configuration["ConnectionStrings:UserManagerConnection"]);
            con.Open();
            bool saved = false;
            try
            {
                var checkDeviceSN = await MachineHelper.checkMachineSNIsValid(con, request.MachineSN, _MachinesQuery, companyInfo);
                if (checkDeviceSN != null)
                    return checkDeviceSN;
                if (string.IsNullOrEmpty(request.arabicName.Trim()))
                    return new ResponseResult
                    {
                        Result = Result.Failed,
                        Alart = new Alart
                        {
                            AlartType = AlartType.error,
                            type = AlartShow.popup,
                            MessageAr = ErrorMessagesAr.arabicNameIsRequired,
                            MessageEn = ErrorMessagesEn.arabicNameIsRequired,
                            titleAr ="خطأ",
                            titleEn = "Error"
                        }
                    };
                if (string.IsNullOrEmpty(request.latinName.Trim()))
                    request.latinName = request.arabicName;
                _MachinesCommand.StartTransaction();

                var machine = new Machines
                {
                    arabicName = request.arabicName.Trim(),
                    latinName = request.latinName.Trim(),
                    branchId = request.branchId,
                    MachineSN = request.MachineSN
                };
                saved = await _MachinesCommand.AddAsync(machine);
                await MachineHelper.pullingData(_InvEmployeesCommand, _InvEmployeesQuery, con,companyInfo,request.MachineSN,machine.Id, _MachineTransactionsCommand);
                _MachinesCommand.CommitTransaction();

            }
            catch (Exception ex)
            {
                _MachinesCommand.Rollback();
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.ErrorSaving,
                        MessageEn = ErrorMessagesEn.ErrorSaving,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    },
                    Note = ex.Message
                };
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart
                {
                    AlartType = AlartType.success,
                    type = AlartShow.note,
                    MessageAr = ErrorMessagesAr.SaveSuccessfully,
                    MessageEn = ErrorMessagesEn.SaveSuccessfully
                }: new Alart
                {
                    AlartType = AlartType.error,
                    type = AlartShow.popup,
                    MessageAr = ErrorMessagesAr.ErrorSaving,
                    MessageEn = ErrorMessagesEn.ErrorSaving
                }
            };
        }
    }
    public class tempMachineMoves
    {
        public int Id { get; set; }
        public int EmployeeCode { get; set; }
        public string machineSN { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime PushTime { get; set; }
        public bool isMoved { get; set; }
    }
}
