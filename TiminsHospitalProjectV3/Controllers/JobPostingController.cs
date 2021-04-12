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

namespace TiminsHospitalProjectV3.Controllers
{
    public class JobPostingController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static JobPostingController()
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

        // GET: JobPosting
        public ActionResult Index()
        {
            return View();
        }

       

        // GET: JobPosting/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobPosting/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(JobPosting PostInfo)
        {
            try
            {
                Debug.WriteLine("hi" + PostInfo.JobTitle);
                string url = "jobpostingdata/addjobpost";
                Debug.WriteLine("hey" + jss.Serialize(PostInfo));
                HttpContent content = new StringContent(jss.Serialize(PostInfo));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    int jobId = response.Content.ReadAsAsync<int>().Result;

                   
                    return RedirectToAction("Details", new { id = jobId });
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            catch
            {
                return View();

            }




        }
 

        // GET: Customers/List?{PageNum}
        public ActionResult List()
        {
            // Grab all players
            string url = "jobpostingdata/getjobposts";
            // Send off an HTTP request
            // GET : /api/playerdata/getplayers
            // Retrieve response
            HttpResponseMessage response = client.GetAsync(url).Result;
            // If the response is a success, proceed
            if (response.IsSuccessStatusCode)
            {
                // Fetch the response content into IEnumerable<PlayerDto>
                IEnumerable<JobPostingDto> SelectedCustomers = response.Content.ReadAsAsync<IEnumerable<JobPostingDto>>().Result;
                return View(SelectedCustomers);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Department/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "jobpostingdata/findpost/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Team data transfer object
                JobPostingDto SelectedPost = response.Content.ReadAsAsync<JobPostingDto>().Result;
                return View(SelectedPost);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Team/Details/5
        public ActionResult Details(int id)
        {
            string url = "jobpostingdata/findpost/" + id;
            // Send off an HTTP request
            // GET : /api/playerdata/getplayers
            // Retrieve response
            HttpResponseMessage response = client.GetAsync(url).Result;
            // If the response is a success, proceed
            if (response.IsSuccessStatusCode)
            {
                // Fetch the response content into IEnumerable<DepartmentDto>
                JobPostingDto SelectedPost = response.Content.ReadAsAsync<JobPostingDto>().Result;
                return View(SelectedPost);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, JobPosting JobInfo)
        {
            string url = "jobpostingdata/updatepost/" + id;
            Debug.WriteLine(jss.Serialize(JobInfo));
            HttpContent content = new StringContent(jss.Serialize(JobInfo));
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

        // GET: Customer/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "jobpostingdata/findpost/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                JobPostingDto SelectedPost = response.Content.ReadAsAsync<JobPostingDto>().Result;
                return View(SelectedPost);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "jobpostingdata/DeleteJobPosting/" + id;
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
