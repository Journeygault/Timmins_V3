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
        ///     Lists out all the FAQs in the database -- SearchKey is used to find an FAQ that matches the input
        /// </summary>
        /// <returns>A list of all Faqs in the database -or- A list of all Faqs in the database that match</returns>(checked)
        //Trying to use the search key to find individual FAQ's
        // GET: api/FaqData/ListFaqs/{FaqSearchKey?}
        [HttpGet]
        [Route("api/FaqData/ListFaqs/{FaqSearchKey?}")]
        [ResponseType(typeof(IEnumerable<FaqDto>))]
        public IHttpActionResult ListFaqs(string FaqSearchKey = null)
        {
            List<Faq> faqs;
            if (FaqSearchKey != null) { faqs = db.Faqs.Where(t => t.FaqQuestion.Contains(FaqSearchKey) || t.FaqAnswer.Contains(FaqSearchKey)).ToList(); }
            else { faqs = db.Faqs.ToList(); }
            List<FaqDto> faqDtos = new List<FaqDto> { };
            //Find to list everything and a way to read the search key!---
            foreach (var faq in faqs)
            {
                FaqDto NewFaq = new FaqDto
                {
                    FaqID = faq.FaqID,
                    FaqQuestion = faq.FaqQuestion,
                    FaqAnswer = faq.FaqAnswer,
                    CategoryID = faq.CategoryID
                };
                faqDtos.Add(NewFaq);
            }
            return Ok(faqDtos);
        }
        /// <summary>
        ///     Finding an FAQ by it's id
        /// </summary>
        /// <param name="id">FaqID</param>
        /// <returns>All the information of the Faq</returns>(checked)
        // GET: api/FaqData/FindFaq/1
        [HttpGet]
        [Route("api/FaqData/FindFaq/{id}")]
        [ResponseType(typeof(FaqDto))]
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
                FaqAnswer = faq.FaqAnswer,
                CategoryID = faq.CategoryID
            };
            return Ok(faqDto);
        }
        /// <summary>
        ///     This Finds the Category for an Faq by the Project Id.
        /// </summary>
        /// <param name="id">Faq Id</param>
        /// <returns>All the information of the Category</returns> (CHECKED)
        // GET: api/CategoryData/FindCategoryForFaq/1
        [HttpGet]
        [Route("api/CategoryData/FindCategoryForFaq/{id}")]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult FindCategoryForFaq(int id)
        {
            //Finds the first Category which has any Projects that match the inputed Project Id.
            Category Category = db.Categories
                .Where(t => t.Faqs.Any(p => p.FaqID == id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Category == null)
            {
                return NotFound();
            }
            //put into a 'Data Transfer Object'
            CategoryDto CategoryDto = new CategoryDto
            {
                CategoryID = Category.CategoryID,
                CategoryName = Category.CategoryName
            };
            //pass along data as 200 status code OK response
            return Ok(CategoryDto);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //
        [HttpGet]
        [Route("api/CategoryData/GetAllCategories")]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult GetAllCategories()
        {
            List<Category> categories = db.Categories.ToList();
            List<Faq> faqs = db.Faqs.ToList();
            List<CategoryDto> categoryDtos = new List<CategoryDto> { };
            var categoryxfaq = from f in db.Faqs
                               join c in db.Categories on f.CategoryID equals c.CategoryID
                               into table1
                               from c in table1.ToList()
                               select new { category = c, faqs = f };

            foreach (var Category in categoryxfaq)
            {
                CategoryDto NewCategory = new CategoryDto
                {
                    CategoryID = Category.category.CategoryID,
                    CategoryName = Category.category.CategoryName
                };
                categoryDtos.Add(NewCategory);
            }
            return Ok(categoryDtos);
        }
        /// <summary>
        ///     Will Update the FAQ from the database by id.
        /// </summary>
        /// <param name="id">FaqID</param>
        /// <param name="faq">Faq Object</param>
        /// <returns>Successful or Not Successful</returns>
        // POST: api/FaqData/UpdateFaq/1
        [HttpPost]
        [Route("api/FaqData/UpdateFaq/{id}")]
        [ResponseType(typeof(void))]
        //[Authorize(Roles = "Admin")]
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
        ///     Adds an FAQ to the database
        /// </summary>
        /// <param name="faq">Faq Object</param>
        /// <returns>Successful or Not Successful</returns>
        // POST: api/FaqData/AddFaq
        [HttpPost]
        [Route("api/FaqData/AddFaq")]
        [ResponseType(typeof(Faq))]
        //[Authorize(Roles = "Admin")]
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
        ///     Deletes an FAQ form the database by its id
        /// </summary>
        /// <param name="id">FaqID</param>
        /// <returns>Successful or Not Successful</returns>
        //  POST: api/FaqData/DeleteFaq/1
        [HttpPost]
        [Route("api/FaqData/DeleteFaq/{id}")]
        [ResponseType(typeof(Faq))]
        //[Authorize(Roles = "Admin")]
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
        ///     Finds an Faq in the system. Internal use only.
        /// </summary>
        /// <param name="id">FaqID</param>
        /// <returns>TRUE if the Faq exists, false otherwise.</returns>
        private bool FaqExists(int id)
        {
            return db.Faqs.Count(e => e.FaqID == id) > 0;
        }
    }
}