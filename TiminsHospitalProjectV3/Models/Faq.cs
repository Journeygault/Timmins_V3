using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Added for Data Types
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace TiminsHospitalProjectV3.Models
{
    public class Faq
    {
        [Key]
        public int FaqID { get; set; }
        [DisplayName("Question")]
        public string FaqQuestion { get; set; }
        [DisplayName("Answer")]
        public string FaqAnswer { get; set; }
        //Foreign Key set on the Users id in the Users Table
        //[ForeignKey("Users")]
        //public int UserID { get; set; }
        //public virtual Users Users { get; set; }
    }

    public class FaqDto
    {
        public int FaqID { get; set; }
        [DisplayName("Question")]
        public string FaqQuestion { get; set; }
        [DisplayName("Answer")]
        public string FaqAnswer { get; set; }
        //public int UserID { get; set; }
    }
}