using App.Infrastructure;
using FastReport.Gauge.Radial;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Holidays.EditHolidays
{
    public class EditHolidaysHandler : IRequestHandler<EditHolidaysRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Holidays> _HolidaysQuery;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Holidays> _HolidaysCommand;

        public EditHolidaysHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Holidays> holidaysCommand, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Holidays> holidaysQuery)
        {
            _HolidaysCommand = holidaysCommand;
            _HolidaysQuery = holidaysQuery;
        }

        public async Task<ResponseResult> Handle(EditHolidaysRequest request, CancellationToken cancellationToken)
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
            var element = await _HolidaysQuery.GetByIdAsync(request.Id);
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


            element.arabicName = request.arabicName.Trim();
            element.latinName = request.latinName.Trim();
            element.startdate = request.startdate;
            element.enddate = request.enddate;
            var saved = await _HolidaysCommand.UpdateAsyn(element);
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
