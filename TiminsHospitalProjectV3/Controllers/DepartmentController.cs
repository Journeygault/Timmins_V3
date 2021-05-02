using TiminsHospitalProjectV3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TiminsHospitalProjectV3.Models.ViewModels;

namespace TiminsHospitalProjectV3.Controllers
{
    public class DepartmentController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        /// <summary>
        /// This allows us to access a pre-defined C# HttpClient 'client' variable for sending POST and GET requests to the data access layer.
        /// </summary>
        static DepartmentController()
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

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        //Allows admin to create a department
        public ActionResult Create(Department DepartmentInfo)
        {
            Debug.WriteLine(DepartmentInfo.DepartmentName);
            string url = "DepartmentData/AddDepartment";
            Debug.WriteLine(jss.Serialize(DepartmentInfo));
            HttpContent content = new StringContent(jss.Serialize(DepartmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int departmentId = response.Content.ReadAsAsync<int>().Result;

               return RedirectToAction("List");

            }
            else
            {
                return RedirectToAction("Error");
            }


        }


        // GET: Department/Details/5
        //Allows admin to see a department's details along with the associated job postings
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            ShowDepartment showdepartment = new ShowDepartment();

            string url = "departmentdata/finddepartment/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            // If the response is a success, proceed
            if (response.IsSuccessStatusCode)
            {
                // Fetch the response content into IEnumerable<DepartmentDto>

                DepartmentDto department = response.Content.ReadAsAsync<DepartmentDto>().Result;
                showdepartment.Department = department;

                url = "DepartmentData/FindJobPostingsForDepartment/" + id;
                response = client.GetAsync(url).Result;
                IEnumerable<JobPostingDto> jobPostings = response.Content.ReadAsAsync<IEnumerable<JobPostingDto>>().Result;
                showdepartment.JobPostings = jobPostings;
                return View(showdepartment);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Department/List
        [Authorize(Roles = "Admin")]
        //Allows admin to see a list of department
        public ActionResult List()
        {
            // Grab all departments
            string url = "departmentdata/getdepartments";
            // Send off an HTTP request
            // GET : /api/departmentdata/getdepartments
            // Retrieve response
            HttpResponseMessage response = client.GetAsync(url).Result;
            // If the response is a success, proceed
            if (response.IsSuccessStatusCode)
            {
                // Fetch the response content into IEnumerable<DepartmentDto>
                IEnumerable<DepartmentDto> SelectedDepartment = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
                return View(SelectedDepartment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Department/Create
        // only administrators get to this page
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }



        // GET: Department/Edit/5
        // only administrators get to this page
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into department data transfer object
                DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                return View(SelectedDepartment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Department/Edit/5
        //Allows admin to edit a department
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Department DepartmentInfo)
        {
            string url = "departmentdata/updatedepartment/" + id;
            Debug.WriteLine(jss.Serialize(DepartmentInfo));
            HttpContent content = new StringContent(jss.Serialize(DepartmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Department/Delete/5
        //admin can confirm the deletion
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into department data transfer object
                DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                return View(SelectedDepartment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        //Allows admin to delete a department
        public ActionResult Delete(int id)
        {
            string url = "departmentdata/deletedepartment/" + id;
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

        //Redirects to the error view
        public ActionResult Error()
        {
            return View();
        }
    }
}
