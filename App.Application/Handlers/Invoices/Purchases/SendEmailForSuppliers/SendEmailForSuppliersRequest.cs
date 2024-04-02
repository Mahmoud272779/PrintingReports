﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.Purchases.SendEmailForSuppliers
{
    public class SendEmailForSuppliersRequest : EmailRequest, IRequest<ResponseResult>
    {
    }
}
