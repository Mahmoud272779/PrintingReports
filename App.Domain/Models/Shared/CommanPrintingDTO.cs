using App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Shared
{
    public interface CommanPrintingDTO
    {
        public bool isArabic { get; set; }
        public int? fileId { get; set; }
        public exportType exportType { get; set; }
    }
}
