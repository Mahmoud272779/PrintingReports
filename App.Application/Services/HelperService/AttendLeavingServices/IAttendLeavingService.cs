using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services.HelperService.AttendLeavingServices
{
    public interface IAttendLeavingService
    {
        public Task<IQueryable<InvEmployees>> GetInvEmployeesForCurrentUser(int branchId =0);
    }
}
