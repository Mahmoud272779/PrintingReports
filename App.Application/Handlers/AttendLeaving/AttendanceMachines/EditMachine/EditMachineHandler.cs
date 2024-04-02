using App.Application.Services.HelperService.SecurityIntegrationServices;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Domain.Entities.Process.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using App.Infrastructure;
using System.Data.SqlClient;

namespace App.Application.Handlers.AttendLeaving.AttendanceMachines.EditMachine
{
    public class EditMachineHandler : IRequestHandler<EditMachineRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<Machines> _MachinesCommand;
        private readonly IRepositoryQuery<Machines> _MachinesQuery;
        private readonly IRepositoryCommand<MachineTransactions> _MachineTransactionsCommand;
        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;
        private readonly IRepositoryCommand<InvEmployees> _InvEmployeesCommand;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IConfiguration _configuration;
        private readonly ISecurityIntegrationService _securityIntegration;
        public EditMachineHandler(IRepositoryCommand<Machines> machinesCommand, IRepositoryCommand<MachineTransactions> machineTransactionsCommand, IRepositoryQuery<GLBranch> gLBranchQuery, IConfiguration configuration, ISecurityIntegrationService securityIntegration, IRepositoryQuery<Machines> machinesQuery, IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryCommand<InvEmployees> invEmployeesCommand)
        {
            _MachinesCommand = machinesCommand;
            _MachineTransactionsCommand = machineTransactionsCommand;
            _GLBranchQuery = gLBranchQuery;
            _configuration = configuration;
            _securityIntegration = securityIntegration;
            _MachinesQuery = machinesQuery;
            _InvEmployeesQuery = invEmployeesQuery;
            _InvEmployeesCommand = invEmployeesCommand;
        }

        public async Task<ResponseResult> Handle(EditMachineRequest request, CancellationToken cancellationToken)
        {
            var machine = _MachinesQuery.TableNoTracking.Where(c => c.Id == request.Id).FirstOrDefault();
            if (machine == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.ThisElementIsNotExist,
                        MessageEn = ErrorMessagesEn.ThisElementIsNotExist
                    }
                };
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
            if (checkBranch == null)
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
            bool saved = false;
            var companyInfo = await _securityIntegration.getCompanyInformation();
            SqlConnection con = new SqlConnection(_configuration["ConnectionStrings:UserManagerConnection"]);
            con.Open();
            try
            {
                var oldMachineSN = machine.MachineSN;
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
                            titleAr = "خطأ",
                            titleEn = "Error"
                        }
                    };
                if (string.IsNullOrEmpty(request.latinName.Trim()))
                    request.latinName = request.arabicName;
                _MachinesCommand.StartTransaction();
                machine.arabicName = request.arabicName;
                machine.latinName = request.latinName;
                machine.MachineSN = request.MachineSN;
                machine.branchId = request.branchId;
                saved = await _MachinesCommand.UpdateAsyn(machine);
                await MachineHelper.pullingData(_InvEmployeesCommand,_InvEmployeesQuery, con, companyInfo, request.MachineSN, machine.Id, _MachineTransactionsCommand, oldMachineSN, true);
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
                } : new Alart
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
