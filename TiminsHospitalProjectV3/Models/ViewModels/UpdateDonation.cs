using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class UpdateDonation
    {
        public DonationDto Donation { get; set; }
        //information that is sent from the user table
        public IEnumerable<EventDto> Allevents { get; set; }
    }
}