using App.Application.Helpers;
using App.Domain.Entities.Process.General;
using App.Domain.Enums;
using App.Infrastructure.Interfaces.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services.Process.GeneralServices.SystemHistoryLogsServices
{
    internal class systemHistoryLogsService : ISystemHistoryLogsService
    {
        private readonly IRepositoryCommand<SystemHistoryLogs> _systemHistoryLogsCommand;
        private readonly iUserInformation _iUserInformation;

        public systemHistoryLogsService(IRepositoryCommand<SystemHistoryLogs> SystemHistoryLogsCommand,iUserInformation iUserInformation)
        {
            _systemHistoryLogsCommand = SystemHistoryLogsCommand;
            _iUserInformation = iUserInformation;
        }

        public async Task<bool> SystemHistoryLogsService(SystemActionEnum systemActionEnum)
        {

            var userinfo = await _iUserInformation.GetUserInformation();
            var _systemHistoryLogs = systemHistoryLogs(userinfo.employeeId,userinfo.CurrentbranchId,(int)systemActionEnum, userinfo.isTechincalSupport);
            if(_systemHistoryLogs!=null)//  فى هذه الحاله هتكون مفيش لوج ليها لاى سبب ان كان 
            return await _systemHistoryLogsCommand.AddAsync(_systemHistoryLogs);
            return true;
        }

        public SystemHistoryLogs systemHistoryLogs(int userId,int CurrentbranchId,int systemActionEnum,bool isTechincalSupport)
        {

            var actionsList = SystemActions.systemActionList().Where(x => x.Id == systemActionEnum );
            if (!actionsList.Any())
                return null;
            SystemHistoryLogs systemHistoryLogs = new SystemHistoryLogs()
            {
                ActionArabicName = actionsList.FirstOrDefault().ArabicTransactionType,
                ActionLatinName = actionsList.FirstOrDefault().LatinTransactionType,
                date = DateTime.Now,
                employeesId = userId,
                BranchId = CurrentbranchId,
                TransactionId = systemActionEnum,
                isTechnicalSupport = isTechincalSupport
            };
            return systemHistoryLogs;
        }
        public async Task<bool> SystemHistoryLogsServiceLogin(int emploteeId,int CurrentBranch,bool isTechincalSupport)
        {

            var _systemHistoryLogs = systemHistoryLogs(emploteeId, CurrentBranch, (int)SystemActionEnum.login, isTechincalSupport);
            if (_systemHistoryLogs == null)
                return false;
            _systemHistoryLogsCommand.Add(_systemHistoryLogs);
            return await _systemHistoryLogsCommand.SaveAsync();
        }
    }
}
