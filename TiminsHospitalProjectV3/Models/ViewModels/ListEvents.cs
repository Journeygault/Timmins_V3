
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ListEvents
    {//this viewmodel is used to determine if the user is loged in as an admin
        public bool isadmin { get; set; }

        public IEnumerable<EventDto> events { get; set; }

    }
}