using App.Domain.Entities.Process.Store.Barcode;
using App.Infrastructure.Persistence.Context;
using App.Infrastructure.Persistence.Seed;
using App.Infrastructure.settings;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Immutable;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;

namespace App.Application
{
    public class dBCreation : iDBCreation
    {
        private readonly ClientSqlDbContext _clientSqlDbContext;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IErpInitilizerData _iErpInitilizerData;

        public dBCreation(ClientSqlDbContext clientSqlDbContext,
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            IErpInitilizerData iErpInitilizerData
            )
        {
            _clientSqlDbContext = clientSqlDbContext;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _iErpInitilizerData = iErpInitilizerData;
        }
        private List<ReportFiles> ListOfReportFiles()
        {
            var fileList = new List<ReportFiles>();
            var fileNames = GetListOfFile.ReportFilesList();

            int index = 0;
            foreach (var file in fileNames)
            {
                index++;
                //arabic files
                fileList.Add(new ReportFiles
                {
                    IsArabic = true,
                    IsReport = file.isReport ? 1 : 0,
                    ReportFileName = file.reportName,
                    ReportFileNameAr = file.reportNameAr,
                    IsDefault = true,
                    Files = ConvertReportToBytes.ConvertReport(_webHostEnvironment, file.reportName, true),
                    
                });
                //englis files
                fileList.Add(new ReportFiles
                {
                    IsArabic = false,
                    IsReport = file.isReport ? 1 : 0,
                    ReportFileName = file.reportName,
                    ReportFileNameAr = file.reportNameAr,
                    IsDefault = true,
                    Files = ConvertReportToBytes.ConvertReport(_webHostEnvironment, file.reportName, false)
                });
            }
            return fileList;
        }

        private List<BarcodePrintFiles> ListOfBarcodeFiles()
        {
            var fileList = new List<BarcodePrintFiles>();
            var fileNames = GetBarcodeFiles.GetBarcodePrintFiles();


            foreach (var file in fileNames)
            {

                //arabic files
                fileList.Add(new BarcodePrintFiles
                {
                    ArabicName = file.ArabicName,
                    LatineName = file.LatineName,
                    IsDefault = file.IsDefault,
                    File = ConvertReportToBytes.ConvertReport(_webHostEnvironment, file.LatineName, true, true)

                });

            }
            return fileList;
        }

        private List<ReportManger> reportMangers(List<ReportFiles> reportFiles)
        {
            var listOfReportManager = new List<ReportManger>();
            var fileNames = GetListOfFile.ReportFilesList();

            foreach (var item in reportFiles)
            {
                listOfReportManager.Add(new ReportManger
                {
                    IsArabic = item.IsArabic,
                    Copies = 1,
                    ArabicFilenameId = item.Id,
                    screenId = fileNames.Where(x => x.reportName == item.ReportFileName).FirstOrDefault().screenId
                });
            }

            return listOfReportManager;
        }
        public async Task<string> getDatabaseScript(string key)
        {
            var migrationList = _clientSqlDbContext.Database.GetMigrations();
            var script = _clientSqlDbContext.Database.GenerateCreateScript();
            return script;
        }
        public async Task<dbCreationResponse> createClientDB(dbCreationRequest parm)
        {
            try
            {
                if (parm.securityKey != defultData.userManagmentApplicationSecurityKey)
                    return new dbCreationResponse()
                    {
                        isCreated = false,
                        message = "securityKey is nor correct"
                    };
                var connectionString = $"Data Source={_configuration["ApplicationSetting:serverName"]};" +
                                       $"Initial Catalog={parm.dbName};" +
                                       $"user id={_configuration["ApplicationSetting:UID"]};" +
                                       $"password={_configuration["ApplicationSetting:Password"]};" +
                                       $"MultipleActiveResultSets=true;";

                _clientSqlDbContext.Database.SetConnectionString(connectionString);
                await _clientSqlDbContext.Database.MigrateAsync();
                InsertDefultDataForCreation insertDefultDataForCreation = new InsertDefultDataForCreation(_iErpInitilizerData);
                await insertDefultDataForCreation.InsertDefultData(_clientSqlDbContext);


                var administrator = _clientSqlDbContext.userAccount.FirstOrDefault();
                administrator.username = parm.userName;
                administrator.password = parm.password;
                administrator.email = parm.Email;
                _clientSqlDbContext.userAccount.Update(administrator);

                var companyInfo = _clientSqlDbContext.companyData.FirstOrDefault();
                companyInfo.ArabicName = parm.companyName;
                companyInfo.LatinName = parm.companyName;
                companyInfo.FieldAr = "";
                companyInfo.FieldEn = "";
                companyInfo.CommercialRegister = "";
                companyInfo.TaxNumber = !string.IsNullOrEmpty(parm.vatNo) ? parm.vatNo : "";
                companyInfo.Phone1 = !string.IsNullOrEmpty(parm.Phone) ? parm.Phone : "";
                companyInfo.Phone2 = !string.IsNullOrEmpty(parm.Phone) ? parm.Phone : "";
                companyInfo.Fax = !string.IsNullOrEmpty(parm.Phone) ? parm.Phone : "";
                companyInfo.Website = "";
                companyInfo.Email = !string.IsNullOrEmpty(parm.Email) ? parm.Email : "";
                companyInfo.ArabicAddress = "";
                companyInfo.LatinAddress = "";
                companyInfo.Image = @"https://fiels.apex-program.com/Defults/DefultCompanyImage.png";
                _clientSqlDbContext.companyData.Update(companyInfo);

                _clientSqlDbContext.SaveChanges();


                try
                {
                    //Insert Reporting Files
                    var files = ListOfReportFiles();
                    _clientSqlDbContext.reportFiles.AddRange(files);
                    await _clientSqlDbContext.SaveChangesAsync();


                    //set File Manager
                    var FileManager = reportMangers(files);
                    _clientSqlDbContext.reportMangers.AddRange(FileManager);



                    //await _clientSqlDbContext.SaveChangesAsync();

                    var barcodeFiles = ListOfBarcodeFiles();
                    _clientSqlDbContext.BarcodePrintFiles.AddRange(barcodeFiles);

                    await _clientSqlDbContext.SaveChangesAsync();
                }
                catch (Exception)
                {

                }

                return new dbCreationResponse()
                {
                    isCreated = true
                };
            }
            catch (Exception ex)
            {

                return new dbCreationResponse()
                {
                    isCreated = true,
                    message = ex.Message
                };
            }

        }

