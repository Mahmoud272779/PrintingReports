using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.EditedMachineTransaction.EditEditedMachineTransaction
{
    public class EditEditedMachineTransactionHandler : IRequestHandler<EditEditedMachineTransactionRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<MachineTransactions> _MachineTransactionsQuery;
        private readonly IRepositoryCommand<MachineTransactions> _MachineTransactionsCommand;
        private readonly iUserInformation _userInformation;

        public EditEditedMachineTransactionHandler(IRepositoryQuery<MachineTransactions> machineTransactionsQuery, IRepositoryCommand<MachineTransactions> machineTransactionsCommand, iUserInformation userInformation)
        {
            _MachineTransactionsQuery = machineTransactionsQuery;
            _MachineTransactionsCommand = machineTransactionsCommand;
            _userInformation = userInformation;
        }

        public async Task<ResponseResult> Handle(EditEditedMachineTransactionRequest request, CancellationToken cancellationToken)
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
            var element = _MachineTransactionsQuery.TableNoTracking.Where(c => c.Id == request.Id).FirstOrDefault();
            if (element == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = "الحركة غير موجوده",
                        MessageEn = "This transactions does not exist",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            element.EditedTransactionDate = request.day.Date.Add(request.transactionTime);
            element.IsEdited = true;
            element.IsMoved = false;
            var saved = await _MachineTransactionsCommand.UpdateAsyn(element);
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ?
                            new Alart
                            {
                                AlartType = AlartType.success,
                                type = AlartShow.note,
                                MessageAr = ErrorMessagesAr.SaveSuccessfully,
                                MessageEn = ErrorMessagesEn.SaveSuccessfully
                            }
                            : new Alart
                            {
                                AlartType = AlartType.error,
                                type = AlartShow.popup,
                                MessageAr = ErrorMessagesAr.ErrorSaving,
                                MessageEn = ErrorMessagesEn.ErrorSaving,
                                titleEn = "خطأ",
                                titleAr ="Error"
                            }
                            //ytyt
            };
        }
    }
}
