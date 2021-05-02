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

namespace TiminsHospitalFaqV3.Controllers
{
    public class CategoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        ///     Returns a List of Categories in the database
        /// </summary>
        /// <example> GET: api/CategoryData/GetCategories </example>
        /// <returns>A list of Category information (Categroy Id, Category Name)</returns> (CHECKED)
        // GET: api/CategoryData/GetCategories
        [HttpGet]
        [Route("api/CategoryData/GetCategories")]
        [ResponseType(typeof(IEnumerable<CategoryDto>))]
        public IHttpActionResult GetCategories()
        {
            List<Category> categories = db.Categories.ToList();
            List<CategoryDto> categoryDtos = new List<CategoryDto> { };

            //Choose which information is exposed to the API
            foreach (var category in categories)
            {
                CategoryDto NewCategory = new CategoryDto
                {
                    CategoryID = category.CategoryID,
                    CategoryName = category.CategoryName
                };
                //Add the Category name to the list
                categoryDtos.Add(NewCategory);
            }
            return Ok(categoryDtos);
        }
        /// <summary>
        ///     Finding a Category by it's id
        /// </summary>
        /// <example> GET: api/CategoryData/FindCategory/1 </example>
        /// <param name="id">Caategory Id</param>
        /// <returns>All the information of the Category</returns> (CHECKED)
        // GET: api/CategoryData/FindCategory/1
        [HttpGet]
        [Route("api/CategoryData/FindCategory/{id}")]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult FindCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            CategoryDto categoryDto = new CategoryDto
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName
            };
            return Ok(categoryDto);
        }
        /// <summary>
        ///      Gets a list of Categories in the database
        /// </summary>
        /// <example> GET: api/CategoryData/GetFaqForCategory </example>
        /// <param name="id">The Category Id</param>
        /// <returns>Returns a list of Faqs associated with the Category</returns> (CHECKED)
        // GET: api/CategoryData/GetFaqsForCategory
        [HttpGet]
        [Route("api/CategoryData/GetFaqsForCategory/{id}")]
        [ResponseType(typeof(IEnumerable<FaqDto>))]
        public IHttpActionResult GetFaqsForCategory(int id)
        {
            List<Faq> faqs = db.Faqs.Where(t => t.CategoryID == id)
                .ToList();
            List<FaqDto> faqDtos = new List<FaqDto> { };

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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/CategoryData/GetAllFaqForCategory")]
        [ResponseType(typeof(IEnumerable<FaqDto>))]
        public IHttpActionResult GetAllFaqForCategory()
        {
            List<Faq> faqs = db.Faqs.ToList();
            List<Category> categories = db.Categories.ToList();
            List<FaqDto> faqDtos = new List<FaqDto> { };
            var faqxcategory = from c in db.Categories 
                               join f in db.Faqs on c.CategoryID equals f.CategoryID
                               into table1 from f in table1.ToList()
                               select new { category = c, faqs = f };

            foreach (var faq in faqxcategory)
            {
                FaqDto NewFaq = new FaqDto
                {
                    FaqID = faq.faqs.FaqID,
                    FaqQuestion = faq.faqs.FaqQuestion,
                    FaqAnswer = faq.faqs.FaqAnswer,
                    CategoryID = faq.faqs.CategoryID,

                };
                faqDtos.Add(NewFaq);
            }
            return Ok(faqDtos);
        }
        /// <summary>
        ///     Updates the Category in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/CategoryData/UpdateCategory/{id}")]
        [ResponseType(typeof(void))]
        //[Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            /*If the Model State is not 
             * valid send a Bad Request*/
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /*If the id doesn't match an 
             * Faq Id send a Bad Request*/
            if (id != category.CategoryID)
            {
                return BadRequest();
            }
            db.Entry(category).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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
        ///     Adds a Category to the database.
        /// </summary>
        /// <example> POST: api/CategoryData/AddCategory </example>
        /// <param name="category">A Category object</param>
        /// <returns>Successful or Not Successful</returns> (CHECKED)
        // POST: api/CategoryData/AddCategory
        [HttpPost]
        [Route("api/CategoryData/AddCategory")]
        [ResponseType(typeof(Category))]
        //[Authorize(Roles = "Admin")]
        public IHttpActionResult AddCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Categories.Add(category);
            db.SaveChanges();

            return Ok(category.CategoryID);
        }
        /// <summary>
        ///     Deletes a Category in the database
        /// </summary>
        /// <example> POST: api/CategoryData/DeleteCategory/4 </example>
        /// <param name="id">The id of the Category to delete</param>
        /// <returns>Successful or Not Successful</returns> (CHECKED)
        // POST: api/CategoryData/DeleteCategory/4
        [HttpPost]
        [Route("api/CategoryData/DeleteCategory/{id}")]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            db.Categories.Remove(category);
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
        ///     Finds a Category in the system. Internal use only.
        /// </summary>
        /// <param name="id">CategoryID</param>
        /// <returns></returns>
        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryID == id) > 0;
        }
    }
}