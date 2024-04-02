using App.Domain.Models.Security.Authentication.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Domain.Enums.Enums;

namespace App.Domain.Models.Request.Store.Sales
{
    public class DebtAgingForCustomersOrSuppliersRequest : GeneralPageSizeParameter
    {
        public DateTime dateTo { get; set; }
        public string branches { get; set; }
        public string? salesmen { get; set; }
        public string persons { get; set; } // customers or suppliers
        public int Department { get; set; } // Sales or Purchases 5/8
        public bool isPrint { get; set; } = false;

    }
}
