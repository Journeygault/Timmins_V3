using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class UpdateEmployee
    {
        public EmployeeDto employee { get; set; }
        public IEnumerable<Job_PostingDto> alljob_postings { get; set; }
    }
}