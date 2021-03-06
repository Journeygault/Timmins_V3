using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

using System;
using System.Linq;
using System.Web;
using System.Configuration;

namespace TiminsHospitalProjectV3.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
   
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /*
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        */

        public ApplicationDbContext()
            : base(AWSConnector.GetRDSConnectionString())
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
 //The following defines the tables in the database

        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Job_Posting> Job_Postings { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillPayment> Bill_Payments { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<Department> Departments { get; set; }
        public System.Data.Entity.DbSet<TiminsHospitalProjectV3.Models.BillDto> BillDtoes { get; set; }

    }

}