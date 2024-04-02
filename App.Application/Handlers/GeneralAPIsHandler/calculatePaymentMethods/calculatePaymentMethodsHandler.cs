using App.Domain.Models.Response.Store.Invoices;
using App.Domain.Models.Security.Authentication.Response.Store.Invoices;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.InvoicesHelper.calculatePaymentMethods
{
    public class calculatePaymentMethodsHandler : IRequestHandler<calculatePaymentMethodsRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<InvGeneralSettings> GeneralSettings;

        public calculatePaymentMethodsHandler(IRepositoryQuery<InvGeneralSettings> generalSettings)
        {
            GeneralSettings = generalSettings;
        }

        public async Task<ResponseResult> Handle(calculatePaymentMethodsRequest request, CancellationToken cancellationToken)
        {
            var Settings = await GeneralSettings.SingleOrDefault(a => a.Id == 1);

            var resultData = new paymentMethodsResponse();
            resultData.paid = Math.Round(request.values.Sum(), Settings.Other_Decimals);  //roundNumbers.GetDefultRoundNumber(request.values.Sum());//

            if (resultData.paid > request.net)
            {
                resultData.remain = 0;
                return new ResponseResult { Data = resultData, Result = Result.PaidOvershootNet };
            }

            resultData.remain = Math.Round(request.net - resultData.paid, Settings.Other_Decimals);// roundNumbers.GetDefultRoundNumber(request.net - resultData.paid);  //
            if (resultData.remain < 0)
                resultData.remain *= -1;

            return new ResponseResult { Data = resultData, Result = Result.Success };
        }
    }
}
