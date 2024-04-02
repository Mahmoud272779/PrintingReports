using App.Application.SignalRHub;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.General;
using App.Infrastructure.settings;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.PushDataAPI
{
    public class PushDataAPIHandler : IRequestHandler<PushDataAPIRequest, bool>
    {
        private readonly IConfiguration _configuration;
        private readonly IHubContext<NotificationHub> _hub;
        public PushDataAPIHandler(IConfiguration configuration, IHubContext<NotificationHub> hub)
        {
            _configuration = configuration;
            _hub = hub;
        }

        public async Task<bool> Handle(PushDataAPIRequest request, CancellationToken cancellationToken)
        {
            var connectionString = ConnectionString.connectionString(_configuration, request.databaseName);
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            try
            {
                var employee = con.Query<InvEmployees>($"select * from InvEmployees where Code = {request.employeeCode}").FirstOrDefault();
                if(employee == null)
                {
                    var machine = con.Query<Machines>($"select * from Machines where MachineSN = '{request.MahcineSN}'").FirstOrDefault();
                    string arabicName = !string.IsNullOrEmpty(request.name) ? request.name : "موظف جديد";
                    string latinName = !string.IsNullOrEmpty(request.name) ? request.name : "New Employee";
                    var insertEmpQuery =
                        $"INSERT INTO [dbo].[InvEmployees]" +
                        $"([Code]," +
                        $"[ArabicName]," +
                        $"[LatinName]," +
                        $"[Status]," +
                        $"[CanDelete]," +
                        $"[UTime]," +
                        $"[gLBranchId])" +
                        $" VALUES " +
                        $"({request.employeeCode}," +
                        $"'{arabicName}'," +
                        $"'{latinName}'," +
                        $"3," +
                        $"1," +
                        $"GETDATE()," +
                        $"{machine.branchId})";
                    con.Execute(insertEmpQuery);
                }
                employee = con.Query<InvEmployees>($"select * from InvEmployees where Code = {request.employeeCode}").FirstOrDefault();
                var ConnectionIds = con.Query<signalR>("select * from signalR").Where(c=> c.isOnline).Select(c=> c.connectionId).ToArray();
                if (ConnectionIds.Any())
                {
                    _hub.Clients.Clients(ConnectionIds).SendAsync("AttandanceLog", new ResponseResult
                    {
                        Result = Result.AttandanceLog,
                        Data = new AttandanceLogDTO
                        {
                            code = employee.Code,
                            arabicName = employee.ArabicName,
                            latinName = employee.LatinName,
                            TransactionDate = request.transactionDate
                        }
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }



            return true;
        }
    }
    public class AttandanceLogDTO
    {
        public int code { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
