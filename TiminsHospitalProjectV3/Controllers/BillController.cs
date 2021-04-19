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

        
    }
}
