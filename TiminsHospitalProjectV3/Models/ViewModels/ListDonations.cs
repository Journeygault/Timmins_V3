﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ListDonations
    {
        //The View needs to conditionally render the page based on admin or non admin.
        //Admin will see "Create New" and "Edit" links, non-admin will not see these.
        public bool isadmin { get; set; }
        public IEnumerable<DonationDto> donations { get; set; }
    }
}