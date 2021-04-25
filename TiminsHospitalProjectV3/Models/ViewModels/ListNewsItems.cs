using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    //this viewmodel is used to determine if the user is loged in as an admin
    public class ListNewsItems
    {
        public bool isadmin { get; set; }

        public IEnumerable<NewsItemDto> newsItems { get; set; }
    }
}