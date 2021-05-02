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

using System.IO;
using System.Web;


namespace TiminsHospitalProjectV3.Controllers
{/// <summary>
/// </summary>
    public class EventDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: api/EventData/GetEvents
        /// <summary>
        /// The following gets all Event data from the events table and makes it accessible to the MVC controller
        /// </summary>
        /// <returns> all events and their information</returns>
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
        /// <summary>
        /// Finds one specific event based off of ID
        /// </summary>
        /// <param name="id">the events ID</param>
        /// <returns> all info based off a specific event</returns>
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

            //All event info the @ creates a verbatem string instead of accidentaly using the event key word
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

            //Returns the Evetn DTO
               return Ok(EventDto);
        }
        /// <summary>
        /// Adds an event to the database
        /// </summary>
        /// <param name="event"> the event in question, holds all the information</param>
        /// <returns> This returns a new event with a new ID </returns>
        [ResponseType(typeof(Event))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        /// <summary>
        /// Allows Admin to delete an event
        /// </summary>
        /// <param name="id">the ID of the event to be deleted</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteEvent(int id)
        {

            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }
            if (@event.EventHasImage && @event.PicExtension != "")
            {
                //also delete image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/Events/" + id + "." + @event.PicExtension);
                if (System.IO.File.Exists(path))
                {
                    Debug.WriteLine("File exists... preparing to delete!");
                    System.IO.File.Delete(path);
                }
            }

            db.Events.Remove(@event);
            db.SaveChanges();

            return Ok();
        }
        /// <summary>
        /// Allows for the update of a specific Event
        /// </summary>
        /// <param name="id">The id of the event to be updated</param>
        /// <param name="event">The event in question/ The info</param>
        /// <returns>Returns an updated event</returns>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
            //The following two preevnt updates here
            db.Entry(@event).Property(p => p.EventHasImage).IsModified = false;
            db.Entry(@event).Property(p => p.PicExtension).IsModified = false;
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
        /// <summary>
        /// The following allows the admin to add an image, and update a current image
        /// </summary>
        /// <param name="id"> the ID of the event having the image added</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateEventImage(int id)
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
                    var EventImage = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (EventImage.ContentLength > 0)
                    {
                        //Checks the type of photo
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(EventImage.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image, this only allows one image per event
                                string fn = id + "." + extension;

                                //Speciies which folder the image is being stored in
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Events/"), fn);

                                //save the file
                                EventImage.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the player haspic and picextension fields in the database
                                Event SelectedEvent = db.Events.Find(id);
                                SelectedEvent.EventHasImage = haspic;
                                SelectedEvent.PicExtension = extension;
                                db.Entry(SelectedEvent).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Event Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                            }
                        }
                    }

                }
            }

            return Ok();
        }
        /// <summary>
        /// The following allows each event to display all donners names
        /// </summary>
        /// <param name="id">The ID of the event </param>
        /// <returns> a list of donners</returns>
        [ResponseType(typeof(IEnumerable<DonationDto>))]
        public IHttpActionResult GetDonationsForEvent(int id)
        {
            //ERROR IN CONTROLLER
            List<Donation> Donations = db.Donations.Where(p => p.EventId == id).ToList();

            List<DonationDto> DonationDtos = new List<DonationDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Donation in Donations)
            {
                DonationDto NewDonation = new DonationDto
                {
                    FistName = Donation.FistName,
                    LastName = Donation.LastName



                };
                DonationDtos.Add(NewDonation);
            }

            return Ok(DonationDtos);
        }
        /// <summary>
        /// The fllowing allows things to be completely deleted 
        /// </summary>
        /// <param name="disposing">the thing being disposed</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Checks to see if the event exists in te fistplace
        /// </summary>
        /// <param name="id">the ID of the event </param>
        /// <returns></returns>

        private bool EventExists(int id)
        {
            return db.NewsItems.Count(e => e.NewsItemID == id) > 0;
        }
    }
}
    

