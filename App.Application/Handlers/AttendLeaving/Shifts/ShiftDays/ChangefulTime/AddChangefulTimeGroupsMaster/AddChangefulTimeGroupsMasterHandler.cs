﻿using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure;
using DocumentFormat.OpenXml.Wordprocessing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTimeGroupsMaster
{
    public class AddChangefulTimeGroupsMasterHandler : IRequestHandler<AddChangefulTimeGroupsMasterRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> _ChangefulTimeGroupsMasterCommand;

        public AddChangefulTimeGroupsMasterHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> changefulTimeGroupsMasterCommand)
        {
            _ChangefulTimeGroupsMasterCommand = changefulTimeGroupsMasterCommand;
        }

        public async Task<ResponseResult> Handle(AddChangefulTimeGroupsMasterRequest request, CancellationToken cancellationToken)
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
            var saved = await _ChangefulTimeGroupsMasterCommand.AddAsync(new Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster
            {
                arabicName = request.arabicName.Trim(),
                latinName = request.latinName.Trim(),
                cDate = DateTime.Now,
                Note= request.Note,
                isRamadan = request.isRamadan,
                shiftsMasterId = request.shiftsMasterId,
                startDate = request.startDate
            });
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}