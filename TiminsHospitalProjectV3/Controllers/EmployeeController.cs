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
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44346/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }
        // GET: Employee/List
        public ActionResult List()
        {
            string url = "employeedata/getemployees";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<EmployeeDto> SelectedEmployees = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
                return View(SelectedEmployees);
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
        public ActionResult Create(Employee EmployeeInfo)
        {
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
        public ActionResult Edit(int id, Employee EmployeeInfo, HttpPostedFileBase EmployeePic)
        {
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
        public ActionResult Delete(int id)
        {
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

