using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ShowJobPosting
    {
        
        //Render the page depending on if the admin is logged in
        public bool isadmin { get; set; }
        public JobPostingDto JobPosting { get; set; }
        public IEnumerable<DepartmentDto> AllDepartments { get; set; }
       
    }
}