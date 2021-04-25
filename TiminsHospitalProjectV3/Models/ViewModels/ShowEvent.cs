using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ShowEvent
    {
        //The following can be used to show all the information from an event as well as all the donors 
        public EventDto Event { get; set; }

        public IEnumerable<DonationDto> DonationToEvent { get; set; }
    }
}