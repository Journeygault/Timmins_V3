using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        //A department can have many job posts
        public ICollection<JobPosting> JobPostings { get; set; }
    }

    public class DepartmentDto
    {
        public int DepartmentId { get; set; }

        [DisplayName("Department Name")]
        public string DepartmentName { get; set; }
    }

}