using App.Application.Handlers.AttendLeaving.Holidays.DeleteHolidys;
using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendancePermission.DeleteAttendancePermission
{
    public class DeleteAttendancePermissionHandler : IRequestHandler<DeleteAttendancePermissionRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.AttendancPermission> _AttendancPermissionCommand;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.AttendancPermission> _AttendancPermissionQuery;

        public DeleteAttendancePermissionHandler(IRepositoryQuery<AttendancPermission> attendancPermissionQuery, IRepositoryCommand<AttendancPermission> attendancPermissionCommand)
        {
            _AttendancPermissionQuery = attendancPermissionQuery;
            _AttendancPermissionCommand = attendancPermissionCommand;
        }

        

        public async Task<ResponseResult> Handle(DeleteAttendancePermissionRequest request, CancellationToken cancellationToken)
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

            var elemetsForDelete = _AttendancPermissionQuery.TableNoTracking.Where(c => ids.Contains(c.Id));
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

            _AttendancPermissionCommand.RemoveRange(elemetsForDelete);
            var saved = await _AttendancPermissionCommand.SaveChanges() > 0 ? true : false;
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.DeletedSuccessfully, MessageEn = ErrorMessagesEn.DeletedSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.CanNotDelete, MessageEn = ErrorMessagesEn.CanNotDelete, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
