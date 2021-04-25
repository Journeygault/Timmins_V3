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

    public class Donation
    {
        [Key]
        public int DonationID { get; set; }
        //These are columns for users that are not signed in ...
        [Required]
        public string FistName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        //...
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Zip { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }//Validate Date
        [Required]
        public string CardName { get; set; }
        [Required]
        public int CardNumber { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }//Validate Date
        [Required]
        public int CVV { get; set; }
        [Required]
        public string CardType { get; set; }
        public string CompanyName { get; set; }
        //Foreign Key set on the Event id in the Events Table
        [ForeignKey("Events")]
        public int EventId { get; set; }
        public virtual Event Events { get; set; }
    }
    public class DonationDto
    {
        public int DonationID { get; set; }
        //These are columns for users that are not signed in ...
        [DisplayName("First Name")]
        public string FistName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        //...
        public string Address { get; set; }
        public string Country { get; set; }
        [DisplayName("Zip/Psotal Code")]
        public string Zip { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        [DisplayName("Amount CA $")]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }//Validate Date
        [DisplayName("Name On Card")]
        public string CardName { get; set; }
        [DisplayName("Card Number")]
        public int CardNumber { get; set; }
        [DisplayName("Expiry Date")]
        public DateTime ExpiryDate { get; set; }//Validate Date
        public int CVV { get; set; }
        [DisplayName("Card Type")]
        public string CardType { get; set; }
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        public int EventId { get; set; }
    }
}