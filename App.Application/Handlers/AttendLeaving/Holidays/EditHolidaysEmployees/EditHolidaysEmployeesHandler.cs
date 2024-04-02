using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Holidays.EditHolidaysEmployees
{
    public class EditHolidaysEmployeesHandler : IRequestHandler<EditHolidaysEmployeesRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployeesCommand;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployeesQuery;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Holidays> _HolidaysQuery;
        public EditHolidaysEmployeesHandler(IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployeesCommand, IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployeesQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Holidays> holidaysQuery, IRepositoryQuery<InvEmployees> invEmployeesQuery)
        {
            _HolidaysEmployeesCommand = holidaysEmployeesCommand;
            _HolidaysEmployeesQuery = holidaysEmployeesQuery;
            _HolidaysQuery = holidaysQuery;
            _InvEmployeesQuery = invEmployeesQuery;
        }
        public async Task<ResponseResult> Handle(EditHolidaysEmployeesRequest request, CancellationToken cancellationToken)
        {
            var holiday = _HolidaysQuery.TableNoTracking.Where(c => c.Id == request.HolidayId).FirstOrDefault();
            if(holiday == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "هذة العطله غير موجوده",
                        MessageEn = "This holiday is not exist",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };


            var holidayForDelete = _HolidaysEmployeesQuery.TableNoTracking.Where(c => c.HolidaysId == request.HolidayId);
            if (holidayForDelete != null)
            {
                _HolidaysEmployeesCommand.RemoveRange(holidayForDelete);
                await _HolidaysEmployeesCommand.SaveChanges();

            }

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

            var emps = _InvEmployeesQuery.TableNoTracking.Where(c => ids.Contains(c.Id)).ToList();

            foreach (var emp in emps)
            {
                _HolidaysEmployeesCommand.Add(new Domain.Entities.Process.AttendLeaving.HolidaysEmployees
                {
                    EmployeesId = emp.Id,
                    HolidaysId = holiday.Id
                });
            }

            var saved = await _HolidaysEmployeesCommand.SaveChanges() > 0 ? true : false;
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
