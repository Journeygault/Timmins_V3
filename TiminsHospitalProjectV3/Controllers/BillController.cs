using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TiminsHospitalProjectV3.Models.ViewModels;
using TiminsHospitalProjectV3.Models;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Net.Http;

namespace TiminsHospitalProjectV3.Controllers
{
    public class BillController : Controller
    {
        //GET : Bill
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private string id;
        private static readonly HttpClient client;

        static BillController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,

            };
            client = new HttpClient(handler);
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

            //collect token
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        /// <summary>
        /// Gets a list of the available bills in the database
        /// </summary>
        /// <returns>Returns a list of the bills in the database</returns>
        // GET: Bill/List
        public ActionResult List()
        {
            string url = "billdata/ListBills";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Bill> SelectedBills = response.Content.ReadAsAsync<IEnumerable<Bill>>().Result;
                return View(SelectedBills);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Shows the details of a bill given an id
        /// </summary>
        /// <param name="id">1</param>
        /// <returns>Bill details with an ID of 1. Including amount, date issued, and bill breakdown</returns>
        public ActionResult Details(int id)
        {
            ShowBill ViewModel = new ShowBill();
            //Show only if the user is in an admin role, can change for user role
            ViewModel.isadmin = User.IsInRole("Admin");
            string url = "BillData/FindBill/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                BillDto SelectedBill = response.Content.ReadAsAsync<BillDto>().Result;
                ViewModel.Bill = SelectedBill;

                return View(ViewModel);

            }
            else
            {
                return RedirectToAction("Error");
            }

        }
        //GET: Bill/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Allows the admin to create a bill utilizing the method created in BillData
        /// </summary>
        /// <param name="BillInfo"></param>
        /// <returns>A newly created bill inserted into the database, appearing on a specific user account dependent on user id associated with bill.</returns>
        //POST : Bill/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Bill BillInfo)
        {
            string url = "billdata/createbill";
            HttpContent content = new StringContent(jss.Serialize(BillInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", "Bill");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm (int id)
        {
            string url = "Billdata/FindBill/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                BillDto SelectedBill = response.Content.ReadAsAsync<BillDto>().Result;
                return View(SelectedBill);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Allows an admin to \delete a bill in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An updated list of the current bills in the database</returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize (Roles = "Admin")]
        public ActionResult Delete (int id)
        {
            string url = "billdata/deletebill/" + id;
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

        /// <summary>
        /// Allows an admin to edit / update an existing bill in the database
        /// </summary>
        /// <param name="id">1</param>
        /// <param name="BillInfo">1</param>
        /// <returns>Returns an updated bill matching bill ID and userID</returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize (Roles = "Admin")]
        public ActionResult Edit(int id, Bill BillInfo)
        {
            string url = "billdata/updatebill/" + id;
            HttpContent content = new StringContent(jss.Serialize(BillInfo));
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
    }
}