        public async Task<string> AddReports(string dbName)
        {
            try
            {
                var connectionString = $"Data Source={_configuration["ApplicationSetting:serverName"]};Initial Catalog={dbName};user id={_configuration["ApplicationSetting:UID"]};password={_configuration["ApplicationSetting:Password"]};MultipleActiveResultSets=true;";
                _clientSqlDbContext.Database.SetConnectionString(connectionString);
                //Insert Reporting Files
                var files = ListOfReportFiles();


                var reportfiles = _clientSqlDbContext.reportFiles.Select(x => x.ReportFileName);
                var sss = files.Where(x => !reportfiles.Contains(x.ReportFileName));
                var filesToInsert = files.Where(x => !reportfiles.Contains(x.ReportFileName)).ToList();
                _clientSqlDbContext.reportFiles.AddRange(filesToInsert);
                var fileSaved = await _clientSqlDbContext.SaveChangesAsync();
                filesToInsert = _clientSqlDbContext.reportFiles.ToList();
               //update screen ids 
               await PrepareInsertIdentityInsert<ScreenName>.Prepare(_iErpInitilizerData.setScreenName(), _clientSqlDbContext, "screenNames");



                //set File Manager
                var FileManager = reportMangers(files);
                var _reportMangers = _clientSqlDbContext.reportMangers.Select(x => x.screenId);
                var rebManRows = FileManager.Where(x => !_reportMangers.Contains(x.screenId)).ToList();
                string insertQuery = string.Empty;
                foreach (var item in rebManRows)
                {
                    insertQuery += $"INSERT INTO [dbo].[ReportManger]([screenId],[ArabicFilenameId],[IsArabic],[Copies]) VALUES ({item.screenId},{item.ArabicFilenameId},{(item.IsArabic ? 1 : 0)},1);";
                }
                _clientSqlDbContext.Database.ExecuteSqlRaw(insertQuery);
                //await _clientSqlDbContext.SaveChangesAsync();
                return "Done";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> ReplaceFiles(string dbName)
        {
            try
            {
                var connectionString = $"Data Source={_configuration["ApplicationSetting:serverName"]};Initial Catalog={dbName};user id={_configuration["ApplicationSetting:UID"]};password={_configuration["ApplicationSetting:Password"]};MultipleActiveResultSets=true;";
                _clientSqlDbContext.Database.SetConnectionString(connectionString);
                //Insert Reporting Files
                var files = ListOfReportFiles();

                var ReportFiles = _clientSqlDbContext.reportFiles;
                _clientSqlDbContext.reportFiles.RemoveRange(ReportFiles);
                await _clientSqlDbContext.SaveChangesAsync();

                _clientSqlDbContext.reportFiles.AddRange(files);
                await _clientSqlDbContext.SaveChangesAsync();


                //set File Manager
                var FileManager = reportMangers(files);

                var repManager = _clientSqlDbContext.reportMangers;
                _clientSqlDbContext.reportMangers.RemoveRange(repManager);
                await _clientSqlDbContext.SaveChangesAsync();

                _clientSqlDbContext.reportMangers.AddRange(FileManager);
                await _clientSqlDbContext.SaveChangesAsync();
                return "Done";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
