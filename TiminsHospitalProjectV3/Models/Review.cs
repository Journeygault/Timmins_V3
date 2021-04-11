using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiminsHospitalProjectV3.Models
{
    public class Review
    {
        [Key]

        public int ReviewID { get; set; }

        public DateTime ReviewDate { get; set; }

        public int ReviewRating { get; set; }

        public string ReviewContent { get; set; }

        //foreign key user ID here
        
    }

    public class ReviewDto
    {
        [Key]
        public int ReviewID { get; set; }

        [DisplayName("Review Date")]
        public DateTime ReviewDate { get; set; }

        [DisplayName("Review Rating")]
        public int ReviewRating { get; set; }

        [DisplayName("Review Content")]
        public string ReviewContent { get; set; }
    }
}