using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Religion.AddReligion
{
    public class AddReligionRequest : IRequest<ResponseResult>
    {
        [Required]
        public string ArabicName { get; set; }
        public string LatinName { get; set; }
    }
}
