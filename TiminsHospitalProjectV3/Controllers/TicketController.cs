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
    public class TicketController : Controller
    {

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static TicketController()
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

        // GET: Tickets/List
        public ActionResult List()
        {
            string url = "TicketData/GetTickets";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<TicketDTO> SelectedTickets = response.Content.ReadAsAsync<IEnumerable<TicketDTO>>().Result;
                return View(SelectedTickets);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Ticket/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ticket/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Ticket NewTicket)
        {
            //Debug.WriteLine(NewsItem.NewsItemID);
            string url = "TicketData/AddTicket";
            Debug.WriteLine(jss.Serialize(NewTicket));
            HttpContent content = new StringContent(jss.Serialize(NewTicket));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    int ticketId = response.Content.ReadAsAsync<int>().Result;
                    return RedirectToAction("Details", new
                    {
                        id = ticketId
                    });

                }
                catch (Exception e)
                {
                    Debug.WriteLine(NewTicket);
                    return RedirectToAction("List");
                }
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Ticket/Details/1
        [HttpGet]
        public ActionResult Details(int id)
        {
            ShowTicket ViewModel = new ShowTicket();
            // ViewModel.isadmin = User.IsInRole("Admin");
            string url = "TicketData/FindTicket/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                TicketDTO SelectedTicket = response.Content.ReadAsAsync<TicketDTO>().Result;
                ViewModel.Ticket = SelectedTicket;
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "TicketData/FindTicket/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Hop data transfer object
                TicketDTO SelectedTicket = response.Content.ReadAsAsync<TicketDTO>().Result;
                return View(SelectedTicket);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "TicketData/DeleteTicket/" + id;
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

        public ActionResult Edit(int id)
        {
            UpdateTicket ViewModel = new UpdateTicket();

            string url = "TicketData/FindTicket/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                TicketDTO SelectedTicket = response.Content.ReadAsAsync<TicketDTO>().Result;
                ViewModel.Ticket = SelectedTicket;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Ticket/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]

        public ActionResult Edit(int id, Ticket TicketInfo)
        {

            string url = "TicketData/UpdateTicket/" + id;
            Debug.WriteLine(jss.Serialize(TicketInfo));
            HttpContent content = new StringContent(jss.Serialize(TicketInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}