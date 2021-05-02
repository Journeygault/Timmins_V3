using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{//The following allows easy and safe acces to the newsitem info
    public class ShowNewsItem
    {
        public NewsItemDto newsItem { get; set; }
    }
}