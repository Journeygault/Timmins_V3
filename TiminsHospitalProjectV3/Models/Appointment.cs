using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;


namespace TiminsHospitalProjectV3.Models
{
    public enum AppointmentStatus
    {
       
        Accepted,       
        Rejected,
        Expired,       
        Pending
    }
    public class Appointment
    {



        [Key]
        public int ID { get; set; }

        [DisplayName("Patient")]
        [ForeignKey("PatientUser")]
        public string PatientID { get; set; }
        public virtual ApplicationUser PatientUser { get; set; }

        [DisplayName("Physician")]
        [ForeignKey("PhysicianUser")]
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





    }
}