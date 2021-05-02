using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ListNewsItems
    {
        public bool isadmin { get; set; }

        public IEnumerable<NewsItemDto> newsItems { get; set; }
    }
}