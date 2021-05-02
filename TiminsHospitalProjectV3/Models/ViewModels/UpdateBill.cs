using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiminsHospitalProjectV3.Models.ViewModels
{
    public class UpdateBill
    {
        public bool isadmin { get; set; }
        public BillDto Bill { get; set; }
    }
}