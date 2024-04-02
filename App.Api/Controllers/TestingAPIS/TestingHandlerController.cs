using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using App.Application.Handlers.Test._1;
using App.Application.Helpers;
using Microsoft.Extensions.Caching.Memory;
using App.Domain.Models.Shared;
using App.Application.Handlers.Setup.ItemCard.Query.FillItemCardQuery;
using App.Application.Helpers.DataBasesMigraiton;
using App.Application.Services.Printing;
using static App.Domain.Enums.BarcodeEnums;
using App.Domain.Enums;
using App.Infrastructure.Persistence.Seed;
using App.Domain.Entities.Process;
using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation;
using App.Application.Handlers.Helper.AttendLeaving;
using DocumentFormat.OpenXml.Wordprocessing;
using App.Application.Handlers.AttendLeaving.GettingCompanyData;

namespace App.Api.Controllers.TestingAPIS
{
    [Route("api/[controller]")]

    public class TestingHandlerController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        private readonly IMediator _mediator;
        private readonly iUpdateMigrationForCompanies iUpdateMigrationForCompanies;
        private readonly IConfiguration _configuration;
        private readonly ClientSqlDbContext _clientSqlDbContext;
        private readonly IErpInitilizerData _erpInitilizerData;
        public TestingHandlerController(IMediator mediator, IMemoryCache memoryCache, iUpdateMigrationForCompanies iUpdateMigrationForCompanies, IConfiguration configuration, ClientSqlDbContext clientSqlDbContext, IErpInitilizerData erpInitilizerData)
        {
            _mediator = mediator;
            _memoryCache = memoryCache;
            this.iUpdateMigrationForCompanies = iUpdateMigrationForCompanies;
            _configuration = configuration;
            _clientSqlDbContext = clientSqlDbContext;
            _erpInitilizerData = erpInitilizerData;
        }
        [HttpGet("SendHello")]
        [AllowAnonymous]
        public async Task<IActionResult> SendHello()
        {
            var response = await _mediator.Send(new SendHello_HandlerRequest()
            {
                Name = "Ahmed"
            });
            return Ok(response);

        }

        [AllowAnonymous]
        [HttpGet("Currency Logo")]
        public async Task<IActionResult> ConvertCurrencyLogo()
        {

            decimal amount = 123.45M;
            string customCurrencySymbol = "$"; // Euro symbol

            string formattedAmount = amount.ToString("C", System.Globalization.CultureInfo.CurrentCulture)
            .Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol, customCurrencySymbol);

