using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ShowJob_Posting
    {
        public Job_PostingDto job_posting { get; set; }


        public IEnumerable<EmployeeDto> job_postingemployees { get; set; }
    }
}