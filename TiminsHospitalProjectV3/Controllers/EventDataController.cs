﻿using System;
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
    public class EventDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: api/EventData/GetEvents
        [ResponseType(typeof(IEnumerable<EventDto>))]
        public IHttpActionResult GetEvents()
        {
            List<Event> Events = db.Events.ToList();
            List<EventDto> EventDtos = new List<EventDto> { };

            foreach (var Event in Events)
            {
                EventDto NewEvent = new EventDto
                {

                    EventId = Event.EventId,
                    Title = Event.Title,
                    EventHasImage = Event.EventHasImage,
                    PicExtension = Event.PicExtension,
                    EventDate = Event.EventDate,
                    EventDisc = Event.EventDisc,
                    UserID =Event.UserID,
                    EventHasOcured=Event.EventHasOcured
                };
                EventDtos.Add(NewEvent);
            }

            return Ok(EventDtos);
        }
        [HttpGet]
        // GET: api/EventData/FindEvent/id

        [ResponseType(typeof(EventDto))]

        public IHttpActionResult FindEvent(int id)
        {
            //Find the data
            Event Event = db.Events.Find(id);
            //if not found, return 404 status code.
            if (Event == null)
            {
                return NotFound();
            }

            //An easy to access entry into the Hop information, safely through the DTO
            EventDto @EventDto = new EventDto
            {
                EventId = Event.EventId,
                Title = Event.Title,
                EventHasImage = Event.EventHasImage,
                PicExtension = Event.PicExtension,
                EventDate = Event.EventDate,
                EventDisc = Event.EventDisc,
                UserID = Event.UserID,
                EventHasOcured = Event.EventHasOcured
            };


            //pass along data as 200 status code OK response
            return Ok(EventDto);
        }
        [ResponseType(typeof(Event))]
        [HttpPost]
        public IHttpActionResult AddEvent([FromBody] Event @event)
        {
            //Just checks to see if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Events.Add(@event);
            db.SaveChanges();

            return Ok(@event.EventId); ;
        }
        [HttpPost]
        public IHttpActionResult DeleteEvent(int id)
        {

            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }
            Debug.WriteLine(@event);

            db.Events.Remove(@event);
            db.SaveChanges();

            return Ok();
        }
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateEvent(int id, [FromBody] Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @event.EventId)
            {
                return BadRequest();
            }

            db.Entry(@event).State = EntityState.Modified;

            try
            {
                //    Debug.WriteLine(hop);

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        private bool EventExists(int id)
        {
            return db.NewsItems.Count(e => e.NewsItemID == id) > 0;
        }
    }
}
    
