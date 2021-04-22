using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ListEmployees
    {
        public bool isadmin { get; set; }

        public IEnumerable<EmployeeDto> employees { get; set; }
    }
}