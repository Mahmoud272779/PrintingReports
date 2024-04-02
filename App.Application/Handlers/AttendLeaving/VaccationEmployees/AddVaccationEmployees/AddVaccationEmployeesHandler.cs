using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.VaccationEmployees.AddVaccationEmployees
{
    public class AddVaccationEmployeesHandler : IRequestHandler<AddVaccationEmployeesRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesCommand;

        public AddVaccationEmployeesHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesCommand)
        {
            _VaccationEmployeesCommand = vaccationEmployeesCommand;
        }

        public async Task<ResponseResult> Handle(AddVaccationEmployeesRequest request, CancellationToken cancellationToken)
        {
            if (request.VaccationId==null || request.EmployeeId==null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "ادخل الكود",
                        MessageEn = "Id is required",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
          

            if (request.DateFrom > request.DateTo)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.StartDateAfterEndDate,
                        MessageEn = ErrorMessagesEn.StartDateAfterEndDate,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };

            var saved = await _VaccationEmployeesCommand.AddAsync(new Domain.Entities.Process.AttendLeaving.VaccationEmployees
            {
                EmployeeId = request.EmployeeId,
                VaccationId = request.VaccationId,
                Note=request.Note,
                DateFrom=request.DateFrom, DateTo=request.DateTo
            });
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
