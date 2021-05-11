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
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Find a bill given the ID of the bill
        /// </summary>
        /// <param name="id">2</param>
        /// <returns>Returns the bill with an ID of 2.</returns>
        [HttpGet]
        [ResponseType(typeof(BillDto))]
        public IHttpActionResult FindBill (int id)
        {
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return NotFound();
            }
            //puts data into an object format
            BillDto newBill = new BillDto
            {
                BillID = bill.BillID,
                DateIssued = bill.DateIssued,
                Amount = bill.Amount,
                Breakdown = bill.Breakdown,
            };
            return Ok(newBill);
        }
        /// <summary>
        /// Gives admins the ability to create a bill
        /// </summary>
        /// <param name="bill"></param>
        /// <returns>A created bill associated with the user</returns>
        [HttpPost]
        [ResponseType(typeof(Bill))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult CreateBill([FromBody] Bill bill)
        {
            db.Bills.Add(bill);
            db.SaveChanges();
            return Ok(bill.BillID);
            //add foreign key user id to connect creating a bill to a specific user - update model to include foreign key ID from table `users` when user table is created.
        }

        [HttpPost]
        [ResponseType(typeof(Bill))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteBill (int id)
        {
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return NotFound();
            }
            db.Bills.Remove(bill);
            db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateBill(int id, [FromBody] Bill bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != bill.BillID)
            {
                return BadRequest();
            }
            db.Entry(bill).State = EntityState.Modified;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        

    }
}