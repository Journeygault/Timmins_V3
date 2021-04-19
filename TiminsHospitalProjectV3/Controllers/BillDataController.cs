using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TiminsHospitalProjectV3.Models;

namespace TiminsHospitalProjectV3.Controllers
{
    public class BillDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Prints out a list of all the bills in the database
        /// </summary>
        /// <returns>A list of all the bills  in the database </returns>
        // GET: api/BillData/GetBills
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Bill>))]
        public IHttpActionResult ListBills()
        {
            List<Bill> Bills = db.Bills.ToList();
            List<BillDto> BillDtos = new List<BillDto> {  };

            foreach (var bill in Bills)
            {
                BillDto newBill = new BillDto
                {
                    //Information about a bill goes here
                    BillID = bill.BillID,
                    DateIssued = bill.DateIssued,
                    Amount = bill.Amount,
                    Breakdown = bill.Breakdown,

                };
                BillDtos.Add(newBill);
            }
            return Ok(BillDtos);
        }

    }
}