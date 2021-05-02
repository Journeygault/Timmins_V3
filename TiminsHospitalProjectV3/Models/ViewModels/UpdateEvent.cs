using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class UpdateEvent
    {
        //Allows for easy updating of the events throlugh the DTO
        
        public EventDto Event { get; set; }
        //information that is sent from the user table
        //public User User { get; set; }
    }
}