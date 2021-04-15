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
    public class AppointmentsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// gets the list of appointments in the database
        /// </summary>
        /// <returns>The list of appointments in the database</returns>
        /// <example>GET: api/AppointmentsData/GetAppointments</example>
        [ResponseType(typeof(IEnumerable<Appointment>))]
        public IHttpActionResult GetAppointments()
        {
            return Ok(db.Appointments.OrderByDescending(a => a.SentOn).ToList());
        }
        /// <summary>
        /// Gets a list of appointments in the database alongside a status code (200 OK). Skips the first {startindex} records and takes {nberPerPage} records.
        /// </summary>
        /// <param name="startIndex">The number of records to skip through</param>
        /// <param name="nberPerPage"> The number of records for each</param>
        /// <returns> a list of appointements</returns>
        /// <example>GET: api/AppointmentsData/GetAppointmentsPage/5/10</example>
        [ResponseType(typeof(IEnumerable<Appointment>))]
        [Route("api/AppointmentsData/GetAppointmentsPage/{startIndex}/{nberPerPage}")]

        public IHttpActionResult GetAppointmentsPage(int startIndex, int nberPerPage)
        {
            return Ok(db.Appointments.OrderByDescending(a => a.SentOn).Skip(startIndex).Take(nberPerPage).ToList());
        }


        /// <summary>
        /// gets a user's list of appointments in the database
        /// </summary>
        /// <param name="userId">a user id</param>
        /// <returns>The list of a user's appointments in the database</returns>
        /// <example>GET: api/AppointmentsData/FindUserAppointments/4</example>
        [ResponseType(typeof(IEnumerable<Appointment>))]
        [Route("api/AppointmentsData/FindUserAppointments/{userId}")]
        [HttpGet]
        public IHttpActionResult FindUserAppointments(string userId)
        {
            return Ok(db.Appointments.Where(a => a.PatientID == userId || a.PhysicianID == userId).OrderByDescending(a => a.SentOn).ToList());
        }
        /// <summary>
        /// Gets a list of appointments associated with a user in the database alongside a status code (200 OK). Skips the first {startindex} records and takes {nberPerPage} records.
        /// </summary>
        /// <param name="userId">a user id</param>
        /// <param name="startIndex">The number of records to skip through</param>
        /// <param name="nberPerPage"> The number of records for each</param>
        /// <returns> a list of appointements</returns>
        /// <example>GET: api/AppointmentsData/FindUserAppointmentsPage/{userId}/{startIndex}/{nberPerPage}</example>
        [HttpGet]
        [Route("api/AppointmentsData/FindUserAppointmentsPage/{userId}/{startIndex}/{nberPerPage}")]
        public IHttpActionResult FindUserAppointmentsPage(string userId, int startIndex, int nberPerPage)
        {
            return Ok(db.Appointments.Where(a => a.PatientID == userId || a.PhysicianID == userId).OrderByDescending(a => a.SentOn)
                .Skip(startIndex).Take(nberPerPage).ToList());
        }

        // GET: api/AppointmentsData/5
        /// <summary>
        /// gets an appointment associated with a prodided id
        /// </summary>
        /// <param name="id"> an appointment id</param>
        /// <returns> an appoinment associated with the id provided</returns>
        /// <example>GET: api/AppointmentsData/GetAppointment/8</example>
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult GetAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }


        /// <summary>
        /// updates an appointmemnt in the database
        /// </summary>
        /// <param name="id">an appointment id</param>
        /// <param name="appointment">the appointment to update</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>POST: api/AppointmentsData/UpdateAppointment/5</example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAppointment(int id, Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.ID)
            {
                return BadRequest();
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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
        /// adds a new appointment in the  database
        /// </summary>
        /// <param name="appointment"> a new appointment</param>
        /// <returns> the newly added appointment's id</returns>
        /// <example>POST: api/AppointmentsData/AddAppointment</example>
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult AddAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return Ok(appointment.ID);
        }


        /// <summary>
        /// deletes an appointment in the database
        /// </summary>
        /// <param name="id"> an appointment id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>DELETE: api/AppointmentsData/DeleteAppointment/5</example>
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            return Ok(appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.ID == id) > 0;
        }
    }
}