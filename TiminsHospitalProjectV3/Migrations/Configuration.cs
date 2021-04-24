namespace TiminsHospitalProjectV3.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TiminsHospitalProjectV3.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            // this.ContextKey = "TiminsHospitalProjectV3.Migrations.Configuration";
            Seed(new Models.ApplicationDbContext());

        }

        protected override void Seed(TiminsHospitalProjectV3.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            IdentityRole PatientRole = new IdentityRole();
            PatientRole.Name = "Patient";
            IdentityRole PhysicianRole = new IdentityRole();
            PhysicianRole.Name = "Physician";
            IdentityRole AdminRole = new IdentityRole();
            AdminRole.Name = "Admin";
            //add your role here

            IdentityRole[] roles = { PatientRole, PhysicianRole, AdminRole };
            foreach (var role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role.Name))
                {
                    context.Roles.Add(role);
                }

                context.SaveChanges();
            }
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
