using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiminsHospitalProjectV3.Models
{
 //The following creates and defines what is in the event table
    public class Event
    {

        [Key]
        public int EventId { get; set; }
        public string Title { get; set; }
//The following two columns are used to determin if there is a picture and what type
        public bool EventHasImage { get; set; }
        public string PicExtension { get; set; }            
        public DateTime EventDate { get; set; }
        public string EventDisc { get; set; }
//Event Has Occured will alow the admin to sort events by weather or not they have occured
        public bool EventHasOcured { get; set; }

//The following is the foring key of user ID and logs the admins ID
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
//The following is a foring key allowing us to show all the doners who donated to an event
        public ICollection<Donation> Donations { get; set; }


    }
//The DTO allows for secure data transfer by creating a degree of seperation between the user and the database
    public class EventDto
    {

        public int EventId { get; set; }
        public string Title { get; set; }
        public bool EventHasImage { get; set; }
        public string PicExtension { get; set; }
        public DateTime EventDate { get; set; }
        public string EventDisc { get; set; }
        public bool EventHasOcured { get; set; }


        // [ForeignKey("UserID")]

        public string UserID { get; set; }
    }
}