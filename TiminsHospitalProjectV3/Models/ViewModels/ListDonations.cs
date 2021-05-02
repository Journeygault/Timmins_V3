using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ListDonations
    {
        public IEnumerable<DonationDto> donations { get; set; }
        public IEnumerable<EventDto> events { get; set; }
    }
}