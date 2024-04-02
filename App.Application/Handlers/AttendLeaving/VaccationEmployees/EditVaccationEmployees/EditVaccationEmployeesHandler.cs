using App.Application.Handlers.AttendLeaving.Holidays.EditHolidays;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.VaccationEmployees.EditVaccationEmployees
{
    public class EditVaccationEmployeesHandler : IRequestHandler<EditVaccationEmployeesRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery;

        public EditVaccationEmployeesHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesCommand)
        {
            _VaccationEmployeesQuery = vaccationEmployeesQuery;
            _VaccationEmployeesCommand = vaccationEmployeesCommand;
        }

        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesCommand;

       

        public  async Task<ResponseResult> Handle(EditVaccationEmployeesRequest request, CancellationToken cancellationToken)
        {
            if (request.Id==null)
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
           
            var element = await _VaccationEmployeesQuery.GetByIdAsync(request.Id);
            if (element == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.ThisElementIsNotExist,
                        MessageEn = ErrorMessagesEn.ThisElementIsNotExist,
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

            element.EmployeeId = request.EmployeeId;
            element.VaccationId = request.VaccationId;
            element.DateFrom = request.DateFrom;
            element.DateTo = request.DateTo;
            element.Note = request.Note;
            var saved = await _VaccationEmployeesCommand.UpdateAsyn(element);
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
