using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TiminsHospitalProjectV3.Models
{
    public class Class1
    {

        [Key]
        public int PlayerID { get; set; }

        public string PlayerFirstName { get; set; }

        public string PlayerLastName { get; set; }

        public string PlayerBio { get; set; }

        //Foreign keys in Entity Framework
        /// https://www.entityframeworktutorial.net/code-first/foreignkey-dataannotations-attribute-in-code-first.aspx

        //A player plays for one team

    }
}