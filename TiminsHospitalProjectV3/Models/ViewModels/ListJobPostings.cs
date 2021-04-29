using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ListJobPostings
    {
        //Conditionally render the page based on admin or non admin users.
        public bool isadmin { get; set; }

        public IEnumerable<JobPostingDto> JobPostings { get; set; }

        public IEnumerable<DepartmentDto> Departments { get; set; }


    }
}