using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class ListBill
    {
        //checking if user is admin or not
        public bool isadmin { get; set; }
        public IEnumerable<Bill> bills { get; set; }

    }
}