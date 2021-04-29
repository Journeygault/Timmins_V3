using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models
{
   
    public class JobPosting
    {
        [Key]
        public int JobId { get; set; }

        [Required]
        public string JobCategory { get; set; }

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public string JobDescription { get; set; }

        [Required]
        public string JobLocation { get; set; }

        [Required]
        public string PositionType { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string SalaryRange { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }

        // A job post is associated with one department
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department Department
        { get; set; }


    }

        public class JobPostingDto
        {

            public int JobId { get; set; }

            [DisplayName("Category")]
            public string JobCategory { get; set; }

            [DisplayName("Title")]
            public string JobTitle { get; set; }

            [DisplayName("Description")]
            public string JobDescription { get; set; }

            [DisplayName("Location")]
            public string JobLocation { get; set; }

            [DisplayName("Position Type")]
            public string PositionType { get; set; }

            public string Email { get; set; }

            [DisplayName("Salary Range")]
            public string SalaryRange { get; set; }

            [DisplayName("Date Posted")]
            public DateTime DatePosted { get; set; }

            public int DepartmentID { get; set; }

            [DisplayName("Department Name")]
            public string DepartmentName { get; set; }

    }

}