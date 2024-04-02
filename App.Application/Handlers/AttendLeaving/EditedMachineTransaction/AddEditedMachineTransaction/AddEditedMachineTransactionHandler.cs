using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.EditedMachineTransaction.AddEditedMachineTransaction
{
    public class AddEditedMachineTransactionHandler : IRequestHandler<AddEditedMachineTransactionRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<MachineTransactions> _MachineTransactionCommand;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryQuery<Machines> _MachinesQuery;
        private readonly iUserInformation _userInformation;


        public AddEditedMachineTransactionHandler(IRepositoryCommand<MachineTransactions> machineTransactionCommand, IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<Machines> MachinesQuery, iUserInformation userInformation)
        {
            _MachineTransactionCommand = machineTransactionCommand;
            _InvEmployeesQuery = invEmployeesQuery;
            _MachinesQuery = MachinesQuery;
            _userInformation = userInformation;
        }

        public async Task<ResponseResult> Handle(AddEditedMachineTransactionRequest request, CancellationToken cancellationToken)
        {
            var userInfo = await _userInformation.GetUserInformation();
            if (request.password != userInfo.userPassword)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = "عذرًا، يبدو أن كلمة المرور التي أدخلتها غير صحيحة. يُرجى المحاولة مرة أخرى.",
                        MessageEn = "Sorry, it seems the password you entered is incorrect. Please try again.",
                        titleAr = "خطأ في كلمة المرور",
                        titleEn = "Password Error"
                    }
                };

            var empExist = _InvEmployeesQuery.TableNoTracking.Where(c => c.Id == request.empId).FirstOrDefault();
            if (empExist == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = "الموظف غير موجود",
                        MessageEn = "This Emp does not exist",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            var branchExist = _MachinesQuery.TableNoTracking.Where(c => c.Id == request.machineId).FirstOrDefault();
            if (branchExist == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = "المكنة غير موحودة",
                        MessageEn = "This machine does not exist",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            var elem = new MachineTransactions
            {
                isAuto = false,
                IsMoved = false,
                IsEdited = true,
                EmployeeCode = empExist.Code,
                machineId = branchExist.Id,
                PushTime = DateTime.Now,
                EditedTransactionDate = request.day.Date.Add(request.transactionTime)
            };
            var saved = await _MachineTransactionCommand.AddAsync(elem);
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart
                {
                    AlartType = AlartType.success,
                    type = AlartShow.note,
                    MessageAr = ErrorMessagesAr.SaveSuccessfully,
                    MessageEn = ErrorMessagesEn.SaveSuccessfully,
                }:
                new Alart
                {
                    AlartType = AlartType.error,
                    type = AlartShow.popup,
                    MessageAr = ErrorMessagesAr.ErrorSaving,
                    MessageEn = ErrorMessagesEn.ErrorSaving,
                    titleAr = "خطأ",
                    titleEn = "Error"
                }
            };
        }
    }
}
