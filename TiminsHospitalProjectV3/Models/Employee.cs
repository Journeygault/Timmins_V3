using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiminsHospitalProjectV3.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        public string EmployeeFirstname { get; set; }
        public string EmployeeLastname { get; set; }
        public string EmployeePhone { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeAddress { get; set; }
        public string EmployeeDes { get; set; }
        public bool EmployeeHasPic { get; set; }
        public string EmployeeResume { get; set; }
        //an applicant have one job title
        //job posting table name
        [ForeignKey("Job_Posting")]
        public int JobID { get; set; }
        public virtual Job_Posting Job_Posting { get; set; }
        //an applicant have one user signin
        //an applicant have one user signin
        //[ForeignKey("ApplicationUser")]
        ////user signin table name
        //public string Id { get; set; }
        //public virtual ApplicationUser ApplicationUser { get; set; }
    }
    public class EmployeeDto
    {
        public int EmployeeID { get; set; }
        [DisplayName("First Name")]
        public string EmployeeFirstname { get; set; }
        [DisplayName("Last Name")]
        public string EmployeeLastname { get; set; }
        [DisplayName("Phone")]
        public string EmployeePhone { get; set; }
        [DisplayName("Email")]
        public string EmployeeEmail { get; set; }
        [DisplayName("Address")]
        public string EmployeeAddress { get; set; }
        [DisplayName("Description")]
        public bool EmployeeHasPic { get; set; }
        public string EmployeeDes { get; set; }
        [DisplayName("Resume")]
        public string EmployeeResume { get; set; }
        public int JobID { get; set; }


    }
}