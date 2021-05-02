using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        public string FaqQuestion { get; set; }
        [AllowHtml]//For TinyEMC Usage 
        public string FaqAnswer { get; set; }
        //Foreign Key set on the Category id in the Categories Table
        [ForeignKey("Categories")]
        public int CategoryID { get; set; }
        public virtual Category Categories { get; set; }
    }

    public class FaqDto
    {
        public int FaqID { get; set; }
        [DisplayName("Question")]
        public string FaqQuestion { get; set; }
        [DisplayName("Answer")]
        public string FaqAnswer { get; set; }
        public int CategoryID { get; set; }
    }
}