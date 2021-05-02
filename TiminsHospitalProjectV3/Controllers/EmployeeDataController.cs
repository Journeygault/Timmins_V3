using System;
using System.IO;
using System.Web;
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
using System.Diagnostics;


namespace hospitalproject.Controllers
{
    public class EmployeeDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // <summary>
        /// Gets a list or job hunters in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of job hunters including their Id, first name,last name,phone number,email,address,description, and their resume</returns>
        /// <example>
        // GET: api/PetData
        /// </example>
        [ResponseType(typeof(IEnumerable<EmployeeDto>))]
        public IHttpActionResult GetEmployees()
        {
            List<Employee> Employees = db.Employees.ToList();
            List<EmployeeDto> EmployeeDtos = new List<EmployeeDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Employee in Employees)
            {
                EmployeeDto NewEmployee = new EmployeeDto
                {
                    EmployeeID = Employee.EmployeeID,
                    EmployeeFirstname = Employee.EmployeeFirstname,
                    EmployeeLastname = Employee.EmployeeLastname,
                    EmployeePhone = Employee.EmployeePhone,
                    EmployeeEmail = Employee.EmployeeEmail,
                    EmployeeAddress = Employee.EmployeeAddress,
                    EmployeeDes = Employee.EmployeeDes,
                    EmployeeHasPic = Employee.EmployeeHasPic,
                    EmployeeResume = Employee.EmployeeResume,
                    //will change in the future
                    JobID = Employee.JobID
                };
                EmployeeDtos.Add(NewEmployee);
            }

            return Ok(EmployeeDtos);
        }
        /// <summary>
        /// Finds a particular job hunters in the database with a 200 status code. If the player is not found, return 404.
        /// </summary>
        /// <param name="id">The job hunters id</param>
        /// <returns>Information about the job hunters including their Id, first name,last name,phone number,email,address,description, and their resume.</returns>
        // <example>
        // GET: api/EmployeeData/FindEmployee/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(EmployeeDto))]
        public IHttpActionResult FindEmployee(int id)
        {
            //Find the data
            Employee Employee = db.Employees.Find(id);
            //if not found, return 404 status code.
            if (Employee == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            EmployeeDto EmployeeDto = new EmployeeDto
            {
                EmployeeID = Employee.EmployeeID,
                EmployeeFirstname = Employee.EmployeeFirstname,
                EmployeeLastname = Employee.EmployeeLastname,
                EmployeePhone = Employee.EmployeePhone,
                EmployeeEmail = Employee.EmployeeEmail,
                EmployeeAddress = Employee.EmployeeAddress,
                EmployeeDes = Employee.EmployeeDes,
                EmployeeHasPic = Employee.EmployeeHasPic,
                EmployeeResume = Employee.EmployeeResume,
                //will change in the future -test
                JobID = Employee.JobID
            };


            //pass along data as 200 status code OK response
            return Ok(EmployeeDto);
        }
        /// <summary>
        /// Finds a particular job posting in the database given a job hunter id with a 200 status code. If the job is not found, return 404.
        /// </summary>
        /// <param name="id">The job hunters id</param>
        /// <returns>Information about the job hunters including their Id, first name,last name,phone number,email,address,description, and their resume.</returns>
        // <example>
        // GET: api/JObData/FindJobForEmployees/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(Job_PostingDto))]
        public IHttpActionResult FindJob_PostingForEmployee(int id)
        {
            //Finds the first job which has any job applicants
            //that match the input Employeeid
            Job_Posting Job_Posting = db.Job_Postings
                .Where(t => t.Employees.Any(p => p.EmployeeID == id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Job_Posting == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            Job_PostingDto Job_PostingDto = new Job_PostingDto
            {
                JobID = Job_Posting.JobID,
                JobName = Job_Posting.JobName
            };


            //pass along data as 200 status code OK response
            return Ok(Job_PostingDto);
        }
        /// <summary>
        /// Updates a job hunters in the database given information about the job hunters.
        /// </summary>
        /// <param name="id">The job hunters id</param>
        /// <param name="employee">A job hunters object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/EmployeeData/UpdateEmployee/5
        /// FORM DATA: Employee JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;
            // Picture update is handled by another method
            db.Entry(employee).Property(p => p.EmployeeHasPic).IsModified = false;
            db.Entry(employee).Property(p => p.EmployeeResume).IsModified = false;

            try
            {
                db.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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
        /// Receives job hunters'resume data, uploads it to the webserver and updates the job hunters'resume option
        /// </summary>
        /// <param name="id">the job hunters id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F employeepic=@file.jpg "https://localhost:xx/api/employeedata/updateemployeepic/2"
        /// POST: api/EmployeeData/UpdateEmployeePic/3
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        /// https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data

        [HttpPost]
        public IHttpActionResult UpdateEmployeePic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var EmployeePic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (EmployeePic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(EmployeePic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/Employees/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Employees/"), fn);

                                //save the file
                                EmployeePic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the player haspic and picextension fields in the database
                                Employee SelectedEmployee = db.Employees.Find(id);
                                SelectedEmployee.EmployeeHasPic = haspic;
                                SelectedEmployee.EmployeeResume = extension;
                                db.Entry(SelectedEmployee).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Resume was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                            }
                        }
                    }

                }
            }

            return Ok();
        }
        /// <summary>
        /// Adds a job hunters to the database.
        /// </summary>
        /// <param name="pet">A job hunters object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/EmployeeData/AddPet
        ///  FORM DATA: Employee JSON Object
        /// </example>
        [ResponseType(typeof(Employee))]
        [HttpPost]
        public IHttpActionResult AddEmployee([FromBody] Employee employee)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Employees.Add(employee);
            db.SaveChanges();

            return Ok(employee.EmployeeID);
        }

        /// <summary>
        /// Deletes a job hunter in the database
        /// </summary>
        /// <param name="id">The id of the job hunter to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        // DELETE: api/EmployeeData/DeleteEmployee/5
        /// </example>
        [HttpPost]

        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            if (employee.EmployeeHasPic && employee.EmployeeResume != "")
            {
                //also delete image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/Employees/" + id + "." + employee.EmployeeResume);
                if (System.IO.File.Exists(path))
                {
                    Debug.WriteLine("File exists... preparing to delete!");
                    System.IO.File.Delete(path);
                }
            }
            db.Employees.Remove(employee);
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
        /// Finds a job hunter in the system. Internal use only.
        /// </summary>
        /// <param name="id">The job hunter id</param>
        /// <returns>TRUE if the job hunter exists, false otherwise.</returns>
        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.EmployeeID == id) > 0;
        }
    }
}