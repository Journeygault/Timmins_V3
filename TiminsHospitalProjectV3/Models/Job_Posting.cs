using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiminsHospitalProjectV3.Models
{
    public class Job_Posting
    {
        [Key]
        public int JobID { get; set; }
        public string JobName { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
    public class Job_PostingDto
    {
        public int JobID { get; set; }
        public string JobName { get; set; }
    }
}