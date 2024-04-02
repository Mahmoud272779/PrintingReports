using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Nationality.AddNationality
{
    public class AddNationalityHandler : IRequestHandler<AddNationalityRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Nationality> _NationalityQuery;

        public AddNationalityHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Nationality> nationalityQuery)
        {
            _NationalityQuery = nationalityQuery;
        }
        public async Task<ResponseResult> Handle(AddNationalityRequest request, CancellationToken cancellationToken)
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
            var saved = await _NationalityQuery.AddAsync(new Domain.Entities.Process.AttendLeaving.Nationality
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
