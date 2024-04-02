using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Infrastructure;
using App.Infrastructure.Persistence.Seed;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.AttendLeaving_Settings.EditAttendLeaving_Settings
{
    public class EditAttendLeaving_SettingsHandlerIRequestHandler : IRequestHandler<EditAttendLeaving_SettingsRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings> _AttendLeaving_SettingsCommand;
        private readonly IRepositoryQuery<Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings> _AttendLeaving_SettingsQuery;
        private readonly IErpInitilizerData ErpInitilizerData;

        private readonly iUserInformation _UserInformation;
        public EditAttendLeaving_SettingsHandlerIRequestHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings> attendLeaving_SettingsCommand, iUserInformation iUserInformation, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings> attendLeaving_SettingsQuery, IErpInitilizerData erpInitilizerData)
        {
            _AttendLeaving_SettingsCommand = attendLeaving_SettingsCommand;
            _UserInformation = iUserInformation;
            _AttendLeaving_SettingsQuery = attendLeaving_SettingsQuery;
            ErpInitilizerData = erpInitilizerData;
        }

        public async Task<ResponseResult> Handle(EditAttendLeaving_SettingsRequest request, CancellationToken cancellationToken)
        {

            var userInfo = await _UserInformation.GetUserInformation();

            var userBranch = userInfo.CurrentbranchId;
            var element = _AttendLeaving_SettingsQuery.TableNoTracking.FirstOrDefault(c => c.BranchId == userBranch);

            bool saved = false;
            if (element == null)
            {
                saved = await _AttendLeaving_SettingsCommand.AddAsync(ErpInitilizerData.SetAttendLeaving_Settings(userInfo.CurrentbranchId));

            }

            else
            {
                element.numberOfShiftsInReports = request.numberOfShiftsInReports;
                element.is_The_maximum_delay_in_minutes = request.is_The_maximum_delay_in_minutes;
                element.The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift = TimeSpan.FromMinutes(request.The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift);
                element.The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift = TimeSpan.FromMinutes(request.The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift);
                element.The_Maximum_limit_for_early_dismissal_in_minutes = TimeSpan.FromMinutes(request.The_Maximum_limit_for_early_dismissal_in_minutes);
                element.is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift = request.is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift;
                element.is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift = request.is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift;
                element.is_The_Maximum_limit_for_early_dismissal_in_minutes = request.is_The_Maximum_limit_for_early_dismissal_in_minutes;
                element.is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift = request.is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift;
                element.is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift = request.is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift;
                //element.is_TimeOfNeglectingMovementsInMinutes = request.is_TimeOfNeglectingMovementsInMinutes;
                element.SetLastMoveAsLeave = request.SetLastMoveAsLeave;
                element.TimeOfNeglectingMovementsInMinutes = TimeSpan.FromMinutes(request.TimeOfNeglectingMovementsInMinutes);
                element.The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift = TimeSpan.FromMinutes(request.The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift);
                element.The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift = TimeSpan.FromMinutes(request.The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift);
                element.The_maximum_delay_in_minutes = TimeSpan.FromMinutes(request.The_maximum_delay_in_minutes);
                
                saved = await _AttendLeaving_SettingsCommand.UpdateAsyn(element);
            }



            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" }
                : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };

        }
    }
}
