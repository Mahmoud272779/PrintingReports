using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.EditChangefulTimeGroupsMaster
{
    public class EditChangefulTimeGroupsMasterHandler : IRequestHandler<EditChangefulTimeGroupsMasterRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> _ChangefulTimeGroupsMasterCommand;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> _ChangefulTimeGroupsMasterQuery;

        public EditChangefulTimeGroupsMasterHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> changefulTimeGroupsMasterCommand, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> changefulTimeGroupsMasterQuery)
        {
            _ChangefulTimeGroupsMasterCommand = changefulTimeGroupsMasterCommand;
            _ChangefulTimeGroupsMasterQuery = changefulTimeGroupsMasterQuery;
        }

        public async Task<ResponseResult> Handle(EditChangefulTimeGroupsMasterRequest request, CancellationToken cancellationToken)
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
            var element = await _ChangefulTimeGroupsMasterQuery.GetByIdAsync(request.Id);
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
            element.cDate = DateTime.Now;
            element.Note = request.Note;
            element.isRamadan = request.isRamadan;
            element.shiftsMasterId = request.shiftsMasterId;
            element.startDate = request.startDate;

            var saved = await _ChangefulTimeGroupsMasterCommand.UpdateAsyn(element);
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
