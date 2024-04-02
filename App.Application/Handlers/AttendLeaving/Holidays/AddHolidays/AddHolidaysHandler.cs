using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Holidays.AddHolidays
{
    public class AddHolidaysHandler : IRequestHandler<AddHolidaysRequest, ResponseResult>
    {

        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Holidays> _holidaysCommand;

        public AddHolidaysHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Holidays> holidaysCommand)
        {
            _holidaysCommand = holidaysCommand;
        }
        public async Task<ResponseResult> Handle(AddHolidaysRequest request, CancellationToken cancellationToken)
        {
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
                request.latinName = request.arabicName.Trim();

            if (request.startdate > request.enddate)
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

            var saved = await _holidaysCommand.AddAsync(new Domain.Entities.Process.AttendLeaving.Holidays
            {
                arabicName = request.arabicName.Trim(),
                latinName = request.latinName.Trim(),
                startdate=request.startdate,
                enddate=request.enddate
            });
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart 
                        { 
                    AlartType = AlartType.success, 
                    type = AlartShow.note, 
                    MessageAr = ErrorMessagesAr.SaveSuccessfully, 
                    MessageEn = ErrorMessagesEn.SaveSuccessfully, 
                    titleAr = "save", titleEn = "save" } : 
                    new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };

        }
    }
}
