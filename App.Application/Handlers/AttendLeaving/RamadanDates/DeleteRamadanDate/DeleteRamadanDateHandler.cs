using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Projects.DeleteProjects;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Models.Request.AttendLeaving;
using App.Infrastructure;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.RamadanDates.DeleteRamadanDate
{
    public class DeleteRamadanDateHandler : IRequestHandler<DeleteRamadanDateRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.RamadanDate> _RamadanDatesCommand;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.RamadanDate> _RamadanDatesQuery;
        public DeleteRamadanDateHandler(IRepositoryCommand<RamadanDate> ramadanDatesCommand, IRepositoryQuery<RamadanDate> ramadanDatesQuery)
        {
            _RamadanDatesCommand = ramadanDatesCommand;
            _RamadanDatesQuery = ramadanDatesQuery;
        }

        
        public async Task<ResponseResult> Handle(DeleteRamadanDateRequest request, CancellationToken cancellationToken)
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
            var elemetsForDelete = _RamadanDatesQuery.TableNoTracking.Where(c => ids.Contains(c.id) );
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
            _RamadanDatesCommand.RemoveRange(elemetsForDelete);
            var saved = await _RamadanDatesCommand.SaveChanges() > 0 ? true : false;
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.DeletedSuccessfully, MessageEn = ErrorMessagesEn.DeletedSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.CanNotDelete, MessageEn = ErrorMessagesEn.CanNotDelete, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
