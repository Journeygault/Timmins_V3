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
//The following Using statement allows for access of logged in users credentials
using Microsoft.AspNet.Identity;


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
                AllowAutoRedirect = false,
                UseCookies = false

            };
            client = new HttpClient(handler);
            //change this to match your own local port number
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

            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }
        public ActionResult List()
        {
            ListNewsItems ViewModel = new ListNewsItems();
            ViewModel.isadmin = User.IsInRole("Admin"); 

            string url = "NewsItemData/GetNewsItems";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<NewsItemDto> SelectedEvents = response.Content.ReadAsAsync<IEnumerable<NewsItemDto>>().Result;
                ViewModel.newsItems = SelectedEvents;

                return View(ViewModel);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Details(int id)
        {
            ShowNewsItem ViewModel = new ShowNewsItem();
            string url = "NewsItemData/FindNewsItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Hop data transfer object
                NewsItemDto SelectedNewsItem = response.Content.ReadAsAsync<NewsItemDto>().Result;
                ViewModel.newsItem = SelectedNewsItem;


                /*url = "hopdata/findHopClassificationforHop/" + id;
                response = client.GetAsync(url).Result;
                HopClassificationDto SelectedHopClassification = response.Content.ReadAsAsync<HopClassificationDto>().Result;
                ViewModel.hopClassification = SelectedHopClassification;*/

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hop/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]

        public ActionResult Create(NewsItem NewsItemInfo)
        {
            GetApplicationCookie();
            //Debug.WriteLine(NewsItem.NewsItemID);
            string url = "NewsItemData/AddNewsItem";//CHANGE
            NewsItemInfo.UserID = User.Identity.GetUserId();

            Debug.WriteLine(jss.Serialize(NewsItemInfo));
            HttpContent content = new StringContent(jss.Serialize(NewsItemInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    int newsItemid = response.Content.ReadAsAsync<int>().Result;
                    return RedirectToAction("Details", new
                    {
                        id = newsItemid
                    });

                }
                catch (Exception e)
                {
                    Debug.WriteLine(NewsItemInfo);
                    return RedirectToAction("List");
                }
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult DeleteConfirm(int id)
        {
            string url = "NewsItemData/FindNewsItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Hop data transfer object
                NewsItemDto SelectedNewsItem = response.Content.ReadAsAsync<NewsItemDto>().Result;
                return View(SelectedNewsItem);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// Same as aboves security measures
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Hop/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "NewsItemData/DeleteNewsItem/" + id;
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
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int id)
        {
            UpdateNewsItem ViewModel = new UpdateNewsItem();

            string url = "NewsItemData/FindNewsItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                //Put data into Hop data transfer object
                NewsItemDto SelectedNewsItem = response.Content.ReadAsAsync<NewsItemDto>().Result;
                ViewModel.newsItem = SelectedNewsItem;
                //The following is for a forign key
                /*url = "HopClassificationdata/getHopClassifications";
                response = client.GetAsync(url).Result;
                IEnumerable<HopClassificationDto> PotentialHops = response.Content.ReadAsAsync<IEnumerable<HopClassificationDto>>().Result;
                ViewModel.allhopclassifications = PotentialHops;
                */
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        ///<results>Edits a specific hop</results>


        /// <summary>
        /// The following is for security reasons as i understand it,
        /// The validantiforgerytoken helps to protect the database, though 
        /// the specifics of how it acomplishes that is beond me
        /// </summary>
        /// <param name="id">specific hop id</param>
        /// <param name="HopInfo">The information on a hop</param>
        /// <returns></returns>
        // POST: Hop/Edit/5
        [Authorize(Roles = "Admin")]

        [HttpPost]
        [ValidateAntiForgeryToken()]

        public ActionResult Edit(int id, NewsItem NewsItemInfo, HttpPostedFileBase NewsItemImage)
        {
            GetApplicationCookie();

            string url = "NewsItemData/UpdateNewsItem/" + id;
            NewsItemInfo.UserID = User.Identity.GetUserId();

            Debug.WriteLine(jss.Serialize(NewsItemInfo));
            HttpContent content = new StringContent(jss.Serialize(NewsItemInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {///UPDATE BELLOW
                if (NewsItemImage != null)
                {
                    Debug.WriteLine("Calling Update Image method.");
                    //Send over image data for player
                    url = "NewsItemData/UpdateNewsItemImage/" + id;
                    //Debug.WriteLine("Received player picture "+PlayerPic.FileName);

                    MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                    HttpContent imagecontent = new StreamContent(NewsItemImage.InputStream);
                    requestcontent.Add(imagecontent, "NewsItemImage", NewsItemImage.FileName);
                    response = client.PostAsync(url, requestcontent).Result;
                }
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// Error message
        /// </summary>
        /// <returns>Returns Error messages</returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}
