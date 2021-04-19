using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TiminsHospitalProjectV3.Models;
//Added Script Seralization for inputs
using System.Web.Script.Serialization;
using TiminsHospitalProjectV3.Models.ViewModels;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Net.Http;

namespace TiminsHospitalProjectV3.Controllers
{
    public class DonationController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private string id;
        private static readonly HttpClient client;

        static DonationController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44346/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }
        /// <summary>
        /// Grabs the authentication credentials which are sent to the Controller.
        /// This is NOT considered a proper authentication technique for the WebAPI. It piggybacks the existing authentication set up in the template for Individual User Accounts. Considering the existing scope and complexity of the course, it works for now.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }
        /// <returns>All Donations in the database if response is successfull -
        /// otherwise redirects to error page</returns>
        // GET: Donation/List
        public ActionResult List()
        {
            ListDonations ViewModel = new ListDonations();
            ViewModel.isadmin = User.IsInRole("Admin");

            string url = "DonationData/GetDonations";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<DonationDto> SelectedDonations = response.Content.ReadAsAsync<IEnumerable<DonationDto>>().Result;
                ViewModel.donations = SelectedDonations;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <returns>Individual Donation found by its id if response is successfull -
        /// otherwise redirects to error page</returns>
        // GET: Donation/Details/1
        [HttpGet]
        public ActionResult Details(int id)
        {
            ShowDonation ViewModel = new ShowDonation();
            ViewModel.isadmin = User.IsInRole("Admin");
            string url = "DonationData/FindDonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                DonationDto SelectedDonations = response.Content.ReadAsAsync<DonationDto>().Result;
                ViewModel.Donation = SelectedDonations;

                //Find the Event for Project by Id
                url = "EventData/FindEventForDonation/" + id;
                response = client.GetAsync(url).Result;
                Debug.WriteLine(response.StatusCode);
                EventDto SelectedEvent = response.Content.ReadAsAsync<EventDto>().Result;
                ViewModel.events = SelectedEvent;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <returns>Retrieves Data</returns>
        // GET: Donation/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            UpdateDonation ViewModel = new UpdateDonation();
            string url = "EventData/GetCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<EventDto> PotetnialCategories = response.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;
            ViewModel.Allevents = PotetnialCategories;
            return View(ViewModel);
        }
        /// <returns>Seralizes the inputs and Adds</returns>
        // POST: Donation/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Donation DonationInfo)
        {
            //pass along authentication credential in http request
            GetApplicationCookie();

            //Debug.WriteLine(DonationInfo.DonationQuestion);
            string url = "DonationData/AddDonation";
            //Debug.WriteLine(jss.Serialize(DonationInfo));
            HttpContent content = new StringContent(jss.Serialize(DonationInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", "Donation");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <returns>Retrieves Data</returns>
        // GET: Donation/Edit/1
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            UpdateDonation ViewModel = new UpdateDonation();
            string url = "DonationData/FindDonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                DonationDto SelectedDonations = response.Content.ReadAsAsync<DonationDto>().Result;
                ViewModel.Donation = SelectedDonations;

                url = "EventData/GetCategories";
                response = client.GetAsync(url).Result;
                IEnumerable<EventDto> DonationsEvent = response.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;
                ViewModel.Allevents = DonationsEvent;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <returns>Seralizes the inputs and Updates</returns>
        // POST: Donation/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Donation DonationInfo)
        {
            //pass along authentication credential in http request
            GetApplicationCookie();

            Debug.WriteLine(DonationInfo.FistName);
            string url = "DonationData/UpdateDonation/" + id;
            Debug.WriteLine(jss.Serialize(DonationInfo));
            HttpContent content = new StringContent(jss.Serialize(DonationInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <returns>Retrieves Data</returns>
        // GET: Donation/DeleteConfim/1
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "DonationData/FindDonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                DonationDto SelectedDonations = response.Content.ReadAsAsync<DonationDto>().Result;
                return View(SelectedDonations);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <returns>Deletes the Donation by DonationID</returns>
        // POST: Donation/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            //pass along authentication credential in http request
            GetApplicationCookie();

            string url = "DonationData/DeleteDonation/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}
