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
    public class Event
    {

        [Key]
        public int EventId { get; set; }
        public string Title { get; set; }
        public bool EventHasImage { get; set; }
        public string PicExtension { get; set; }            
        public DateTime EventDate { get; set; }
        public string EventDisc { get; set; }
        public bool EventHasOcured { get; set; }


        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

        public ICollection<Donation> Donations { get; set; }


        //ADD PUBLIC VIRTUAL FOR USER ID
    }
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