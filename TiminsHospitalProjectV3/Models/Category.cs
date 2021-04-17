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
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        //Foreign Key set on the Category id in the Faqs Table
        public ICollection<Faq> Faqs { get; set; }
    }
    public class CategoryDto
    {
        public int CategoryID { get; set; }
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }
    }
}