using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiminsHospitalProjectV3.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        public string TicketTitle { get; set; }

        public string TicketBody { get; set; }
        public DateTime TicketDate { get; set; }

        // Retrieving Logged In UserID
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
    public class TicketDTO
    {

        public int TicketId { get; set; }

        public string TicketTitle { get; set; }

        public string TicketBody { get; set; }

        public DateTime TicketDate { get; set; }

        // Retrieving Logged In UserID
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}