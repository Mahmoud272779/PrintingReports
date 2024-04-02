using App.Application.Handlers.AttendLeaving.Missions.DeleteMissions;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Holidays.DeleteHolidys
{
    public class DeleteHolidaysHandler : IRequestHandler<DeleteHolidaysRequest , ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Holidays> _holidaysQuery;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Holidays> _holidaysCommand;

        public DeleteHolidaysHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Holidays> holidaysQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Holidays> holidatsCommand)
        {
            _holidaysQuery = holidaysQuery;
            _holidaysCommand = holidatsCommand;
        }

        public async Task<ResponseResult> Handle(DeleteHolidaysRequest request, CancellationToken cancellationToken)
        {
            var ids = request.Ids.Split(',').Select(c => int.Parse(c)).ToArray();
            if (!ids.Any())
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.DataRequired,
                        MessageEn = ErrorMessagesEn.DataRequired,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            var elemetsForDelete = _holidaysQuery.TableNoTracking.Include(c => c.EmployeesHolidays)
                .Where(c => ids.Contains(c.Id) )
                .Where(c=>!c.EmployeesHolidays.Any());
            if (!elemetsForDelete.Any())
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.CanNotDelete,
                        MessageEn = ErrorMessagesEn.CanNotDelete,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            _holidaysCommand.RemoveRange(elemetsForDelete);
            var saved = await _holidaysCommand.SaveChanges() > 0 ? true : false;
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.DeletedSuccessfully, MessageEn = ErrorMessagesEn.DeletedSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.CanNotDelete, MessageEn = ErrorMessagesEn.CanNotDelete, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
