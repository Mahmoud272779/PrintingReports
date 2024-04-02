﻿using App.Application.Services.Printing.GenralPrint;
using App.Domain.Models.Response.Store.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services.Printing
{
    public interface IGeneralPrint
    {

        Task<WebReport> PrintReport<T, T1, T2>(
            T Data,
            List<T1> FirstList,
            List<T2> SecondList,
            TablesNames tablesNames,
            object otherData,
            int screenId,
            exportType exportType,
            bool isArabic,
            int fileId = 0,
            int closingStep = 0,
            bool isBarcode = false,
            List<object> obj = null);
        public Task<WebReport> PrintReport(PrintReportDTO printReport);
    }

    public class PrintReportDTO
    {
        public List<GetDatatableDTO> lists { get; set; }
        public (dynamic, string) MainData { get; set; }
        public bool isArabic { get; set; }
        public int fileId { get; set; }
        public int screenId { get; set; }
        public exportType exportType { get; set; }
    }
}
