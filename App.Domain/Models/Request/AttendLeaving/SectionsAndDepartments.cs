using App.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddSectionsAndDepartmentsDTO
    {
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public int? empId { get; set; }
        public int parentId { get; set; }
    }
    public class EditSectionsAndDepartmentsDTO : AddSectionsAndDepartmentsDTO
    {
        public int Id { get; set; }
    }
    public class DeleteSectionsAndDepartmentsDTO
    {
        public string Id { get; set; }
    }
    public class GetSectionsAndDepartmentsDTO : PaginationVM
    {
        public string? searchCriteria { get; set; }
        [Required]
        public int ParentId { get; set; }

    }
}
