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
    public class DonationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        ///     Lists out all the Donations in the database -- SearchKey is used to find an Donation that matches the input
        /// </summary>
        /// <returns>A list of all Donations in the database -or- A list of all Donations in the database that match</returns>(checked)
        //Trying to use the search key to find individual Donation's
        // GET: api/DonationData/GetDonations
        [HttpGet]
        [Route("api/DonationData/GetDonations")]
        [ResponseType(typeof(IEnumerable<DonationDto>))]
        public IHttpActionResult GetDonations()
        {
            List<Donation> donations = db.Donations.ToList();
            List<DonationDto> donationDtos = new List<DonationDto> { };

            foreach (var donation in donations)
            {
                DonationDto NewDonation = new DonationDto
                {
                    DonationID = donation.DonationID,
                    FistName = donation.FistName,
                    LastName = donation.LastName,
                    Email = donation.Email,
                    PhoneNumber = donation.PhoneNumber,
                    Address = donation.Address,
                    Country = donation.Country,
                    Zip = donation.Zip,
                    Province = donation.Province,
                    City = donation.City,
                    Amount = donation.Amount,
                    Date = donation.Date,
                    CardName = donation.CardName,
                    CardNumber = donation.CardNumber,
                    ExpiryDate = donation.ExpiryDate,
                    CVV = donation.CVV,
                    CardType = donation.CardType,
                    CompanyName = donation.CompanyName,
                    EventId =donation.EventId

                };
                donationDtos.Add(NewDonation);
            }
            return Ok(donationDtos);
        }
        /// <summary>
        ///     Finding an Donation by it's id
        /// </summary>
        /// <param name="id">DonationID</param>
        /// <returns>All the information of the Donation</returns>(checked)
        // GET: api/DonationData/FindDonation/1
        [HttpGet]
        [Route("api/DonationData/FindDonation/{id}")]
        [ResponseType(typeof(DonationDto))]
        public IHttpActionResult FindDonation(int id)
        {
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return NotFound();
            }
            DonationDto DonationDto = new DonationDto
            {
                DonationID = donation.DonationID,
                FistName = donation.FistName,
                LastName = donation.LastName,
                Email = donation.Email,
                PhoneNumber = donation.PhoneNumber,
                Address = donation.Address,
                Country = donation.Country,
                Zip = donation.Zip,
                Province = donation.Province,
                City = donation.City,
                Amount = donation.Amount,
                Date = donation.Date,
                CardName = donation.CardName,
                CardNumber = donation.CardNumber,
                ExpiryDate = donation.ExpiryDate,
                CVV = donation.CVV,
                CardType = donation.CardType,
                CompanyName = donation.CompanyName,
                EventId = donation.EventId
            };
            return Ok(DonationDto);
        }
        /// <summary>
        ///     This Finds the Event for an Donation by the Project Id.
        /// </summary>
        /// <param name="id">Donation Id</param>
        /// <returns>All the information of the Event</returns> (CHECKED)
        // GET: api/EventData/FindEventForDonation/1
        [HttpGet]
        [Route("api/EventData/FindEventForDonation/{id}")]
        [ResponseType(typeof(EventDto))]
        public IHttpActionResult FindEventForDonation(int id)
        {
            //Finds the first Event which has any Projects that match the inputed Project Id.
            Event Event = db.Events
                .Where(d => d.Donations.Any(p => p.DonationID == id))//Donations only functional once put into Event Model 
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Event == null)
            {
                return NotFound();
            }
            //put into a 'Data Transfer Object'
            EventDto EventDto = new EventDto
            {
                //All that is needed for the selection
                EventId = Event.EventId,
                Title = Event.Title
            };
            //pass along data as 200 status code OK response
            return Ok(EventDto);
        }
        /// <summary>
        ///     Will Update the Donation from the database by id.
        /// </summary>
        /// <param name="id">DonationID</param>
        /// <param name="donation">Donation Object</param>
        /// <returns>Successful or Not Successful</returns>
        // POST: api/DonationData/UpdateDonation/1
        [HttpPost]
        [Route("api/DonationData/UpdateDonation/{id}")]
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateDonation(int id, [FromBody] Donation donation)
        {
            /*If the Model State is not 
             * valid send a Bad Request*/
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /*If the id doesn't match an 
             * Donation Id send a Bad Request*/
            if (id != donation.DonationID)
            {
                return BadRequest();
            }
            /*Otherwise Update the inputed Project*/
            db.Entry(donation).State = EntityState.Modified;
            /*Save the changes => Catch 
             * if Donation Id does not exist*/
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
        /// <summary>
        ///     Adds an Donation to the database
        /// </summary>
        /// <param name="donation">Donation Object</param>
        /// <returns>Successful or Not Successful</returns>
        // POST: api/DonationData/AddDonation
        [HttpPost]
        [Route("api/DonationData/AddDonation")]
        [ResponseType(typeof(Donation))]
        //[Authorize(Roles = "Admin")]
        public IHttpActionResult AddDonation([FromBody] Donation donation)
        {
            donation.Date = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Donations.Add(donation);
            db.SaveChanges();

            return Ok(donation.DonationID);
        }
        /// <summary>
        ///     Deletes an Donation form the database by its id
        /// </summary>
        /// <param name="id">DonationID</param>
        /// <returns>Successful or Not Successful</returns>
        //  POST: api/DonationData/DeleteDonation/1
        [HttpPost]
        [Route("api/DonationData/DeleteDonation/{id}")]
        [ResponseType(typeof(Donation))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteDonation(int id)
        {
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return NotFound();
            }
            db.Donations.Remove(donation);
            db.SaveChanges();
            return Ok();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary>
        ///     Finds an Donation in the system. Internal use only.
        /// </summary>
        /// <param name="id">DonationID</param>
        /// <returns>TRUE if the Donation exists, false otherwise.</returns>
        private bool DonationExists(int id)
        {
            return db.Donations.Count(e => e.DonationID == id) > 0;
        }
    }
}