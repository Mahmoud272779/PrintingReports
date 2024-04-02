using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.EditMasterShift
{
    public class EditMasterShiftHandler : IRequestHandler<EditMasterShiftRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<ShiftsMaster> _ShiftsMasterCommand;
        private readonly IRepositoryQuery<ShiftsMaster> _ShiftsMasterQuery;

        public EditMasterShiftHandler(IRepositoryCommand<ShiftsMaster> shiftsMasterCommand, IRepositoryQuery<ShiftsMaster> shiftsMasterQuery)
        {
            _ShiftsMasterCommand = shiftsMasterCommand;
            _ShiftsMasterQuery = shiftsMasterQuery;
        }

        public async Task<ResponseResult> Handle(EditMasterShiftRequest request, CancellationToken cancellationToken)
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
            var element = await _ShiftsMasterQuery.GetByIdAsync(request.Id);
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
            element.arabicName = request.arabicName.Trim();
            element.latinName = request.latinName.Trim();
            element.dayEndTime = request.dayEndTime != null ? request.dayEndTime.Value : new DateTime();
            element.shiftType = (int)request.shiftType;
            var saved = await _ShiftsMasterCommand.UpdateAsyn(element);
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
