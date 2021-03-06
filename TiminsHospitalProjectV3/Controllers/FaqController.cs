
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
    public class FaqController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private string id;
        private static readonly HttpClient client;

        static FaqController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("http://hospitalproject-env.eba-fm6cqgtc.us-east-2.elasticbeanstalk.com/api/");
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
        /// <returns>All Faqs in the database if response is successfull -
        /// otherwise redirects to error page</returns>
        // GET: Faq/List/{FaqSearchKey?)
        public ActionResult List(string FaqSearchKey = null)
        {
            ListFaqs ViewModel = new ListFaqs();
            ViewModel.isadmin = User.IsInRole("Admin");

            string url = "FaqData/ListFaqs/" + FaqSearchKey;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<FaqDto> SelectedFaqs = response.Content.ReadAsAsync<IEnumerable<FaqDto>>().Result;
                ViewModel.faqs = SelectedFaqs;

                //Find the Category for Project by Id
                url = "CategoryData/GetCategories";
                response = client.GetAsync(url).Result;
                Debug.WriteLine(response.StatusCode);
                IEnumerable<CategoryDto> SelectedCategory = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
                ViewModel.categories = SelectedCategory;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <returns>Individual Faq found by its id if response is successfull -
        /// otherwise redirects to error page</returns>
        // GET: Faq/Details/1
        [HttpGet]
        public ActionResult Details(int id)
        {
            ShowFaq ViewModel = new ShowFaq();
            ViewModel.isadmin = User.IsInRole("Admin");
            string url = "FaqData/FindFaq/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                FaqDto SelectedFaqs = response.Content.ReadAsAsync<FaqDto>().Result;
                ViewModel.Faq = SelectedFaqs;

                //Find the Category for Project by Id
                url = "CategoryData/FindCategoryForFaq/" + id;
                response = client.GetAsync(url).Result;
                Debug.WriteLine(response.StatusCode);
                CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
                ViewModel.Categories = SelectedCategory;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <returns>Retrieves Data</returns>
        // GET: Faq/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            UpdateFaq ViewModel = new UpdateFaq();
            string url = "CategoryData/GetCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CategoryDto> PotetnialCategories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.Allcategories = PotetnialCategories;
            return View(ViewModel);
        }
        /// <returns>Seralizes the inputs and Adds</returns>
        // POST: Faq/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Faq faqInfo)
        {
            //pass along authentication credential in http request
            GetApplicationCookie();

            //Debug.WriteLine(faqInfo.FaqQuestion);
            string url = "FaqData/AddFaq";
            //Debug.WriteLine(jss.Serialize(faqInfo));
            HttpContent content = new StringContent(jss.Serialize(faqInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", "Faq");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <returns>Retrieves Data</returns>
        // GET: Faq/Edit/1
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            UpdateFaq ViewModel = new UpdateFaq();
            string url = "FaqData/FindFaq/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                FaqDto SelectedFaqs = response.Content.ReadAsAsync<FaqDto>().Result;
                ViewModel.Faq = SelectedFaqs;

                url = "CategoryData/GetCategories";
                response = client.GetAsync(url).Result;
                IEnumerable<CategoryDto> FaqsCategory = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
                ViewModel.Allcategories = FaqsCategory;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <returns>Seralizes the inputs and Updates</returns>
        // POST: Faq/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Faq faqInfo)
        {
            //pass along authentication credential in http request
            GetApplicationCookie();

            Debug.WriteLine(faqInfo.FaqQuestion);
            string url = "FaqData/UpdateFaq/" + id;
            Debug.WriteLine(jss.Serialize(faqInfo));
            HttpContent content = new StringContent(jss.Serialize(faqInfo));
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
        // GET: Faq/DeleteConfim/1
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FaqData/FindFaq/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                FaqDto SelectedFaqs = response.Content.ReadAsAsync<FaqDto>().Result;
                return View(SelectedFaqs);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <returns>Deletes the FAQ by FaqID</returns>
        // POST: Faq/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            //pass along authentication credential in http request
            GetApplicationCookie();

            string url = "FaqData/DeleteFaq/" + id;
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
