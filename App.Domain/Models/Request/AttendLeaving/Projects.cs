using App.Domain.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddProjects
    {
        [Required]
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
    public class EditProject : AddProjects
    {
        [Required]
        public int Id { get; set; }
    }
    public class GetPorject : PaginationVM
    {
        public string? searchCriteria { get; set; }

    }
    public class DeleteProjects
    {
        [Required]
        public string Ids { get; set; }
    }
}
