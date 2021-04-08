using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiminsHospitalProjectV3.Models
{
    public enum AppointmentStatus
    {
        Cancelled,
        Accepted,
        Modified,
        Rejected,
        Expired,
        Past,
        Pending
    }
    public class Appointment
    {



        [Key]
        public int ID { get; set; }

        [ForeignKey("PatientUser")]
        public string PatientID { get; set; }
        public virtual ApplicationUser PatientUser { get; set; }

        [ForeignKey("PhysicianUser")]
        public string PhysicianID { get; set; }
        public virtual ApplicationUser PhysicianUser { get; set; }

        [Required]
        public DateTime RequestDatetime { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public AppointmentStatus Status { get; set; }
        [Required]
        public DateTime SentOn { get; set; }
        public DateTime DecisionMadeOn { get; set; }





    }
}