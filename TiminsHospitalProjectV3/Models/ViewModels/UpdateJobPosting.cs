using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class UpdateJobPosting
    {
        public JobPostingDto JobPosting { get; set; }
        public IEnumerable<DepartmentDto> AllDepartments { get; set; }
    }
}