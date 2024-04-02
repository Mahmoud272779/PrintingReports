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
    public class AddMissionsHandler : IRequestHandler<AddMissionsRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Missions> _missionsCommand;

        public AddMissionsHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Missions> missionsCommand)
        {
            _missionsCommand = missionsCommand;
        }

        public async Task<ResponseResult> Handle(AddMissionsRequest request, CancellationToken cancellationToken)
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
            var saved = await _missionsCommand.AddAsync(new Domain.Entities.Process.AttendLeaving.Missions
            {
                arabicName = request.arabicName.Trim(),
                latinName = request.latinName.Trim(),
            });
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
