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
    public class DepartmentDataController : ApiController
    {
        //This variable is acts as a database access point
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Adds a department to the database.
        /// </summary>
        /// <param name="department">A department object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/DepartmentData/AddDepartment
        ///  FORM DATA: Department JSON Object
        /// </example>
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult AddDepartment([FromBody] Department department)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);
            db.SaveChanges();
            return Ok(department.DepartmentId);
        }


        /// <summary>
        /// Gets a list of departments in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of departments including their details.</returns>
        /// <example>
        /// GET : api/departmentdata/getdepartments
        /// </example>
        [ResponseType(typeof(IEnumerable<DepartmentDto>))]
        [Route("api/departmentdata/getdepartments")]
        public IHttpActionResult GetDepartments()
        {
            List<Department> Department = db.Departments.ToList();
            List<DepartmentDto> departmentDtos = new List<DepartmentDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Departments in Department)
            {
                DepartmentDto NewDepartment = new DepartmentDto
                {
                    DepartmentId = Departments.DepartmentId,
                    DepartmentName = Departments.DepartmentName,
                };
                departmentDtos.Add(NewDepartment);
            }

            return Ok(departmentDtos);
        }

        /// <summary>
        /// Finds a particular department in the database with a 200 status code. If the department is not found, return 404.
        /// </summary>
        /// <param name="id">The department id</param>
        /// <returns>Information about the department (i.e. department name)</returns>
        // <example>
        // GET: api/DepartmentData/FindDepartment/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult FindDepartment(int id)
        {
            //Find the data
            Department department = db.Departments.Find(id);
            //if not found, return 404 status code.
            if (department == null)
            {
                return NotFound();
            }
            

            //put into a 'friendly object format'
            DepartmentDto departmentDto = new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
            };


            //pass along data as 200 status code OK response
            return Ok(departmentDto);
        }

        /// <summary>
        /// Updates a department in the database given information about the department.
        /// </summary>
        /// <param name="id">The department id</param>
        /// <param name="Department">A department object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/DepartmentData/UpdateDepartment/5
        /// FORM DATA: Department JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDepartment(int id, [FromBody] Department Department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Department.DepartmentId)
            {
                return BadRequest();
            }

            db.Entry(Department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        //GetJobPostings for the department, Input- departmentID, output- JobPostings

        /// <summary>
        /// Finds a particular job posting in the database given a department id with a 200 status code. If the job posting is not found, return 404.
        /// </summary>
        /// <param name="id">The department id</param>
        /// <returns>Information about the job posting, including job posting id, title, Description, Category, Position Type, Salary Range, Location, DatePosted, Email and the name of the associated department</returns>
        // <example>
        // GET: api/DepartmentData/FindJobPostingsForDepartment/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<JobPostingDto>))]
        public IHttpActionResult FindJobPostingsForDepartment(int id)
        {

            List<JobPosting> JobPostings = db.JobPostings
                .Where(t => t.DepartmentID == id)
                .ToList();

            List<JobPostingDto> JobPostingDtos = new List<JobPostingDto> { };

            foreach (var JobPosting in JobPostings)
            {
                JobPostingDto NewJobPosting = new JobPostingDto
                {
                    JobId = JobPosting.JobId,
                    JobTitle = JobPosting.JobTitle,
                    JobDescription = JobPosting.JobDescription,
                    JobCategory = JobPosting.JobCategory,
                    JobLocation = JobPosting.JobLocation,
                    PositionType = JobPosting.PositionType,
                    SalaryRange = JobPosting.SalaryRange,
                    DatePosted = JobPosting.DatePosted.Date,
                    Email = JobPosting.Email,
                    DepartmentName = JobPosting.Department.DepartmentName
                };
                JobPostingDtos.Add(NewJobPosting);
            }
            return Ok(JobPostingDtos);
        }



        /// <summary>
        /// Deletes a department in the database
        /// </summary>
        /// <param name="id">The id of the department to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/DepartmentData/DeleteDepartment/5
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Department))]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            db.SaveChanges();

            return Ok(department);
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
        /// Finds a department in the system. Internal use only.
        /// </summary>
        /// <param name="id">The department id</param>
        /// <returns>TRUE if the department exists, false otherwise.</returns>
        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentId == id) > 0;
        }


    }
}

