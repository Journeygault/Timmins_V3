using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ShowEvent
    {
        public EventDto Event { get; set; }

        public IEnumerable<DonationDto> DonationToEvent { get; set; }
    }
}