            return Ok(formattedAmount);
        }

        [HttpPost("setValueToCash")]
        public async Task<IActionResult> setValueToCash(string key, string value)
        {
            var cash = new MemoryCashHelper(_memoryCache);
            cash.SaveValueIntoCash(key, value);
            return Ok();
        }
        [HttpPost("GetValueToCash")]
        public async Task<IActionResult> GetValueToCash(string key)
        {
            var cash = new MemoryCashHelper(_memoryCache);
            return Ok(cash.GetValue(key));
        }
        [HttpPost("GetAllValueToCash")]
        public async Task<IActionResult> GetAllValueToCash(string key)
        {
            var cash = new MemoryCashHelper(_memoryCache);
            var items = cash.GetSignalRCashedValues();
            return Ok(items);
        }
        [HttpPost("ExTest")]
        [AllowAnonymous]
        public async Task<IActionResult> ExTest()
        {
            var ss = int.Parse("aa");
            return Ok();
        }
        [HttpPost("DecryptString")]
        [AllowAnonymous]
        public async Task<IActionResult> DecryptString([FromBody] string EncryptedString, [FromHeader] string Key)
        {
            if (Key != "newttech123")
                return BadRequest();
            var res = StringEncryption.DecryptString(EncryptedString);
            return Ok(res);
        }
        [HttpPost("CurrencyFormat")]
        [AllowAnonymous]
        public async Task<IActionResult> CurrencyFormat([FromBody] double value)
        {

            var res = value.ToString("C").Replace("$", string.Empty);
            return Ok(res);
        }
        [HttpPost("MovedTransactions")]
        [AllowAnonymous]
        public async Task<IActionResult> MovedTransactions([FromBody] double value)
        {

            var res = value.ToString("C").Replace("$", string.Empty);
            return Ok(res);
        }
        [HttpPost("ExporttoExcel")]
        [AllowAnonymous]
        public FileResult ExporttoExcel([FromBody] string HtmlTable)
        {
            return File(Encoding.ASCII.GetBytes(HtmlTable), "application/vnd.ms-excel", "htmltable.xls");
        }
        [HttpPost("databasesMigration")]
        [AllowAnonymous]
        public async Task<ActionResult> databasesMigration()
        {
            var res = await iUpdateMigrationForCompanies.updateDatabases();
            return Ok(res);
        }
        //[HttpPost("GeneratBarcode")]
        //[AllowAnonymous]
        //public async Task<ActionResult> GeneratBarcode([FromQuery]Enums.Barcodestander_BarcodeType type, [FromQuery] string value)
        //{
        //    var url = QRCode.GenerateBarcode(value, type, _configuration);
        //    return Ok(url);
        //}

        [HttpGet("getMigrations")]
        [Authorize]
        public async Task<IActionResult> getMigrations()
        {
            var migrations = _clientSqlDbContext.Database.GetMigrations();
            var Appliedmigrations = _clientSqlDbContext.Database.GetAppliedMigrations();
            var Pandingmigrations = _clientSqlDbContext.Database.GetPendingMigrations();

            return Ok(new migtations
            {
                migrations = migrations,
                Appliedmigrations = Appliedmigrations,
                Pandingmigrations = Pandingmigrations
            });
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> TestAlart([FromQuery]bool haveAlalrt ,[FromQuery] Enums.AlartShow type, [FromQuery] Enums.AlartType AlartType)
        {
            var res = new ResponseResult();
            if (haveAlalrt)
            {
                var Alart = new Alart
                {
                    titleAr = "تجربة الاكسبشن",
                    titleEn = "Exiption Testing",
                    MessageAr = "نص رسالع تجربه الاكسبشن",
                    MessageEn = "Exeption Handler Message"
                };
                if (type == Enums.AlartShow.note)
                {
                    Alart.type = Enums.AlartShow.note;
                }
                else
                {
                    Alart.type = Enums.AlartShow.popup;
                }
                if (AlartType == Enums.AlartType.warrning)
                {
                    Alart.AlartType = Enums.AlartType.warrning;
                }
                else if (AlartType == Enums.AlartType.information)
                {
                    Alart.AlartType = Enums.AlartType.information;
                }
                else if (AlartType == Enums.AlartType.error)
                {
                    Alart.AlartType = Enums.AlartType.error;
                }
                else if (AlartType == Enums.AlartType.success)
                {
                    Alart.AlartType = Enums.AlartType.success;
                }
                res.Alart = Alart;
            }
            
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpGet("TestInsertingData")]
        public async Task<IActionResult> TestInsertingData()
        {
            ModelBuilder modelBuilder = new ModelBuilder();
            modelBuilder.Entity<InvStpStores>().HasData(_erpInitilizerData.setDefultStores());
            
            return Ok();
        }



        [HttpPost(nameof(MoveTransaction_AttendingLeaving))]
        public async Task<IActionResult> MoveTransaction_AttendingLeaving()
        {
            var res = await _mediator.Send(new TimeCalculationRequest());
            return Ok(res);
        }
        [HttpPost(nameof(AddMachineMovies))]
        public async Task<IActionResult> AddMachineMovies([FromBody]AddMachineMoviesRequest request)
        {
            var res = await _mediator.Send(request);
            return Ok(res);
        }
        
        [HttpPost(nameof(GettingCompanyData))]
        public async Task<IActionResult> GettingCompanyData([FromBody] GettingCompanyDataRequest request)
        {
            var res = await _mediator.Send(request);
            return Ok(res);
        }
        [HttpPost(nameof(testArray))]
        [AllowAnonymous]
        public async Task<IActionResult> testArray([FromQuery]int Id)
        {
            List<int> elementList = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
            int index = (Id - 2 + elementList.Count) % elementList.Count;
            var res =  elementList[index];
            return Ok(res);
        }

    }
    public class GetDaysDTO
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
    public class migtations
    {
        public object migrations { get; set; }
        public object Appliedmigrations { get; set; }
        public object Pandingmigrations { get; set; }
    }


}
