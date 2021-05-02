using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiminsHospitalProjectV3.Models
{
    public class Bill
    {
        [Key]
        public int BillID { get; set; }

        public DateTime DateIssued { get; set; }

        public int Amount { get; set; }

        public string Breakdown { get; set; }

        //foreign key user ID here
    }
    public class BillDto
    {
        [Key]
        public int BillID { get; set; }

        [DisplayName("Bill Date Issued")]
        public DateTime DateIssued { get; set; }

        [DisplayName("Bill Amount")]
        public int Amount { get; set; }

        [DisplayName("Bill Breakdown")]
        public string Breakdown { get; set; }
    }
}