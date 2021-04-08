using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class UpdateNewsItem
    {
        public NewsItemDto newsItem { get; set; }
        //Need The following to update wither userID's POTENTIALY
        //public IEnumerable<HopClassificationDto> allhopclassifications { get; set; }


    }
}