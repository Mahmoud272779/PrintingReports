using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.Reports.SalesReports.ItemsProfitServices
{
    public class ItemsProfitResponse
    {
        public string ItemCode { get; set; }
        public string itemNameAr { get; set; }
        public string itemNameEn { get; set; }
        public double QTY { get; set; }
        public double Net { get; set; }
        public double Cost { get; set; }
        public double Profit { get; set; }
    }
    public class TotalsItemsProfitResponse
    { 
        public double TotalNet { get; set; }
        public double TotalCost { get; set; }
        public double TotalQTY { get; set; }
        public double TotalProfit { get; set; }

        public List<ItemsProfitResponse> Data = new List<ItemsProfitResponse>();


    }
}
