using App.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddNationality
    {
        public string ArabicName { get; set; }
        public string LatinName { get; set; }
    }
    public class EditNationality : AddNationality
    {
        public int Id { get; set; }
    }
    public class DeleteNationality
    {
        public string Ids { get; set; }
    }
    public class GetNationality : PaginationVM
    {
        public string? searchCriteria { get; set; }
    }
}
