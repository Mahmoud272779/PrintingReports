using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure;
using MediatR;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static App.Domain.Enums.Enums;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.AddMasterShift
{
    public class AddMasterShiftHandler : IRequestHandler<AddMasterShiftRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.ShiftsMaster> _ShiftsMasterCommand;

        public AddMasterShiftHandler(IRepositoryCommand<ShiftsMaster> shiftsMasterCommand)
        {
            _ShiftsMasterCommand = shiftsMasterCommand;
        }

        public async Task<ResponseResult> Handle(AddMasterShiftRequest request, CancellationToken cancellationToken)
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


            var saved = await _ShiftsMasterCommand.AddAsync(new ShiftsMaster
            {
                arabicName = request.arabicName.Trim(),
                latinName = request.latinName.Trim(),
                dayEndTime = request.dayEndTime != null ? request.dayEndTime.Value : new DateTime(),
                shiftType = (int)request.shiftType,
            });
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
