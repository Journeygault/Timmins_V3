using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using TiminsHospitalProjectV3.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using TiminsHospitalProjectV3.Models.ViewModels;
using System.Globalization;

namespace TiminsHospitalProjectV3.Controllers
{
    public class AppointmentsController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        private int perPage = 10;//number of record per page

        /// <summary>
        /// This allows us to access a pre-defined C# HttpClient 'client' variable for sending POST and GET requests to the data access layer.
        /// </summary>
        static AppointmentsController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            //change this to match your own local port number
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
        [Authorize(Roles = "Patient,Physician,Admin")]
        // GET: Appointments/List?{pageNum}
        public ActionResult List(int pageNum = 1, string searchStatus = "all")
        {
            string url = "", paginatedUrl = "";
            //grab all appointments
            if (User.IsInRole("Admin"))
            {
                paginatedUrl = "AppointmentsData/GetAppointmentsPage";
                url = "AppointmentsData/GetAppointments";
            }

            else if (User.IsInRole("Patient") || User.IsInRole("Physician"))
            {
                paginatedUrl = "AppointmentsData/FindUserAppointmentsPage";
                url = "AppointmentsData/FindUserAppointments";
                url = url + "/" + User.Identity.GetUserId();
                paginatedUrl += "/" + User.Identity.GetUserId();
            }


            if (url != "")
            {
                url += "?searchStatus=" + searchStatus;

                //sends http request and retrieves the response
                HttpResponseMessage response = client.GetAsync(url).Result;

                //grab the result if the request is a success
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<Appointment> appointments = response.Content.ReadAsAsync<IEnumerable<Appointment>>().Result;
                    //determines the max number of pages
                    int nberItems = appointments.Count();
                    int maxPageNber = (int)Math.Ceiling((decimal)nberItems / perPage);
                    // Lower boundary for Max Page number
                    if (maxPageNber < 1) maxPageNber = 1;
                    // Lower boundary for Page Number
                    if (pageNum < 1) pageNum = 1;
                    // Upper Bound for Page Number
                    if (pageNum > maxPageNber) pageNum = maxPageNber;
                    // The Record Index of our Page Start
                    int startIndex = perPage * (pageNum - 1);

                    //Helps us generate the HTML which shows "Page 1 of ..." on the list view
                    ViewData["PageNum"] = pageNum;
                    ViewData["MaxPageNum"] = maxPageNber;
                    ViewData["searchStatus"] = searchStatus;

                    //get the list of appoinments according to pagination
                    paginatedUrl += "/" + startIndex + "/" + perPage + "?searchStatus=" + searchStatus; ;
                    response = client.GetAsync(paginatedUrl).Result;
                    appointments = response.Content.ReadAsAsync<IEnumerable<Appointment>>().Result;
                    foreach (Appointment appt in appointments)
                    {
                        appt.PatientUser = new ApplicationDbContext().Users.Find(appt.PatientID);
                        appt.PhysicianUser = new ApplicationDbContext().Users.Find(appt.PhysicianID);
                    }
                    return View(appointments);
                }
                else
                {
                    // If we reach here something went wrong with our list algorithm
                    return RedirectToAction("Error");
                }

            }
            else
            {
                // If we reach here something went wrong with our list algorithm
                return RedirectToAction("Error");
            }


        }

        [Authorize(Roles = "Patient,Physician,Admin")]
        // GET: Appointments/Details/5
        public ActionResult Details(int id)
        {
            string url = "AppointmentsData/GetAppointment/" + id;
            //sends http request and retrieves the response
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Appointment appt = response.Content.ReadAsAsync<Appointment>().Result;
                appt.PatientUser = new ApplicationDbContext().Users.Find(appt.PatientID);
                appt.PhysicianUser = new ApplicationDbContext().Users.Find(appt.PhysicianID);
                ViewData["listStatus"] = Enum.GetValues(typeof(AppointmentStatus));
                return View(appt);
            }
            else
            {
                // If we reach here something went wrong with our list algorithm
                return RedirectToAction("Error");
            }

        }
        [HttpPost]
        [Authorize(Roles = "Physician")]
        [ValidateAntiForgeryToken()]
        // POST: Appointments/ChangeStatus/5/Accepted
        public ActionResult ChangeStatus(int id, AppointmentStatus status)
        {
            string url = "AppointmentsData/GetAppointment/" + id;
            //sends http request and retrieves the response
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Appointment appt = response.Content.ReadAsAsync<Appointment>().Result;
                appt.Status = status;
                appt.DecisionMadeOn = DateTime.Now.ToString("yyyy/MM/dd hh:mm tt");
                //pass along authentication credential in http request
                GetApplicationCookie();

                //update the appointment
                url = "AppointmentsData/UpdateAppointment/" + id;

                HttpContent content = new StringContent(jss.Serialize(appt));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", new { id = id });

                }
                else
                {
                    return RedirectToAction("Error");

                }


            }
            else
            {
                // If we reach here something went wrong with our list algorithm
                return RedirectToAction("Error");
            }

        }



        [Authorize(Roles = "Patient,Physician")]
        // GET: Appointments/Create
        public ActionResult Create()
        {
            IdentityRole role = null;
            //if user's role is 'patient' fetch the users with 'Doctor' role and vice versa
            if (User.IsInRole("Patient"))
            {
                role = new ApplicationDbContext().Roles.SingleOrDefault(m => m.Name == "Physician");

            }

            else
            {
                role = new ApplicationDbContext().Roles.SingleOrDefault(m => m.Name == "Patient");
            }
            CreateViewAppointment viewModel = new CreateViewAppointment();
            viewModel.UsersInRole = new ApplicationDbContext().Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id)).ToList();
            ViewData["user_id"] = User.Identity.GetUserId();

            return View(viewModel);
        }

        // POST: Appointments/Create
        [Authorize(Roles = "Patient,Physician")]
        [ValidateAntiForgeryToken()]
        [HttpPost]
        public ActionResult Create(CreateViewAppointment viewAppointment)
        {          

            if (ModelState.IsValid)
            {
                Appointment newAppointment = new Appointment();
                newAppointment.PatientID = viewAppointment.PatientID;
                newAppointment.PhysicianID = viewAppointment.PhysicianID;
                newAppointment.ID = 0;
                //insert the new appointment
                string url = "AppointmentsData/AddAppointment";

                
                newAppointment.Subject = viewAppointment.Subject;
                newAppointment.Message = viewAppointment.Message;

                newAppointment.SentOn = DateTime.Now.ToString("yyyy/MM/dd hh:mm tt");
                //newAppointment.RequestDatetime = DateTime.ParseExact(newAppointment.RequestDatetime.ToString("yyyy/MM/dd HH:mm"), "yyyy/MM/dd HH:mm", System.Globalization.CultureInfo.CreateSpecificCulture("en-CA"));
                newAppointment.Status = AppointmentStatus.Pending;
                var cultureInfo = new CultureInfo("en-CA");
                DateTime requestedDateTime = DateTime.Parse(viewAppointment.RequestDatetime, cultureInfo);
                newAppointment.RequestDatetime = requestedDateTime.ToString("yyyy/MM/dd hh:mm tt");
                //pass along authentication credential in http request
                GetApplicationCookie();

                HttpContent content = new StringContent(jss.Serialize(newAppointment));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List", new { pageNum = 1 });

                }
                else
                {
                    return RedirectToAction("Error");

                }
            }
            else
            {
                IdentityRole role = null;
                //if user's role is 'patient' fetch the users with 'Doctor' role and vice versa
                if (User.IsInRole("Patient"))
                {
                    role = new ApplicationDbContext().Roles.SingleOrDefault(m => m.Name == "Physician");

                }

                else
                {
                    role = new ApplicationDbContext().Roles.SingleOrDefault(m => m.Name == "Patient");
                }
                CreateViewAppointment viewModel = new CreateViewAppointment();
                viewModel.UsersInRole = new ApplicationDbContext().Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id)).ToList();
                ViewData["user_id"] = User.Identity.GetUserId();

                return View(viewAppointment);
            }
        }

        [Authorize(Roles = "Patient,Physician")]
        // GET: Appointments/Edit/5
        public ActionResult Edit(int id)
        {
            
            string url = "AppointmentsData/GetAppointment/" + id;
            //sends http request and retrieves the response
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                UpdateViewAppointment viewModel = new UpdateViewAppointment();
                Appointment appointment = response.Content.ReadAsAsync<Appointment>().Result;
                var cultureInfo = new CultureInfo("en-CA");
                DateTime requestedDateTime = DateTime.Parse(appointment.RequestDatetime, cultureInfo);
                viewModel.RequestDatetime = requestedDateTime.ToString("yyyy/MM/dd HH:mm ");
                viewModel.PatientUser = new ApplicationDbContext().Users.Find(appointment.PatientID);
                viewModel.PhysicianUser = new ApplicationDbContext().Users.Find(appointment.PhysicianID);
                viewModel.Subject = appointment.Subject;
                viewModel.Message = appointment.Message;
                viewModel.Status = appointment.Status;
                viewModel.SentOn = appointment.SentOn;
                viewModel.PatientID = appointment.PatientID;
                viewModel.PhysicianID = appointment.PhysicianID;
                viewModel.ID = appointment.ID;

                return View(viewModel);

            }
            else
            {
                return RedirectToAction("Error");

            }


        }

        // POST: Appointments/Edit/5
        [Authorize(Roles = "Patient,Physician")]
        [ValidateAntiForgeryToken()]
        [HttpPost]
        public ActionResult Edit(int id, UpdateViewAppointment viewModel)
        {
            if (ModelState.IsValid)
            {
                Appointment appointment = new Appointment();
                appointment.ID = viewModel.ID;
                appointment.PatientID = viewModel.PatientID;
                appointment.PhysicianID = viewModel.PhysicianID;
                appointment.SentOn = viewModel.SentOn;
                appointment.Subject = viewModel.Subject;
                appointment.Message = viewModel.Message;
                appointment.Status = viewModel.Status;
                //pass along authentication credential in http request
                GetApplicationCookie();

                //update the appointment
                string url = "AppointmentsData/UpdateAppointment/" + id;
                var cultureInfo = new CultureInfo("en-CA");
                DateTime requestedDateTime = DateTime.Parse(viewModel.RequestDatetime, cultureInfo);
                appointment.RequestDatetime = requestedDateTime.ToString("yyyy/MM/dd hh:mm tt");

                HttpContent content = new StringContent(jss.Serialize(appointment));
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
            else
            {
                return View(viewModel);
            }
        }

        [Authorize(Roles = "Patient,Physician")]
        // GET: Appointments/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "AppointmentsData/GetAppointment/" + id;
            //sends http request and retrieves the response
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Appointment appointment = response.Content.ReadAsAsync<Appointment>().Result;
                appointment.PatientUser = new ApplicationDbContext().Users.Find(appointment.PatientID);
                appointment.PhysicianUser = new ApplicationDbContext().Users.Find(appointment.PhysicianID);
                if (User.IsInRole("Patient"))
                    ViewData["recipientUsername"] = "Dr "+appointment.PhysicianUser.FirstName + " "+ appointment.PhysicianUser.LastName;
                else
                    ViewData["recipientUsername"] = appointment.PatientUser.FirstName + " "+ appointment.PatientUser.LastName;

                return View(appointment);

            }
            else
            {
                return RedirectToAction("Error");

            }
        }
        [Authorize(Roles = "Patient,Physician")]
        [ValidateAntiForgeryToken()]
        // POST: Appointments/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {

            //update the appointment
            string url = "AppointmentsData/DeleteAppointment/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", new { pageNum = 1 });

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
