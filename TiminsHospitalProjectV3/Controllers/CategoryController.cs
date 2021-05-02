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
    public class CategoryController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static CategoryController()
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
        /// <return>---</return>
        // GET: Category/List
        public ActionResult List(string FaqSearchKey = null)
        {
            ListCategories ViewModel = new ListCategories();
            ViewModel.isadmin = User.IsInRole("Admin");

            string url = "CategoryData/GetCategories" + FaqSearchKey;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<CategoryDto> SelectedCategories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
                ViewModel.categories = SelectedCategories;

                //Get All Faqs for specific Category
                url = "CategoryData/GetAllFaqForCategory";
                response = client.GetAsync(url).Result;
                //Debug.WriteLine(response.StatusCode);
                IEnumerable<FaqDto> SelectedFaqs = response.Content.ReadAsAsync<IEnumerable<FaqDto>>().Result;
                ViewModel.CategoryFaqs = SelectedFaqs;//Seen in the ViewModel Folder

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <return>---</return>
        // GET: Category/Details/1
        [HttpGet]
        public ActionResult Details(int id)
        {
            ShowCategory ViewModel = new ShowCategory();
            ViewModel.isadmin = User.IsInRole("Admin");

            string url = "CategoryData/FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Category Data Transfer Object
                CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
                ViewModel.Category = SelectedCategory;//Seen in the ViewModel Folder

                //Get All Faqs for specific Category
                url = "CategoryData/GetFaqsForCategory/" + id;
                response = client.GetAsync(url).Result;
                //Debug.WriteLine(response.StatusCode);
                IEnumerable<FaqDto> SelectedFaqs = response.Content.ReadAsAsync<IEnumerable<FaqDto>>().Result;
                ViewModel.CategoryFaqs = SelectedFaqs;//Seen in the ViewModel Folder

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <return>---</return>
        // GET: Category/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        /// <return>---</return>
        // POST: Category/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Category CategoryInfo)
        {
            //pass along authentication credential in http request
            GetApplicationCookie();

            Debug.WriteLine(CategoryInfo.CategoryName);
            string url = "CategoryData/AddCategory";
            HttpContent content = new StringContent(jss.Serialize(CategoryInfo));
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
        /// <return>---</return>
        // GET: Category/Edit/1
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string url = "CategoryData/FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Category data transfer object
                CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
                return View(SelectedCategory);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <return>---</return>
        // POST: Category/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Category CategoryInfo)
        {
            //pass along authentication credential in http request
            GetApplicationCookie();

            Debug.WriteLine(CategoryInfo.CategoryName);
            string url = "CategoryData/UpdateCategory/" + id;
            Debug.WriteLine(jss.Serialize(CategoryInfo));
            HttpContent content = new StringContent(jss.Serialize(CategoryInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { Id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <return>---</return>
        // GET: Category/DeleteConfirm/1
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "CategoryData/FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
                return View(SelectedCategory);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <return>---</return>
        // POST: Category/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            //pass along authentication credential in http request
            GetApplicationCookie();

            string url = "CategoryData/DeleteCategory/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", "Faq");
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
