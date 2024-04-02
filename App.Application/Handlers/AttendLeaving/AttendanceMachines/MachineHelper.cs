using App.Application.Handlers.AttendLeaving.AttendanceMachines.AddMachine;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Infrastructure.UserManagementDB;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Domain.Models.Response.General.AdditionalPrices;

namespace App.Application.Handlers.AttendLeaving.AttendanceMachines
{
    public static class MachineHelper
    {
        
        public static async Task<ResponseResult> checkMachineSNIsValid(SqlConnection con, string MachineSN, IRepositoryQuery<Machines> _MachinesQuery, companyInfomation companyInfo)
        {
            //check if the machine exist for another company
            string checkMachineQuery = $"select * from AttendanceLeavingMachines where machineSN ='{MachineSN}' and userApplicationId != '{companyInfo.companyId}'";
            var machines = con.Query<AttendanceLeavingMachines>(checkMachineQuery);
            if (machines.Any())
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.information,
                        type = AlartShow.popup,
                        MessageAr = "هذا الجهاز مُسجل في شركة أخرى. يرجى الاتصال بخدمة العملاء.",
                        MessageEn = "This device has registered for another company. Please call customer support.",
                        titleAr = "تنبيه بشأن تسجيل الجهاز",
                        titleEn = "Device Registration Alert"
                    }
                };
            //check if the device registed before 
            var findDevice = _MachinesQuery.TableNoTracking.Where(c=> c.MachineSN == MachineSN);
            if(findDevice.Any())
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.information,
                        type = AlartShow.popup,
                        MessageAr = "تم تسجيل هذا الجهاز من قبل. إذا كنت تعتقد أن هذا خطأ، يرجى الاتصال بخدمة العملاء.",
                        MessageEn = "This device has already been registered. If you believe this is an error, please contact customer support.",
                        titleAr = "تنبيه: الجهاز مُسجل بالفعل",
                        titleEn = "Alert: Device is Already Registered"
                    }
                };
            return null;
        }
        public static async Task<bool> pullingData(IRepositoryCommand<InvEmployees> _InvEmployeesCommand, IRepositoryQuery<InvEmployees> _IRepositoryQueryCommand, SqlConnection con, companyInfomation companyInfo, string MachineSN ,int machineId, IRepositoryCommand<MachineTransactions> _MachineTransactionsCommand, string oldMachinSN = "", bool isUpdate = false)
        {
            string query = string.Empty;
            if (!isUpdate)
            {
                query = $"insert into AttendanceLeavingMachines (machineSN,userApplicationId) values ('{MachineSN}',{companyInfo.companyId})";
            }
            else
            {
                query = $"update AttendanceLeavingMachines set machineSN = '{MachineSN}' where machineSN = '{oldMachinSN}'";
            }
            con.Execute(query);

            var selectTempMovesQuery = $"select * from tempMachineMoves where machineSN = '{MachineSN}' and isMoved = 0";
            var tempMachineMoves = con.Query<tempMachineMoves>(selectTempMovesQuery);
            if (tempMachineMoves.Any())
            {

                var trans = tempMachineMoves.Select(c => new MachineTransactions
                {
                    EmployeeCode = c.EmployeeCode,
                    IsEdited = false,
                    IsMoved = false,
                    machineId = machineId,
                    PushTime = c.PushTime,
                    TransactionDate = c.TransactionDate
                });
                var emps = _IRepositoryQueryCommand.TableNoTracking;
                var newEmps = trans
                    .Where(c => !emps.Select(x => x.Code).Contains(c.EmployeeCode))
                    .Select(c=> new InvEmployees
                    {
                        Code = c.EmployeeCode,
                        FirstLogmachineId = machineId,
                        ArabicName = "موظف  جديد",
                        LatinName = "New Employee",
                        Status = (int)Status.newElement,
                        CanDelete = false,
                        UTime = DateTime.Now,
                        gLBranchId = 1
                    });

                _InvEmployeesCommand.AddAsync(newEmps);
                var addTransaction = await _MachineTransactionsCommand.AddAsync(trans);
                if (addTransaction)
                {
                    var updateTempTableQuery = $"update tempMachineMoves set isMoved = 1 where Id in ({string.Join(',', tempMachineMoves.Select(c => c.Id))})";
                    con.Execute(updateTempTableQuery);
                }
                return addTransaction;
            }
            return true;
        }
    }
    public class AttendanceLeavingMachines
    {
        public int Id { get; set; }
        public string machineSN { get; set; }
        public int userApplicationId { get; set; }
    }
}
