using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiminsHospitalProjectV3.Models;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class UpdateAppointment
    {
        public Appointment Appointment { get; set; }//appointment to update
        public IEnumerable<ApplicationUser> UsersInRole { get; set; }
    }
}