using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class UpdateTicket
    {
        public TicketDTO Ticket { get; set; }
        public IEnumerable<ApplicationUser> UsersInRole { get; set; }
    }
}