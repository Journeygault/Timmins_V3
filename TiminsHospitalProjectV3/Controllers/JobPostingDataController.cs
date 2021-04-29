using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
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
        /// Gets a list or job posts in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of job posts including their details.</returns>
        /// <example>
        /// GET : api/jobpostingdata/getjobposts
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
                    DatePosted = JobPost.DatePosted.Date,
                    Email = JobPost.Email,
                    DepartmentName = JobPost.Department.DepartmentName
                };
                JobPostsDtos.Add(NewJobPost);
            }

            return Ok(JobPostsDtos);
        }

        /// <summary>
        /// Deletes a post in the database
        /// </summary>
        /// <param name="id">The id of the post to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/jobpostingdata/DeleteJobPosting/5
        /// </example>
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

   

        /// <summary>
        /// Adds a post to the database.
        /// </summary>
        /// <param name="jobPosting">A post object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/jobpostingdata/AddJobPost
        ///  FORM DATA: JobPosting JSON Object
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
        /// Finds a particular post in the database with a 200 status code. If the post is not found, return 404.
        /// </summary>
        /// <param name="id">The post id</param>
        /// <returns>Information about the post, including job id, title, description, category, location, position type, salary range, date posted, email</returns>
        // <example>
        // GET: api/jobpostingdata/FindPost/5
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
                DatePosted = jobPosting.DatePosted.Date,
                Email = jobPosting.Email,
                DepartmentName = jobPosting.Department.DepartmentName,

            };


            //pass along data as 200 status code OK response
            return Ok(jobPostingDto);
        }

        /// <summary>
        /// Updates a post in the database given information about the post.
        /// </summary>
        /// <param name="id">The jobPosting id</param>
        /// <param name="jobPosting">A jobPosting object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/jobpostingdata/UpdatePost/5
        /// FORM DATA: JobPosting JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePost(int id, [FromBody] JobPosting jobPosting)
        {
            Debug.WriteLine("id::" + jobPosting.JobId);
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

    }
}