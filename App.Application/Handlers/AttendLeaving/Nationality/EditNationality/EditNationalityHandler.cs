using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Nationality.EditNationality
{
    public class EditNationalityHandler : IRequestHandler<EditNationalityRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Nationality> _NationalityCommand;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Nationality> _NationalityQuery;

        public EditNationalityHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Nationality> nationalityQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Nationality> nationalityCommand)
        {
            _NationalityQuery = nationalityQuery;
            _NationalityCommand = nationalityCommand;
        }

        public async Task<ResponseResult> Handle(EditNationalityRequest request, CancellationToken cancellationToken)
        {
            var element = await _NationalityQuery.GetByIdAsync(request.Id);
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

            element.arabicName = request.ArabicName.Trim();
            element.latinName = request.LatinName.Trim();
            var saved = await _NationalityCommand.UpdateAsyn(element);
            if (saved)
                return new ResponseResult
                {
                    Result = Result.Success,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = ErrorMessagesAr.SaveSuccessfully,
                        MessageEn = ErrorMessagesEn.SaveSuccessfully,
                        titleAr = "خطأ",
                        titleEn = "Error"
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
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
        }
    }
}
