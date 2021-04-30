using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ListTicket
    {
        public bool isadmin { get; set; }

        public IEnumerable<TicketDTO> tickets { get; set; }
    }
}