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

namespace hospitalproject.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        static EmployeeController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44346/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

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

        // GET: Employee/List?{PageNum}
        public ActionResult List(int PageNum = 0)
        {
            ListEmployees ViewModel = new ListEmployees();
            ViewModel.isadmin = User.IsInRole("Admin");
            string url = "employeedata/getemployees";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<EmployeeDto> SelectedEmployees = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
                // -- Start of Pagination Algorithm --

                // Find the total number of job applications
                int EmployeeCount = SelectedEmployees.Count();
                // Number of jobhunters to display per page
                int PerPage = 12;
                // Determines the maximum number of pages (rounded up), assuming a page 0 start.
                int MaxPage = (int)Math.Ceiling((decimal)EmployeeCount / PerPage) - 1;

                // Lower boundary for Max Page
                if (MaxPage < 0) MaxPage = 0;
                // Lower boundary for Page Number
                if (PageNum < 0) PageNum = 0;
                // Upper Bound for Page Number
                if (PageNum > MaxPage) PageNum = MaxPage;

                // The Record Index of our Page Start
                int StartIndex = PerPage * PageNum;

                //Helps us generate the HTML which shows "Page 1 of ..." on the list view
                ViewData["PageNum"] = PageNum;
                ViewData["PageSummary"] = " " + (PageNum + 1) + " of " + (MaxPage + 1) + " ";

                // -- End of Pagination Algorithm --


                // Send back another request to get job hunters, this time according to our paginated logic rules
                // GET api/employeedata/getemployeespage/{startindex}/{perpage}
                url = "employeedata/getemployeespage/" + StartIndex + "/" + PerPage;
                response = client.GetAsync(url).Result;

                // Retrieve the response of the HTTP Request
                IEnumerable<EmployeeDto> SelectedEmployeesPage = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
                ViewModel.employees = SelectedEmployeesPage;
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            ShowEmployee ViewModel = new ShowEmployee();
            ViewModel.isadmin = User.IsInRole("Admin");
            string url = "employeedata/findemployee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into job posting data transfer object
                EmployeeDto SelectedEmployee = response.Content.ReadAsAsync<EmployeeDto>().Result;
                ViewModel.employee = SelectedEmployee;


                url = "employeedata/findjob_postingforemployee/" + id;
                response = client.GetAsync(url).Result;
                Job_PostingDto SelectedJob_Posting = response.Content.ReadAsAsync<Job_PostingDto>().Result;
                ViewModel.job_posting = SelectedJob_Posting;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        // GET: Employee/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Employee EmployeeInfo)
        {
            GetApplicationCookie();
            string url = "employeedata/addemployee";
            Debug.WriteLine(jss.Serialize(EmployeeInfo));
            HttpContent content = new StringContent(jss.Serialize(EmployeeInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                int employeeid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = employeeid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        // GET: Employee/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            UpdateEmployee ViewModel = new UpdateEmployee();

            string url = "employeedata/findemployee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into job hunters data transfer object
                EmployeeDto SelectedEmployee = response.Content.ReadAsAsync<EmployeeDto>().Result;
                ViewModel.employee = SelectedEmployee;

                //get information about job postings this job hunters belong to.
                //url = "job_postingdata/getjob_postings";
                //response = client.GetAsync(url).Result;
                //IEnumerable<Job_PostingDto> PotentialJob_Postings = response.Content.ReadAsAsync<IEnumerable<Job_PostingDto>>().Result;
                //ViewModel.alljob_postings = PotentialJob_Postings;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Employee EmployeeInfo, HttpPostedFileBase EmployeePic)
        {
            GetApplicationCookie();
            string url = "employeedata/updateemployee/" + id;
            Debug.WriteLine(jss.Serialize(EmployeeInfo));
            HttpContent content = new StringContent(jss.Serialize(EmployeeInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                if (EmployeePic != null)
                {
                    Debug.WriteLine("Calling Update Image method.");
                    //Send over image data for job hunter
                    url = "employeedata/updateemployeepic/" + id;


                    MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                    HttpContent imagecontent = new StreamContent(EmployeePic.InputStream);
                    requestcontent.Add(imagecontent, "EmployeePic", EmployeePic.FileName);
                    response = client.PostAsync(url, requestcontent).Result;
                }

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        // GET: Employee/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "employeedata/findemployee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into job hunters data transfer object
                EmployeeDto SelectedEmployee = response.Content.ReadAsAsync<EmployeeDto>().Result;
                return View(SelectedEmployee);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "employeedata/deleteemployee/" + id;
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

        public ActionResult Error()
        {
            return View();
        }
    }
}

