using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Infrastructure;
using Dapper;
using DocumentFormat.OpenXml.Wordprocessing;
using MediatR;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.EditedMachineTransaction.DeleteEditedMachineTransaction
{
    public class DeleteEditedMachineTransactionHandler : IRequestHandler<DeleteEditedMachineTransactionRequest, ResponseResult>
    {
        public IRepositoryQuery<MachineTransactions> _MachineTransactionsQuery;

        public DeleteEditedMachineTransactionHandler(IRepositoryQuery<MachineTransactions> machineTransactionsQuery)
        {
            _MachineTransactionsQuery = machineTransactionsQuery;
        }

        public async Task<ResponseResult> Handle(DeleteEditedMachineTransactionRequest request, CancellationToken cancellationToken)
        {
            int[] ids = null;
            if (!string.IsNullOrEmpty(request.Ids))
                ids = request.Ids.Split(',').Select(c => int.Parse(c)).ToArray();
            if (!ids.Any())
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = "يجب اختيار حركات لحذفها",
                        MessageEn = "you should select transactions to delete",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            //All data
            var allData = _MachineTransactionsQuery
                            .TableNoTracking
                            .Where(c => ids.Contains(c.Id));
            var autoData = allData.Where(c => c.isAuto).ToList();
            var AddedData = allData.Where(c => !c.isAuto).ToList();

            SqlConnection con = new SqlConnection(_MachineTransactionsQuery.ConnectionString());
            con.Open();
            bool deleted = false;
            try
            {
                var query = "";
                if (autoData.Any())
                {
                    var autoIds = string.Join(',', autoData.Select(c => c.Id));
                    query += $"update {nameof(MachineTransactions)} set IsEdited = 0, IsMoved = 0 where Id in ({autoIds});";
                }

                if (AddedData.Any())
                {
                    var addedIds = string.Join(',', AddedData.Select(c => c.Id));
                    query += $"delete from {nameof(MachineTransactions)} where Id in ({addedIds});";
                }


                deleted = con.Execute(query) > 0 ? true : false;
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
            return new ResponseResult
            {
                Result = deleted ? Result.Success : Result.Failed,
                Alart = deleted ?
                        new Alart
                        {
                            AlartType = AlartType.success,
                            type = AlartShow.note,
                            MessageAr = ErrorMessagesAr.DeletedSuccessfully,
                            MessageEn = ErrorMessagesEn.DeletedSuccessfully,
                        }:
                        new Alart
                        {
                            AlartType = AlartType.error,
                            type = AlartShow.popup,
                            MessageAr = ErrorMessagesAr.CanNotDelete,
                            MessageEn = ErrorMessagesEn.CanNotDelete,
                            titleAr = "خطأ",
                            titleEn = "Error"
                        }
            };


        }
    }
}
