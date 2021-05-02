using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ShowDonation
    {
        public bool isadmin { get; set; }
        public DonationDto Donation { get; set; }
        //information that is sent from the user table
        public EventDto events { get; set; }
    }
}