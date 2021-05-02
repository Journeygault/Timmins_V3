using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ListReviews
    {
        //rendering the page based on admin or user
        public bool isadmin { get; set; }

        public IEnumerable<ReviewDto> reviews { get; set; }
    }
}