using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ShowFaq
    {
        public bool isadmin { get; set; }
        public FaqDto Faq { get; set; }
        //information that is sent from the user table
        //public User User { get; set; }
    }
}