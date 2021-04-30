using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http.Description;
using TiminsHospitalProjectV3.Models;
using System.Diagnostics;

namespace TiminsHospitalProjectV3.Controllers
{
    public class TicketDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// gets the list of tickets in the database
        /// </summary>
        /// <returns>The list of tickets in the database</returns>
        /// <example>GET: api/TicketData/GetTickets</example>
        [ResponseType(typeof(IEnumerable<TicketDTO>))]
        public IHttpActionResult GetTickets()
        {
            List<Ticket> Tickets = db.Tickets.ToList();
            List<TicketDTO> TicketDTOs = new List<TicketDTO> { };

            foreach (var specificTicket in Tickets)
            {
                TicketDTO NewTicket = new TicketDTO
                {
                    TicketId = specificTicket.TicketId,
                    TicketTitle = specificTicket.TicketTitle,
                    TicketBody = specificTicket.TicketBody,
                    TicketDate = specificTicket.TicketDate,
                    UserID = specificTicket.UserID,
                    User = specificTicket.User

                };
                TicketDTOs.Add(NewTicket);
            }

            return Ok(TicketDTOs);
        }

        /// <summary>
        ///     Adds a ticket to the database
        /// </summary>
        /// <param name="ticket">The Ticket being created</param>
        /// <returns> A new ticket</returns>
        /// <example>POST: api/TicketData/TicketData</example>
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Ticket))]
        [HttpPost]
        public IHttpActionResult AddTicket([FromBody] Ticket ticket)
        {
            //Just checks to see if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tickets.Add(ticket);
            db.SaveChanges();

            return Ok(ticket.TicketId); ;
        }

        /// <summary>
        ///     Finds a ticket in the database
        /// </summary>
        /// <param name="id">The id of the Ticket being searched for</param>
        /// <returns> a ticket </returns>
        /// <example>GET: api/TicketData/findticket/1 </example>
        [HttpGet]
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult FindTicket(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }
            TicketDTO ticketDTO = new TicketDTO
            {
                TicketId = ticket.TicketId,
                TicketTitle = ticket.TicketTitle,
                TicketBody = ticket.TicketBody,
                TicketDate = ticket.TicketDate
            };
            return Ok(ticketDTO);
        }

        // GET: api/TicketData/DeleteTicket
        /// <summary>
        ///     Deletes a ticket
        /// </summary>
        /// <param name="id">The ID of the ticket being deleted</param>
        /// <returns> Returns OK </returns>
        [HttpPost]
        public IHttpActionResult DeleteTicket(int id)
        {

            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }
            Debug.WriteLine(ticket);

            db.Tickets.Remove(ticket);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        ///     Updates a ticket
        /// </summary>
        /// <param name="id">The ticket ID</param>
        /// <param name="ticket">the ticket information to be updated </param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTicket(int id, [FromBody] Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticket.TicketId)
            {
                return BadRequest();
            }

            db.Entry(ticket).State = EntityState.Modified;

            try
            {

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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
        /// Checks to see if the ticket exists
        /// </summary>
        /// <param name="id">The ID of the ticket being checked</param>
        /// <returns></returns>
        private bool TicketExists(int id)
        {
            return db.Tickets.Count(e => e.TicketId == id) > 0;
        }
    }
}
