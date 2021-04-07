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
    public class FaqDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        ///     --
        /// </summary>
        /// <returns>A list of all Faqs in the database</returns>(checked)
        // GET: api/FaqData/GetFaqs
        [HttpGet]
        [Route("api/FaqData/GetFaqs")]
        [ResponseType(typeof(IEnumerable<FaqDto>))]
        public IHttpActionResult GetFaqs()
        {
            List<Faq> faqs = db.Faqs.ToList();
            List<FaqDto> faqDtos = new List<FaqDto> { };
            foreach (var faq in faqs)
            {
                FaqDto NewFaq = new FaqDto
                {
                    FaqID = faq.FaqID,
                    FaqQuestion = faq.FaqQuestion,
                    FaqAnswer = faq.FaqAnswer
                };
                faqDtos.Add(NewFaq);
            }
            return Ok(faqDtos);
        }
        /// <summary>
        ///     --
        /// </summary>
        /// <param name="id">FaqID</param>
        /// <returns>An Faq</returns>(checked)
        // GET: api/FaqData/FindFaq/1
        [HttpGet]
        [Route("api/FaqData/FindFaq/{id}")]
        [ResponseType(typeof(Faq))]
        public IHttpActionResult FindFaq(int id)
        {
            Faq faq = db.Faqs.Find(id);
            if (faq == null)
            {
                return NotFound();
            }
            FaqDto faqDto = new FaqDto
            {
                FaqID = faq.FaqID,
                FaqQuestion = faq.FaqQuestion,
                FaqAnswer = faq.FaqAnswer
            };
            return Ok(faqDto);
        }
        /// <summary>
        ///     --
        /// </summary>
        /// <param name="id">FaqID</param>
        /// <param name="faq">Parameter from models.Faq</param>
        /// <returns></returns>
        // POST: api/FaqData/UpdateFaq/1
        [HttpPost]
        [Route("api/FaqData/UpdateFaq/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateFaq(int id, [FromBody] Faq faq)
        {
            /*If the Model State is not 
             * valid send a Bad Request*/
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /*If the id doesn't match an 
             * Faq Id send a Bad Request*/
            if (id != faq.FaqID)
            {
                return BadRequest();
            }
            /*Otherwise Update the inputed Project*/
            db.Entry(faq).State = EntityState.Modified;
            /*Save the changes => Catch 
             * if Faq Id does not exist*/
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaqExists(id))
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
        ///     --
        /// </summary>
        /// <param name="faq"></param>
        /// <returns></returns>
        // POST: api/FaqData/AddFaq
        [HttpPost]
        [Route("api/FaqData/AddFaq")]
        [ResponseType(typeof(Faq))]
        public IHttpActionResult AddFaq([FromBody] Faq faq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Faqs.Add(faq);
            db.SaveChanges();

            return Ok(faq.FaqID);
        }
        /// <summary>
        ///     --
        /// </summary>
        /// <param name="id">FaqID</param>
        /// <returns></returns>
        //  POST: api/FaqData/DeleteFaq/1
        [HttpPost]
        [Route("api/FaqData/DeleteFaq/{id}")]
        [ResponseType(typeof(Faq))]
        public IHttpActionResult DeleteFaq(int id)
        {
            Faq faq = db.Faqs.Find(id);
            if (faq == null)
            {
                return NotFound();
            }
            db.Faqs.Remove(faq);
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
        ///     Finds a Faq in the system. Internal use only.
        /// </summary>
        /// <param name="id">FaqID</param>
        /// <returns>TRUE if the Faq exists, false otherwise.</returns>
        private bool FaqExists(int id)
        {
            return db.Faqs.Count(e => e.FaqID == id) > 0;
        }
    }
}