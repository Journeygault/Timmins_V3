using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiminsHospitalProjectV3.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class CreateViewAppointment
    {
        [Required]
        [DisplayName("Patient:")]        
        public string PatientID { get; set; }       

        [Required]
        [DisplayName("Physician:")]        
        public string PhysicianID { get; set; }       

        [Required]
        public string RequestDatetime { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }        
        public IEnumerable<ApplicationUser> UsersInRole { get; set; }
    }

    public class UpdateViewAppointment
    {
        [Required]
        public int ID { get; set; }
        [Required]
        [DisplayName("Patient:")]
        public string PatientID { get; set; }
        public virtual ApplicationUser PatientUser { get; set; }

        [Required]
        [DisplayName("Physician:")]
        public string PhysicianID { get; set; }
        public virtual ApplicationUser PhysicianUser { get; set; }

        [Required]
        public string RequestDatetime { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public AppointmentStatus Status { get; set; }
        [Required]
        public string SentOn { get; set; }
        public string DecisionMadeOn { get; set; }
        public IEnumerable<ApplicationUser> UsersInRole { get; set; }
    }
}