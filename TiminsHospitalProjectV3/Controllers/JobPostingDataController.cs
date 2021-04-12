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
    public class JobPostingDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gets a list or customers in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of customers including their details.</returns>
        /// <example>
        /// GET : api/customersdata/getcustomers
        /// </example>
        [ResponseType(typeof(IEnumerable<JobPostingDto>))]
        [Route("api/jobpostingdata/getjobposts")]
        public IHttpActionResult GetJobPosts()
        {
            List<JobPosting> JobPosts = db.JobPostings.ToList();
            List<JobPostingDto> JobPostsDtos = new List<JobPostingDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var JobPost in JobPosts)
            {
                JobPostingDto NewJobPost = new JobPostingDto
                {
                    JobId = JobPost.JobId,
                    JobTitle = JobPost.JobTitle,
                    JobDescription = JobPost.JobDescription,
                    JobCategory = JobPost.JobCategory,
                    JobLocation = JobPost.JobLocation,
                    PositionType = JobPost.PositionType,
                    SalaryRange = JobPost.SalaryRange,
                    DatePosted = JobPost.DatePosted,
                    Email = JobPost.Email
                };
                JobPostsDtos.Add(NewJobPost);
            }

            return Ok(JobPostsDtos);
        }

        // PUT: api/JobPostingData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJobPosting(int id, JobPosting jobPosting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jobPosting.JobId)
            {
                return BadRequest();
            }

            db.Entry(jobPosting).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobPostingExists(id))
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

        // POST: api/JobPostingData
        [ResponseType(typeof(JobPosting))]
        public IHttpActionResult PostJobPosting(JobPosting jobPosting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.JobPostings.Add(jobPosting);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = jobPosting.JobId }, jobPosting);
        }

        // DELETE: api/JobPostingData/5
        [HttpPost]
        [ResponseType(typeof(JobPosting))]
        public IHttpActionResult DeleteJobPosting(int id)
        {
            JobPosting jobPosting = db.JobPostings.Find(id);
            if (jobPosting == null)
            {
                return NotFound();
            }

            db.JobPostings.Remove(jobPosting);
            db.SaveChanges();

            return Ok(jobPosting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JobPostingExists(int id)
        {
            return db.JobPostings.Count(e => e.JobId == id) > 0;
        }


        /// <summary>
        /// Adds a customer to the database.
        /// </summary>
        /// <param name="customer">A player object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/CustomersData/AddCustomer
        ///  FORM DATA: Player JSON Object
        /// </example>
        [ResponseType(typeof(JobPosting))]
        [HttpPost]
        public IHttpActionResult AddJobPost([FromBody] JobPosting jobPosting)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.JobPostings.Add(jobPosting);
            db.SaveChanges();

            return Ok(jobPosting.JobId);
        }



        /// <summary>
        /// Finds a particular Team in the database with a 200 status code. If the Team is not found, return 404.
        /// </summary>
        /// <param name="id">The Team id</param>
        /// <returns>Information about the Team, including Team id, bio, first and last name, and teamid</returns>
        // <example>
        // GET: api/TeamData/FindTeam/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(JobPostingDto))]
        public IHttpActionResult FindPost(int id)
        {
            //Find the data
            JobPosting jobPosting = db.JobPostings.Find(id);
            //if not found, return 404 status code.
            if (jobPosting == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            JobPostingDto jobPostingDto = new JobPostingDto
            {
                JobId = jobPosting.JobId,
                JobTitle = jobPosting.JobTitle,
                JobDescription = jobPosting.JobDescription,
                JobCategory = jobPosting.JobCategory,
                JobLocation = jobPosting.JobLocation,
                PositionType = jobPosting.PositionType,
                SalaryRange = jobPosting.SalaryRange,
                DatePosted = jobPosting.DatePosted,
                Email = jobPosting.Email
            };


            //pass along data as 200 status code OK response
            return Ok(jobPostingDto);
        }

        /// <summary>
        /// Updates a Team in the database given information about the Team.
        /// </summary>
        /// <param name="id">The Team id</param>
        /// <param name="Team">A Team object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/TeamData/UpdateTeam/5
        /// FORM DATA: Team JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePost(int id, [FromBody] JobPosting jobPosting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jobPosting.JobId)
            {
                return BadRequest();
            }

            db.Entry(jobPosting).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobPostingExists(id))
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
    }
}