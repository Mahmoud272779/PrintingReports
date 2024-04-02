using App.Application.Handlers.AttendLeaving.Holidays.AddHolidays;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static MailKit.Net.Imap.ImapEvent;

namespace App.Application.Handlers.AttendLeaving.HolidaysEmployees.AddHolidaysEmployees
{
    public class AddHolidaysEmployeesHandler : IRequestHandler<AddHolidaysEmployeesRequest, ResponseResult>
    {
        

        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployeesCommand;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployeesQuery;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Holidays> _HolidaysQuery;
        public AddHolidaysEmployeesHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployeesCommand, IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployeesQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Holidays> holidaysQuery)
        {
            _HolidaysEmployeesCommand = holidaysEmployeesCommand;
            _InvEmployeesQuery = invEmployeesQuery;
            _HolidaysEmployeesQuery = holidaysEmployeesQuery;
            _HolidaysQuery = holidaysQuery;
        }

        public async Task<ResponseResult> Handle(AddHolidaysEmployeesRequest request, CancellationToken cancellationToken)
        {
            int[] empsId = null;
            if (!string.IsNullOrEmpty(request.empIds))
                empsId = request.empIds.Split(',').Select(c => int.Parse(c)).ToArray();
            if (empsId == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = "يجب اختيار موظفين",
                        MessageEn ="You have to choose employees",
                    }
                };
            var checkHolidy = await _HolidaysQuery.GetByIdAsync(request.parentId);
            if (checkHolidy == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = "هذه العطله غير موجوده",
                        MessageEn= "This holiday is not exist",
                    }
                };
            var emps = _InvEmployeesQuery.TableNoTracking.Where(c => empsId.Contains(c.Id));
            List<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> HolidaysEmployeesList = new List<Domain.Entities.Process.AttendLeaving.HolidaysEmployees>();
            foreach (var item in emps)
            {
                HolidaysEmployeesList.Add(new Domain.Entities.Process.AttendLeaving.HolidaysEmployees
                {

                    EmployeesId = item.Id,
                    HolidaysId = request.parentId
                });

            }

            _HolidaysEmployeesCommand.AddRange(HolidaysEmployeesList);



            var saved = await _HolidaysEmployeesCommand.SaveChanges() > 0 ? true : false;
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }

    public class HolidaysEmp 
    {
        public int EmployeesId { get; set; }
        public int HolidaysId { get; set; }
    }
}
