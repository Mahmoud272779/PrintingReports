using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Projects.EditProject;
using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.RamadanDates.EditRamadanDate
{
    public class EditRamadanDateHandler : IRequestHandler<EditRamadanDateRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.RamadanDate> _RamadanDatesCommand;

        public EditRamadanDateHandler(IRepositoryCommand<RamadanDate> ramadanDatesCommand, IRepositoryQuery<RamadanDate> ramadanDatesQuery)
        {
            _RamadanDatesCommand = ramadanDatesCommand;
            _RamadanDatesQuery = ramadanDatesQuery;
        }

        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.RamadanDate> _RamadanDatesQuery;
        public async Task<ResponseResult> Handle(EditRamadanDateRequest request, CancellationToken cancellationToken)
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
            if (request.startdate > request.enddate)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = ErrorMessagesAr.StartDateAfterEndDate,
                        MessageEn = ErrorMessagesEn.StartDateAfterEndDate,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            if (string.IsNullOrEmpty(request.latinName.Trim()))
                request.latinName = request.arabicName.Trim();
            var element = await _RamadanDatesQuery.GetByIdAsync(request.Id);
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
            element.ArabicName = request.arabicName.Trim();
            element.LatinName = request.latinName.Trim();
            element.FromDate = request.startdate;
            element.ToDate = request.enddate;
            element.Note = request.Note ?? "";

            var saved = await _RamadanDatesCommand.UpdateAsyn(element);
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
