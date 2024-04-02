﻿using App.Application.Handlers.AttendLeaving.Missions.AddMissions;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Vaccation.EditVaccation
{
    public class EditVaccationHandler : IRequestHandler<EditVaccationRequest, ResponseResult>
    {

        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Vaccation> _vaccationCommand;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Vaccation> _vaccationQuery;

        public EditVaccationHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Vaccation> missionsCommand, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Vaccation> missionsQuery)
        {
            _vaccationCommand = missionsCommand;
            _vaccationQuery = missionsQuery;
        }

        public async Task<ResponseResult> Handle(EditVaccationRequest request, CancellationToken cancellationToken)
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
            var element = await _vaccationQuery.GetByIdAsync(request.Id);
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
            var saved = await _vaccationCommand.UpdateAsyn(element);
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
