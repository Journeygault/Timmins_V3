using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using TiminsHospitalProjectV3.Models;
//using TiminsHospitalProjectV3.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace TiminsHospitalProjectV3.Controllers
{
    public class NewsItemController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static NewsItemController()
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
        public ActionResult List()
        {
            string url = "NewsItemData/GetNewsItemss";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<NewsItemDto> SelectedHops = response.Content.ReadAsAsync<IEnumerable<NewsItemDto>>().Result;
                return View(SelectedHops);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
