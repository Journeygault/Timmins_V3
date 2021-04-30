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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Globalization;

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
                AllowAutoRedirect = false,
                // Allows usage of cookies
                UseCookies = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44346/api/");
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

        // GET: Tickets/List
        public ActionResult List()
        {
            ListTicket ViewModel = new ListTicket();
            ViewModel.isadmin = User.IsInRole("Admin"); //Checks to see if the user is an admin

            string url = "TicketData/GetTickets";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<TicketDTO> SelectedTickets = response.Content.ReadAsAsync<IEnumerable<TicketDTO>>().Result;
                ViewModel.tickets = SelectedTickets;
                return View(ViewModel);
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(CreateTicket NewTicket)
        {
            // Checking cookies to check if user is logged in admin
            GetApplicationCookie();

            //Ticket newTicket = new Ticket();
            NewTicket.UserID = User.Identity.GetUserId();
            NewTicket.TicketDate = DateTime.Now;
            //newTicket.TicketTitle = NewTicket.TicketTitle;
            //newTicket.TicketBody = NewTicket.TicketBody;
            //newTicket.UserID = NewTicket.UserID;

            Debug.WriteLine(User.Identity.GetUserId());
            //Debug.WriteLine(newTicket.User.Id);
            //Debug.WriteLine(newTicket.UserID);

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken()]

        public ActionResult Edit(int id, Ticket TicketInfo)
        {
            GetApplicationCookie();

            string url = "TicketData/UpdateTicket/" + id;
            TicketInfo.UserID = User.Identity.GetUserId();
            Debug.WriteLine(TicketInfo.UserID);

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