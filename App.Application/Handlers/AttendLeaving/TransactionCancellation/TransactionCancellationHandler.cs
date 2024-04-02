using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Infrastructure;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.TransactionCancellation
{
    public class TransactionCancellationHandler : IRequestHandler<TransactionCancellationRequest, ResponseResult>
    {
        private readonly IConfiguration _configuration;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;

        public TransactionCancellationHandler(IConfiguration configuration,IRepositoryQuery<InvEmployees> invEmployeesQuery)
        {
            _configuration = configuration;
            _InvEmployeesQuery = invEmployeesQuery;
        }

        public async Task<ResponseResult> Handle(TransactionCancellationRequest request, CancellationToken cancellationToken)
        {
            SqlConnection con = new SqlConnection(_InvEmployeesQuery.ConnectionString());
            ResponseResult response;
            try
            {
                int[] empIds = null;
                if (!string.IsNullOrEmpty(request.empIds))
                    empIds = request.empIds.Split(',').Select(c=> int.Parse(c)).ToArray();
                var employees = _InvEmployeesQuery
                    .TableNoTracking
                    .Include(c => c.Sections)
                    .Include(c=>c.Departments)
                    .Where(c => empIds != null && request.empIds!="0" ? empIds.Contains(c.Id) : true)
                    .Where(c => request.shiftIds != null ? c.shiftsMasterId == request.shiftIds : true)
                    .Where(c => request.branchId != null ? c.gLBranchId == request.branchId : true)
                    .Where(c => request.sectionId != null ? c.SectionsId == request.sectionId : true)
                    .Where(c => request.departmentId != null? c.DepartmentsId == request.departmentId : true);

                var updatedCodes = string.Join(',', employees.Select(c => c.Code).ToArray());
                var updatedIds = string.Join(',', employees.Select(c => c.Id).ToArray());

                var query = $"update MachineTransactions set IsMoved = 0 where (TransactionDate between '{request.dateFrom.ToString("yyyy-MM-dd")}' And '{request.dateTo.AddDays(1).Date.ToString("yyyy-MM-dd")}' or EditedTransactionDate between '{request.dateFrom.ToString("yyyy-MM-dd")}' And '{request.dateTo.AddDays(1).Date.ToString("yyyy-MM-dd")}' ) and EmployeeCode in ({updatedCodes});";
                //query += $"delete from MoviedTransactions where day between '{request.dateFrom.Date.ToString("yyyy-MM-dd")}' And '{request.dateTo.Date.ToString("yyyy-MM-dd")}' and EmployeesId in ({updatedIds});";
                con.Open();
                con.Execute(query);

            }
            catch (Exception ex)
            {
                return response = new ResponseResult
                {
                    Result = Result.Failed,
                    Note = ex.Message,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.ErrorSaving,
                        MessageEn = ErrorMessagesEn.ErrorSaving,
                        titleAr = "خطا",
                        titleEn = "Error"
                    }
                };
            }
            finally 
            {
                con.Close();  
                con.Dispose();
            }
            return response = new ResponseResult
            {
                Result = Result.Success,
                Alart = new Alart
                {
                    AlartType = AlartType.success,
                    type = AlartShow.note,
                    MessageAr = ErrorMessagesAr.SaveSuccessfully,
                    MessageEn = ErrorMessagesEn.SaveSuccessfully
                }
            };
        }
    }
}
