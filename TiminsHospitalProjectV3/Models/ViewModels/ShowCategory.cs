using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ShowCategory
    {
        public bool isadmin { get; set; }

        //Information about the team
        public CategoryDto Category { get; set; }

        //Information about all Projects on that Categories
        public IEnumerable<FaqDto> CategoryFaqs { get; set; }
    }
}