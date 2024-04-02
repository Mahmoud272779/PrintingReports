using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.GettingCompanyData.GettingCompanyDataDTOS
{
    public class GettingDataDTO
    {
        public companyEntity companyEntity { get; set; }
        public List<BranshesEntity> BranshesEntity { get; set; }
        public List<BranchMachinesEntity> BranchMachinesEntity { get; set; }
        public List<jobsEntity> jobsEntity { get; set; }
        public List<ProjectsEntity> ProjectsEntity { get; set; }
        public List<NationalitiesEntity> NationalitiesEntity { get; set; }
        public List<WorkTasksEntity> missionsEntity { get; set; }
        public List<EmployeeGroupsEntity> EmployeeGroupsEntity { get; set; }
        public List<ShiftsEntity> ShiftsEntity { get; set; }
        public List<shiftDetaliesEntity> shiftDetaliesEntity { get; set; }
        public List<EmployeesEntity> EmployeesEntity { get; set; }
        public List<VacationDaysEntity> VacationDaysEntity { get; set; }
        public List<MachineTransactionsEntity> MachineTransactionsEntity { get; set; }
    }
}
