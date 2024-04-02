using App.Domain.Entities.Process.AttendLeaving.Transactions;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.Helper.AttendLeaving
{
    public class AddMachineMoviesHandler : IRequestHandler<AddMachineMoviesRequest, ResponseResult>
    {

        private readonly IRepositoryCommand<MachineTransactions> _MachineTransactionsQuery;

        public AddMachineMoviesHandler(IRepositoryCommand<MachineTransactions> machineTransactionsQuery)
        {
            _MachineTransactionsQuery = machineTransactionsQuery;
        }

        public async Task<ResponseResult> Handle(AddMachineMoviesRequest request, CancellationToken cancellationToken)
        {
            SqlConnection con = new SqlConnection("Data Source=192.168.1.253,63888;Initial Catalog=AttendLeave27092020;user id=sa;password=XB@&)^@4354tfdsg;MultipleActiveResultSets=true;TrustServerCertificate=True;");
            con.Open();
            var selectMovies = con.Query<SelectModel>($"SELECT [Id],[EmployeeID],[TransactionDate],[MachineSN],[IsMoved],[IsEdited],[PushTime]FROM [dbo].[MachineTransactions] where MachineSN = 'OGT7020057012000806'");
            con.Close();
            foreach (var item in selectMovies)
            {
                _MachineTransactionsQuery.Add(new MachineTransactions
                {
                    EmployeeCode = request.CurrentEmployeeCode,
                    IsEdited= false,
                    IsMoved = false,
                    machineId = 1,
                    PushTime = item.PushTime,
                    TransactionDate = item.TransactionDate
                });
            }
            _MachineTransactionsQuery.SaveChanges();
            return new ResponseResult();
        }
    }
    public class SelectModel
    {
        public int Id {get;set;}    
        public int EmployeeID {get;set;}
        public DateTime TransactionDate {get;set;}
        public string MachineSN {get;set;}
        public bool IsMoved {get;set;}
        public bool IsEdited {get;set;}
        public DateTime PushTime { get; set; }
    }
}
