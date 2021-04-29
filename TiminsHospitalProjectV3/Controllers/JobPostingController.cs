﻿using TiminsHospitalProjectV3.Models;
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
    public class JobPostingController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

         /// <summary>
        /// This allows us to access a pre-defined C# HttpClient 'client' variable for sending POST and GET requests to the data access layer.
        /// </summary>
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
            UpdateJobPosting ViewModel = new UpdateJobPosting();
            string url = "DepartmentData/GetDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewModel.AllDepartments = departments;
            return View(ViewModel);
        }

        // POST: JobPosting/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(JobPosting PostInfo)
        {
            try
            {
                Debug.WriteLine(PostInfo.JobTitle);
                string url = "jobpostingdata/addjobpost";
                Debug.WriteLine(jss.Serialize(PostInfo));
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
 

        // GET: JobPosting/List
        public ActionResult List()
        {
            // Grab all players
            string url = "jobpostingdata/getjobposts";
            // Send off an HTTP request
            // GET : /api/jobpostingdata/getjobposts
            // Retrieve response
            HttpResponseMessage response = client.GetAsync(url).Result;
            // If the response is a success, proceed
            if (response.IsSuccessStatusCode)
            {
                // Fetch the response content into IEnumerable<JobPostingDto>
                IEnumerable<JobPostingDto> SelectedPost = response.Content.ReadAsAsync<IEnumerable<JobPostingDto>>().Result;
                return View(SelectedPost);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

  

        // GET: Jobposting/Details/5
        public ActionResult Details(int id)
        {
            ShowJobPosting ViewModel = new ShowJobPosting();
            string url = "jobpostingdata/findpost/" + id;
         
            HttpResponseMessage response = client.GetAsync(url).Result;
            // If the response is a success, proceed
            if (response.IsSuccessStatusCode)
            {
                // Fetch the response content into IEnumerable<JobPostingDto>
                JobPostingDto SelectedPost = response.Content.ReadAsAsync<JobPostingDto>().Result;

                ViewModel.JobPosting = SelectedPost;
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Jobposting/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            UpdateJobPosting ViewModel = new UpdateJobPosting();

            string url = "jobpostingdata/findpost/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into jobposting data transfer object
              
                JobPostingDto SelectedJobPostings = response.Content.ReadAsAsync<JobPostingDto>().Result;
                ViewModel.JobPosting = SelectedJobPostings;
                 url = "DepartmentData/GetDepartments";
                 response = client.GetAsync(url).Result;
                IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
                ViewModel.AllDepartments = departments;
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }



        // POST: Jobposting/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, JobPosting JobInfo)
        {
            string url = "jobpostingdata/updatepost/" + id;
            Debug.WriteLine(jss.Serialize(JobInfo));
            Debug.WriteLine(JobInfo.JobId);

            HttpContent content = new StringContent(jss.Serialize(JobInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(jss.Serialize(JobInfo));

              if (response.IsSuccessStatusCode)
              {
            return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Jobposting/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "jobpostingdata/findpost/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Jobposting data transfer object
                JobPostingDto SelectedPost = response.Content.ReadAsAsync<JobPostingDto>().Result;
                return View(SelectedPost);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Jobposting/Delete/5
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