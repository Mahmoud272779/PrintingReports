using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Projects.AddProject;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Models.Request.AttendLeaving;
using App.Infrastructure;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.RamadanDates.AddRamadanDate
{
    public class AddRamadanDateHandler : IRequestHandler<AddRamadanDateRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.RamadanDate> _RamadanDateCommand;

        public AddRamadanDateHandler(IRepositoryCommand<RamadanDate> ramadanDateCommand)
        {
            _RamadanDateCommand = ramadanDateCommand;
        }

        public async Task<ResponseResult> Handle(AddRamadanDateRequest request, CancellationToken cancellationToken)
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

            var saved = await _RamadanDateCommand.AddAsync(new Domain.Entities.Process.AttendLeaving.RamadanDate
            {
                ArabicName = request.arabicName.Trim(),
                LatinName = request.latinName.Trim(),
                FromDate = request.startdate
                ,
                ToDate = request.enddate,
                Note=request.Note ??"",
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
                    titleAr = "save",
                    titleEn = "save"
                } :
                    new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };

        }
    }
}
