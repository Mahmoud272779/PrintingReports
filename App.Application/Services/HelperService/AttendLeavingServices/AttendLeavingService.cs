using App.Domain.Entities.Process.AttendLeaving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services.HelperService.AttendLeavingServices
{
    public class AttendLeavingService : IAttendLeavingService
    {
        private readonly iUserInformation _iUserInformation;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;
        private readonly IRepositoryQuery<SectionsAndDepartments> _SectionsAndDepartmentsQuery;

        public AttendLeavingService(iUserInformation iUserInformation, IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<GLBranch> gLBranchQuery, IRepositoryQuery<SectionsAndDepartments> sectionsAndDepartmentsQuery)
        {
            _iUserInformation = iUserInformation;
            _InvEmployeesQuery = invEmployeesQuery;
            _GLBranchQuery = gLBranchQuery;
            _SectionsAndDepartmentsQuery = sectionsAndDepartmentsQuery;
        }

        public async Task<IQueryable<InvEmployees>> GetInvEmployeesForCurrentUser(int branchId = 0)
        {
            var userInfo = await _iUserInformation.GetUserInformation();
            var invEmployee = _InvEmployeesQuery
                .TableNoTracking
                .Where(c => branchId != 0 ? c.gLBranchId == branchId : true)
                .Where(c=> c.Status != (int)Enums.Status.newElement && c.shiftsMasterId != null);
            if (userInfo.otherSettings.showAllEmployees)
                return invEmployee;
            
            var branchesIds = _GLBranchQuery.TableNoTracking.Where(c => c.ManagerId == userInfo.employeeId).Select(c=> c.Id).ToList();
            var sectionsAndDepartments = _SectionsAndDepartmentsQuery.TableNoTracking.Where(c => c.empId == userInfo.CurrentbranchId).Select(c=> c.Id).ToList();
            IQueryable<InvEmployees> invEmployees = 
                invEmployee.Where(c=> branchesIds.Contains(c.gLBranchId) || 
                sectionsAndDepartments.Contains(c.SectionsId.Value) ||
                sectionsAndDepartments.Contains(c.DepartmentsId.Value) ||
                c.Id == userInfo.employeeId || 
                c.ManagerId == userInfo.employeeId
                );
            return invEmployees;

        }
    }
}
