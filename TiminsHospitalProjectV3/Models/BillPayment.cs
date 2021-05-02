using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiminsHospitalProjectV3.Models
{
    public class BillPayment
    {
        [Key]
        public int BillPaymentID { get; set; }

        public string CardType { get; set; }

        public int CardNumber { get; set; }

        public string ExpiryDate { get; set; }

        public int CVV { get; set; }

        public DateTime DatePaid { get; set; }

        public int AmountPaid { get; set; }

    }
    public class BillPaymentDto
    {
        [Key]
        public int BillPaymentID { get; set; }

        [DisplayName("Card Type")]
        public string CardType { get; set; }

        [DisplayName("Card Number")]
        public int CardNumber { get; set; }

        [DisplayName("Card Expiry Date")]
        public string ExpiryDate { get; set; }

        [DisplayName("CVV")]
        public int CVV { get; set; }

        [DisplayName("Date Paid")]
        public DateTime DatePaid { get; set; }

        [DisplayName("Amount Paid")]
        public int AmountPaid { get; set; }
    }

}