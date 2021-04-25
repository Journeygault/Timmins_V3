using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class UpdateNewsItem
    {
        //Allows for easy secure updating through the DTO
        public NewsItemDto newsItem { get; set; }
        //information that is sent from the user table
        //public User User { get; set; }
    }
}