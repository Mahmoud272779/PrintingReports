using App.Application.Services.HelperService.SecurityIntegrationServices;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Models.Security.Authentication.Response.Store;
using App.Infrastructure;
using App.Infrastructure.Interfaces;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendanceMachines.DeleteMachine
{
    public class DeleteMachineHandler : IRequestHandler<DeleteMachineRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<Machines> _MachinesCommand;
        private readonly IRepositoryQuery<Machines> _MachinesQuery;
        private readonly IConfiguration _configuration;
        private readonly ISecurityIntegrationService _securityIntegrationService;

        public DeleteMachineHandler(IRepositoryCommand<Machines> machinesCommand, IRepositoryQuery<Machines> machinesQuery, ISecurityIntegrationService securityIntegrationService, IConfiguration configuration)
        {
            _MachinesCommand = machinesCommand;
            _MachinesQuery = machinesQuery;
            _securityIntegrationService = securityIntegrationService;
            _configuration = configuration;
        }

        public async Task<ResponseResult> Handle(DeleteMachineRequest request, CancellationToken cancellationToken)
        {
            var ids = request.Ids.Split(',').Select(c => int.Parse(c)).ToArray();
            if (!ids.Any())
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.DataRequired,
                        MessageEn = ErrorMessagesEn.DataRequired
                    }
                };
            var machinesForDelete = _MachinesQuery.TableNoTracking.Where(c => ids.Contains(c.Id));
            if (!machinesForDelete.Any())
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "لا يوجد بيانات للحذف",
                        MessageEn = "No data for delete"
                    }
                };
            var machinesSN = machinesForDelete.Select(c => c.MachineSN).ToArray();
            _MachinesCommand.RemoveRange(machinesForDelete);
            var deleted = await _MachinesCommand.SaveAsync();
            if(deleted)
            {
                SqlConnection con = new SqlConnection(_configuration["ConnectionStrings:UserManagerConnection"]);
                con.Open();
                try
                {
                    var companyInfo = await _securityIntegrationService.getCompanyInformation();
                    string idsInString = "('" + string.Join("', '", machinesSN) + "')";
                    string query = $"delete from [AttendanceLeavingMachines] where [userApplicationId] = {companyInfo.companyId} and [machineSN] in {idsInString}";
                    await con.ExecuteAsync(query);
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
            }
            return new ResponseResult
            {
                Result = deleted ? Result.Success : Result.Failed,
                Alart = deleted ? new Alart
                {
                    AlartType = AlartType.success,
                    type = AlartShow.note,
                    MessageAr = ErrorMessagesAr.SaveSuccessfully,
                    MessageEn = ErrorMessagesEn.SaveSuccessfully
                } : new Alart
                {
                    AlartType = AlartType.error,
                    type = AlartShow.popup,
                    MessageAr = ErrorMessagesAr.ErrorSaving,
                    MessageEn = ErrorMessagesEn.ErrorSaving
                }
            };
        }
    }
}
