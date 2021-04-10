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
                    AllowAutoRedirect = false
                };
                client = new HttpClient(handler);
                //change this to match your own local port number
                client.BaseAddress = new Uri("https://localhost:44346/api/");
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));



            }
            public ActionResult List()
            {
                string url = "EventData/GetEvents";
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<EventDto> SelectedEvents = response.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;
                    return View(SelectedEvents);
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
        public ActionResult Create(Event EventInfo)
        {
            //Debug.WriteLine(NewsItem.NewsItemID);
            string url = "EventData/AddEvent";
            Debug.WriteLine(jss.Serialize(EventInfo));
            HttpContent content = new StringContent(jss.Serialize(EventInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    int newsItemid = response.Content.ReadAsAsync<int>().Result;
                    return RedirectToAction("Details", new
                    {
                        id = newsItemid
                    });

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
    }
    }
