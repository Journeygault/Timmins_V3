using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ShowEmployee
    {
        public EmployeeDto employee { get; set; }
        public Job_PostingDto job_posting { get; set; }
    }
}