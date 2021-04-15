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
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: api/DepartmentData/5
        [ResponseType(typeof(Department))]
        public IHttpActionResult GetDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        
        // DELETE: api/DepartmentData/5
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

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentId == id) > 0;
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
        /// Gets a list or customers in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of customers including their details.</returns>
        /// <example>
        /// GET : api/customersdata/getcustomers
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
        /// Finds a particular Team in the database with a 200 status code. If the Team is not found, return 404.
        /// </summary>
        /// <param name="id">The Team id</param>
        /// <returns>Information about the Team, including Team id, bio, first and last name, and teamid</returns>
        // <example>
        // GET: api/TeamData/FindTeam/5
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
        //GetJobPostings for the department In- departmentID, op- JobPostings
    }
}

