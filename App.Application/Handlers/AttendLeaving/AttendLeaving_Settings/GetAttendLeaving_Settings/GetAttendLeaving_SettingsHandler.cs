using System.ComponentModel.DataAnnotations;
using System.Threading;
using App.Infrastructure;
using App.Infrastructure.Persistence.Seed;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.AttendLeaving_Settings.GetAttendLeaving_Settings
{
    public class GetAttendLeaving_SettingsHandler : IRequestHandler<GetAttendLeaving_SettingsRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings> _AttendLeaving_SettingsQuery;
        private readonly iUserInformation _iUserInformation;
        private readonly IMediator _mediator;
        private readonly IRepositoryCommand<Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings> _AttendLeaving_SettingsCommand;

        private readonly IErpInitilizerData ErpInitilizerData;

        public GetAttendLeaving_SettingsHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings> attendLeaving_SettingsQuery, iUserInformation iUserInformation, IMediator mediator, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.AttendLeaving_Settings> attendLeaving_SettingsCommand, IErpInitilizerData erpInitilizerData)
        {
            _AttendLeaving_SettingsQuery = attendLeaving_SettingsQuery;
            _iUserInformation = iUserInformation;
            _mediator = mediator;
            _AttendLeaving_SettingsCommand = attendLeaving_SettingsCommand;
            ErpInitilizerData = erpInitilizerData;
        }

        public async Task<ResponseResult> Handle(GetAttendLeaving_SettingsRequest request, CancellationToken cancellationToken)
        {

            var userInfo = await _iUserInformation.GetUserInformation();
            var userBranch = userInfo.CurrentbranchId;
            var response = _AttendLeaving_SettingsQuery.TableNoTracking.FirstOrDefault(c => c.BranchId == userBranch);

            if (response == null)
            {
                //EditAttendLeaving_SettingsRequest nullRequest = null;
                //var data = await _mediator.Send( nullRequest );

                bool saved = await _AttendLeaving_SettingsCommand.AddAsync(ErpInitilizerData.SetAttendLeaving_Settings(userInfo.CurrentbranchId));
                if (saved)
                {
                    response = _AttendLeaving_SettingsQuery.TableNoTracking.FirstOrDefault(c => c.BranchId == userBranch);
                }
                else
                {
                    return new ResponseResult
                    {
                        Result = Result.Failed,
                        Alart = new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }


                    };
                }
            }

            GetAttendLeaving_SettingsDTO data = new GetAttendLeaving_SettingsDTO { 
            
                numberOfShiftsInReports=response.numberOfShiftsInReports,
                TimeOfNeglectingMovementsInMinutes=(int)response.TimeOfNeglectingMovementsInMinutes.TotalMinutes,
                The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift=(int)response.The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift.TotalMinutes,
                The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift = (int)response.The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift.TotalMinutes,
                The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift = (int)response.The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift.TotalMinutes,
                The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift = (int)response.The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift.TotalMinutes,
                The_maximum_delay_in_minutes = (int)response.The_maximum_delay_in_minutes.TotalMinutes,
                The_Maximum_limit_for_early_dismissal_in_minutes= (int)response.The_Maximum_limit_for_early_dismissal_in_minutes.TotalMinutes,
                is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift=response.is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift,
                is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift=response.is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift,
                is_The_maximum_delay_in_minutes=response.is_The_maximum_delay_in_minutes,
                is_The_Maximum_limit_for_early_dismissal_in_minutes=response.is_The_Maximum_limit_for_early_dismissal_in_minutes,
                SetLastMoveAsLeave=response.SetLastMoveAsLeave,
                is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift =response.is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift,
                is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift=response.is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift,
            };

            return new ResponseResult
            {
                Result = Result.Success,
                Data = data,


            };
        }
    }


    public class GetAttendLeaving_SettingsDTO 
    {
        
        public int numberOfShiftsInReports { get; set; }


        public int TimeOfNeglectingMovementsInMinutes { get; set; }

        public bool is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift { get; set; }

        public int The_Minimum_Limit_For_Calculating_Overtime_For_Employees_Before_shift { get; set; }

        public bool is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift { get; set; }
        public int The_Maximum_Limit_For_Calculating_Overtime_For_Employees_Before_shift { get; set; }

        public bool is_The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift { get; set; }//
        public int The_Minimum_Limit_For_Calculating_Overtime_For_Employees_After_shift { get; set; }

        public bool is_The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift { get; set; }//
        public int The_Maximum_Limit_For_Calculating_Overtime_For_Employees_After_shift { get; set; }

        public bool is_The_maximum_delay_in_minutes { get; set; }//
        public int The_maximum_delay_in_minutes { get; set; }

        public bool is_The_Maximum_limit_for_early_dismissal_in_minutes { get; set; }//
        public int The_Maximum_limit_for_early_dismissal_in_minutes { get; set; }

        public bool SetLastMoveAsLeave { get; set; }
    }
}
