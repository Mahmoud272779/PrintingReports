using App.Application.Handlers.AttendLeaving.Holidays.DeleteHolidys;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.HolidaysEmployees.DeleteHolidaysEmployees
{
    public class DeleteHolidaysEmployeesHandler : IRequestHandler<DeleteHolidaysEmployeesRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployeesQuery;

        public DeleteHolidaysEmployeesHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployeesQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployeesCommand)
        {
            _HolidaysEmployeesQuery = holidaysEmployeesQuery;
            _HolidaysEmployeesCommand = holidaysEmployeesCommand;
        }

        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployeesCommand;

        public async Task<ResponseResult> Handle(DeleteHolidaysEmployeesRequest request, CancellationToken cancellationToken)
        {
            var ids = request.EmployeesIds.Split(',').Select(c => int.Parse(c)).ToArray();
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
            var elemetsForDelete = _HolidaysEmployeesQuery.TableNoTracking.Include(c => c.Employees).Where(c => ids.Contains(c.Id));
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
            _HolidaysEmployeesCommand.RemoveRange(elemetsForDelete);
            var saved = await _HolidaysEmployeesCommand.SaveChanges() > 0 ? true : false;
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.DeletedSuccessfully, MessageEn = ErrorMessagesEn.DeletedSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.CanNotDelete, MessageEn = ErrorMessagesEn.CanNotDelete, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
