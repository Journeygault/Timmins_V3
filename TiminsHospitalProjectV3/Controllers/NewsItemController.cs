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
            string url = "NewsItemData/GetNewsItems";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<NewsItemDto> SelectedNewsItems = response.Content.ReadAsAsync<IEnumerable<NewsItemDto>>().Result;
                return View(SelectedNewsItems);
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
        public ActionResult Create(NewsItem NewsItemInfo)
        {
            //Debug.WriteLine(NewsItem.NewsItemID);
            string url = "NewsItemData/AddNewsItem";//CHANGE
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
        public ActionResult Delete(int id)
        {
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
        [HttpPost]
        [ValidateAntiForgeryToken()]

        public ActionResult Edit(int id, NewsItem NewsItemInfo)
        {

            string url = "NewsItemData/UpdateNewsItem/" + id;
            Debug.WriteLine(jss.Serialize(NewsItemInfo));
            HttpContent content = new StringContent(jss.Serialize(NewsItemInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

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
