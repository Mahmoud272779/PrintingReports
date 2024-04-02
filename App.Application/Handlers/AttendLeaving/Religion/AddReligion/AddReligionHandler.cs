using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Nationality.AddNationality;
using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Religion.AddReligion
{
    public class AddReligionHandler : IRequestHandler<AddReligionRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.religions> _religionsCommand;

        public AddReligionHandler(IRepositoryCommand<religions> religionsCommand)
        {
            _religionsCommand = religionsCommand;
        }

        public async Task<ResponseResult> Handle(AddReligionRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ArabicName.Trim()))
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.arabicNameIsRequired,
                        MessageEn = ErrorMessagesEn.arabicNameIsRequired,
                        titleAr = "حفظ",
                        titleEn = "Save"
                    }
                };
            if (string.IsNullOrEmpty(request.LatinName.Trim()))
                request.LatinName = request.ArabicName.Trim();
            var saved = await _religionsCommand.AddAsync(new Domain.Entities.Process.AttendLeaving.religions
            {
                arabicName = request.ArabicName.Trim(),
                latinName = request.LatinName.Trim()
            });
            if (saved)
                return new ResponseResult
                {
                    Result = Result.Success,
                    Alart = new Alart
                    {
                        AlartType = AlartType.success,
                        type = AlartShow.note,
                        MessageAr = ErrorMessagesAr.SaveSuccessfully,
                        MessageEn = ErrorMessagesEn.SaveSuccessfully,
                        titleAr = "حفظ",
                        titleEn = "Save"
                    }
                };
            else
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.ErrorSaving,
                        MessageEn = ErrorMessagesEn.ErrorSaving,
                        titleAr = "حفظ",
                        titleEn = "Save"
                    }
                };
        }
    }
}
