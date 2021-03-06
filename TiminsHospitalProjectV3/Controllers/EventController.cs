using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using TiminsHospitalProjectV3.Models;
using TiminsHospitalProjectV3.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;

namespace TiminsHospitalProjectV3.Controllers
{
    
        // GET: Event
        public class EventController : Controller
        {
            private JavaScriptSerializer jss = new JavaScriptSerializer();
            private static readonly HttpClient client;


            static EventController()
            {
                HttpClientHandler handler = new HttpClientHandler()
                {
                    AllowAutoRedirect = false,
                    UseCookies = false

                };
                client = new HttpClient(handler);
                //change this to match your own local port number
                client.BaseAddress = new Uri("http://hospitalproject-env.eba-fm6cqgtc.us-east-2.elasticbeanstalk.com/api/");
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));



            }

        private void GetApplicationCookie()
        {
            string token = "";

            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }
        public ActionResult List()
            {

            ListEvents ViewModel = new ListEvents();
            ViewModel.isadmin = User.IsInRole("Admin");

            string url = "EventData/GetEvents";
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<EventDto> SelectedEvents = response.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;
                    ViewModel.events = SelectedEvents;

                return View(ViewModel);



                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
        public ActionResult Details(int id)
        {
            ShowEvent ViewModel = new ShowEvent();
            string url = "EventData/FindEvent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Hop data transfer object
                EventDto SelectedEvent = response.Content.ReadAsAsync<EventDto>().Result;
                ViewModel.Event = SelectedEvent;


                //Change
                url = "EventData/GetDonationsForEvent/" + id;
                response = client.GetAsync(url).Result;
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                IEnumerable<DonationDto> SelectedDonations = response.Content.ReadAsAsync<IEnumerable<DonationDto>>().Result;
                ViewModel.DonationToEvent = SelectedDonations;

                /*url = "hopdata/findHopClassificationforHop/" + id;
                response = client.GetAsync(url).Result;
                HopClassificationDto SelectedHopClassification = response.Content.ReadAsAsync<HopClassificationDto>().Result;
                ViewModel.hopClassification = SelectedHopClassification;*/

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hop/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Event EventInfo)
        {
            GetApplicationCookie();
            //Debug.WriteLine(NewsItem.NewsItemID);
            string url = "EventData/AddEvent";
            EventInfo.UserID = User.Identity.GetUserId();
            //USERID
            Debug.WriteLine(jss.Serialize(EventInfo.UserID));
            HttpContent content = new StringContent(jss.Serialize(EventInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    int eventid = response.Content.ReadAsAsync<int>().Result;
                    return RedirectToAction("Details", new
                    {
                        id = eventid,
                       


                    }) ;

                }
                catch (Exception e)
                {
                    Debug.WriteLine(EventInfo);
                    return RedirectToAction("List");
                }
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //Allows Admin to find specific Event for deletion
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "EventData/FindEvent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            if (response.IsSuccessStatusCode)
            {
                //Put data into Hop data transfer object
                EventDto SelectedEvent = response.Content.ReadAsAsync<EventDto>().Result;
                return View(SelectedEvent);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //Deletes the event
        // POST: Hop/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "EventData/DeleteEvent/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        //Allows the Admin to edit
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            UpdateEvent ViewModel = new UpdateEvent();

            string url = "EventData/FindEvent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                //Put data into Hop data transfer object
                EventDto SelectedEvent = response.Content.ReadAsAsync<EventDto>().Result;
                ViewModel.Event = SelectedEvent;
                //The following is for a forign key that may later be added

                /*url = "HopClassificationdata/getHopClassifications";
                response = client.GetAsync(url).Result;
                IEnumerable<HopClassificationDto> PotentialHops = response.Content.ReadAsAsync<IEnumerable<HopClassificationDto>>().Result;
                ViewModel.allhopclassifications = PotentialHops;
                */
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        ///<results>Edits a specific hop</results>


        //Alows the admin to edit or add photos to an event
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Event EventInfo, HttpPostedFileBase EventImage) 
        {
            GetApplicationCookie();
            string url = "EventData/UpdateEvent/" + id;
            EventInfo.UserID = User.Identity.GetUserId();

            Debug.WriteLine(jss.Serialize(EventInfo));
            HttpContent content = new StringContent(jss.Serialize(EventInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //only attempt to send player picture data if we have it
                if (EventImage != null)
                {
                    Debug.WriteLine("Calling Update Image method.");
                    //Send over image data for player
                    //Calls the update event images method
                    url = "EventData/UpdateEventImage/" + id;
                    Debug.WriteLine("Received player picture "+ EventImage.FileName);

                    MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                    HttpContent imagecontent = new StreamContent(EventImage.InputStream);
                    requestcontent.Add(imagecontent, "EventImage", EventImage.FileName);
                    response = client.PostAsync(url, requestcontent).Result;
                }

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// Error message
        /// </summary>
        /// <returns>Returns Error messages</returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}
