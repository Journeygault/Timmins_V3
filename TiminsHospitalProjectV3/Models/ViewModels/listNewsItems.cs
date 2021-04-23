using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class listNewsItems
    {
        public bool isadmin { get; set; }

        public IEnumerable<NewsItemDto> events { get; set; }
    }
}