using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.SectionsAndDepartments.SectionsAndDepartmentsDropdownList
{
    public class SectionsAndDepartmentsDropdownListRequest : IRequest<ResponseResult>
    {
        public Enums.SectionsAndDepartmentsType Type { get; set; }
        public string parentId { get; set; }
    }
}
