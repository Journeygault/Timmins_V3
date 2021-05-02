using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ShowDepartment
    {
        //Render the page depending on if the admin is logged in
        public bool isadmin { get; set; }
        public DepartmentDto Department { get; set; }
        public IEnumerable<JobPostingDto> JobPostings { get; set; }
    }
}