﻿using App.Application.Services.Process.Invoices.Purchase.IPurchasesServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers
{
    public class SendEmailForSuppliers_WOVHandler : IRequestHandler<SendEmailForSuppliers_WOVRequest,ResponseResult>
    {
        private readonly ISendEmailPurchases SendEmailPurchases;

        public SendEmailForSuppliers_WOVHandler(ISendEmailPurchases sendEmailPurchases)
        {
            SendEmailPurchases = sendEmailPurchases;
        }

        public async Task<ResponseResult> Handle(SendEmailForSuppliers_WOVRequest request, CancellationToken cancellationToken)
        {
            return await SendEmailPurchases.SendEmailForSuppliers(request);
        }
    }
}
