using App.Domain.Entities.Process.AttendLeaving.Shift;
using App.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddChangefulTimeGroupsMasterDTO
    {
        [Required]
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public string Note { get; set; }
        public bool isRamadan { get; set; }
        public int shiftsMasterId { get; set; }
        [Required]
        public DateTime startDate { get; set; }
    }
    public class EditChangefulTimeGroupsMasterDTO : AddChangefulTimeGroupsMasterDTO
    {
        [Required]
        public int Id { get; set; }
    }
    public class DeleteChangefulTimeGroupsMasterDTO
    {
        [Required]
        public string Ids { get; set; }
    }
    public class GetChangefulTimeGroupsMasterDTO : PaginationVM
    {
        public string? SearchCriteria { get; set; }
        [Required]
        public int Id { get; set; }
    }
    public class EditChangefulTimeDays
    {
        [JsonIgnore]
        public DateTime? startDate { get; set; }
        [JsonIgnore]
        public DateTime? endDate { get; set; }
        public int changefulTimeGroupsId { get; set; }
        public bool IsRamadan { get; set; }
        public List<EditChangefulTimeDays_Shifts> shifts { get; set; }
    }
    public class EditChangefulTimeDays_Shifts : TimesComman
    {
        public int workDaysNumber { get; set; }
        public int weekendNumber { get; set; }
    }
    public class GetChangefulTimeDays : PaginationVM
    {
        public string? SearchCriteria { get; set; }
        [Required]
        public int changefulTimeGroupsId { get; set; }
        public bool IsRamadan { get; set; }
    }
}
