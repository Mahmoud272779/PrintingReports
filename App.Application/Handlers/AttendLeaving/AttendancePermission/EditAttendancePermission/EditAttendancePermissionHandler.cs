using App.Application.Handlers.AttendLeaving.Holidays.EditHolidays;
using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendancePermission.EditAttendancePermission
{
    public class EditAttendancePermissionHandler : IRequestHandler<EditAttendancePermissionRequest, ResponseResult>

    {
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.AttendancPermission> _AttendancPermissionQuery;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.AttendancPermission> _AttendancPermissionCommand;
        public EditAttendancePermissionHandler(IRepositoryQuery<AttendancPermission> attendancPermissionQuery, IRepositoryCommand<AttendancPermission> attendancPermissionCommand, IRepositoryQuery<InvEmployees> invEmployeesQuery)
        {
            _AttendancPermissionQuery = attendancPermissionQuery;
            _AttendancPermissionCommand = attendancPermissionCommand;
            _InvEmployeesQuery = invEmployeesQuery;
        }


        public async Task<ResponseResult> Handle(EditAttendancePermissionRequest request, CancellationToken cancellationToken)
        {
            var element = await _AttendancPermissionQuery.GetByIdAsync(request.Id);
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

            var allEmps = _InvEmployeesQuery.TableNoTracking.ToList();
            var checkEmpId = allEmps.Where(e => e.Id == request.EmpId).FirstOrDefault();
            if (checkEmpId == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = "الموظف غير موجود",
                        MessageEn = "This Emp does not exist",
                    }
                };

            if (request.shift1_start > request.shift1_end || request.shift2_start > request.shift2_end ||
               request.shift3_start > request.shift3_end || request.shift4_start > request.shift4_end)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.StartDateAfterEndDate,
                        MessageEn = ErrorMessagesEn.StartDateAfterEndDate,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };

            element.EmpId = request.EmpId;
            element.Day = request.Day;
            element.totalHoursForOpenShift = request.totalHoursForOpenShift;
            element.type = request.type;
            element.IsMoved = request.IsMoved;
            element.shift1_start = request.shift1_start;
            element.shift1_end = request.shift1_end;
            element.shift2_start = request.shift2_start;
            element.shift2_end = request.shift2_end;
            element.shift3_start = request.shift3_start;
            element.shift3_end = request.shift3_end;
            element.shift4_start = request.shift4_start;
            element.shift4_end = request.shift4_end;


            var saved = await _AttendancPermissionCommand.UpdateAsyn(element);
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
