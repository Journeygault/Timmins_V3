using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiminsHospitalProjectV3.Models
{//Table name
    public class NewsItem
    {
//Primary key of table
        [Key]
        public int NewsItemID { get; set; }

        public string Title { get; set; }

        public string NewsBody { get; set; }
        public DateTime NewItemDate { get; set; }
//The following two fields allow for detection of a picture and that type of picture(IE JPG)
        public bool NewsItemHasPic { get; set; }

        public string NewsItemPicExtension { get; set; }

//The ForegnKey is of the loged in user ID 
        //[ForeignKey("AccountViewModels")]
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
 //The DTO helps keep data safe by adding a degree of seperation to the database
    public class NewsItemDto
    {
        public int NewsItemID { get; set; }

        public string Title { get; set; }

        public string NewsBody { get; set; }
        public DateTime NewItemDate { get; set; }
        public bool NewsItemHasPic { get; set; }

        public string NewsItemPicExtension { get; set; }


        public string UserID { get; set; }
    }
}