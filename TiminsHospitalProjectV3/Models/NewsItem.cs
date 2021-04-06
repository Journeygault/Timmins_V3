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
    public class NewsItem
    {

        [Key]
        public int NewsItemID { get; set; }

        public string Title { get; set; }

        public string NewsBody { get; set; }
        public DateTime NewItemDate { get; set; }
        public bool NewsItemHasPic { get; set; }

        //If the player has an image, record the extension of the image (.png, .gif, .jpg, etc.)
        public string NewsItemPicExtension { get; set; }

        
        //A player plays for one team
        //[ForeignKey("AccountViewModels")]
        public int UserID { get; set; }
        //public virtual AccountViewModels UserID { get; set; }
    }
    public class NewsItemDto
    {
        public int NewsItemID { get; set; }

        public string Title { get; set; }

        public string NewsBody { get; set; }
        public DateTime NewItemDate { get; set; }
        public bool NewsItemHasPic { get; set; }

        //If the player has an image, record the extension of the image (.png, .gif, .jpg, etc.)
        public string NewsItemPicExtension { get; set; }


        //A player plays for one team
        //[ForeignKey("AccountViewModels")]
        public int UserID { get; set; }
    }
}