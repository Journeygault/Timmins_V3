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
        // GET: api/TicketData/GetTickets
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

                };
                TicketDTOs.Add(NewTicket);
            }

            return Ok(TicketDTOs);
        }

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

        // GET: api/TicketData/FindTicket/1
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
    }
}
