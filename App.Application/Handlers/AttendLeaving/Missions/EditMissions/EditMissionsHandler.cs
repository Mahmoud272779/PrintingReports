using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure;
using MediatR;
using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Missions.AddMissions
{
    public class EditMissionsHandler : IRequestHandler<EditMissionsRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Missions> _missionsCommand;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Missions> _missionsQuery;

        public EditMissionsHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Missions> missionsCommand, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Missions> missionsQuery)
        {
            _missionsCommand = missionsCommand;
            _missionsQuery = missionsQuery;
        }

        public async Task<ResponseResult> Handle(EditMissionsRequest request, CancellationToken cancellationToken)
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
            if(string.IsNullOrEmpty(request.latinName.Trim()))
                request.latinName = request.arabicName.Trim();
            var element = await _missionsQuery.GetByIdAsync(request.Id);
            if(element == null)
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
            element.arabicName = request.arabicName.Trim();
            element.latinName = request.latinName.Trim();
            var saved = await _missionsCommand.UpdateAsyn(element);
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
