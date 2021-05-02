using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class CreateTicket
    {

        public string TicketTitle { get; set; }

        public string TicketBody { get; set; }
        public DateTime TicketDate { get; set; }

        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}