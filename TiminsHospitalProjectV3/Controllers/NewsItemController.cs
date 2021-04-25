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
            //The following defines how cookies will be handled
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //This allows us to use cookies to check admin status of logged in user
                UseCookies = false

            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44346/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));



        }
        //This method actually gets the cookies
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
        // Allows us to acces the get news items
        public ActionResult List()
        {
            ListNewsItems ViewModel = new ListNewsItems();
            ViewModel.isadmin = User.IsInRole("Admin"); //Checks to see if the user is an admin

            string url = "NewsItemData/GetNewsItems";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                //Produces a lsit of news items through the DTO
                IEnumerable<NewsItemDto> SelectedEvents = response.Content.ReadAsAsync<IEnumerable<NewsItemDto>>().Result;
                ViewModel.newsItems = SelectedEvents;

                return View(ViewModel);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //Gets the specified details of an individual news item
        public ActionResult Details(int id)
        {
            //instantiates the ShowNEwsITem viewmodle
            ShowNewsItem ViewModel = new ShowNewsItem();
            string url = "NewsItemData/FindNewsItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
               //Finds the info for one specific news item through the dto
                NewsItemDto SelectedNewsItem = response.Content.ReadAsAsync<NewsItemDto>().Result;
                ViewModel.newsItem = SelectedNewsItem;



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

    
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]

        public ActionResult Create(NewsItem NewsItemInfo)
        {
            //Gets the cookie to check for admin
            GetApplicationCookie();
            string url = "NewsItemData/AddNewsItem";
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
        //finds the news item to be deleted
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult DeleteConfirm(int id)
        {
            string url = "NewsItemData/FindNewsItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
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

        //Confirms and actually deletes the specific news item
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id)
        {
            //gets the cookie to check for admin
            GetApplicationCookie();
            string url = "NewsItemData/DeleteNewsItem/" + id;
       
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
 

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
        //allows the view to find the specific news item to be edited
        public ActionResult Edit(int id)
        {
            UpdateNewsItem ViewModel = new UpdateNewsItem();

            string url = "NewsItemData/FindNewsItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                //Put data into newsitem data transfer object
                NewsItemDto SelectedNewsItem = response.Content.ReadAsAsync<NewsItemDto>().Result;
                ViewModel.newsItem = SelectedNewsItem;
             
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

       //Allows the editing of the specified news item
        [Authorize(Roles = "Admin")]

        [HttpPost]
        [ValidateAntiForgeryToken()]

        public ActionResult Edit(int id, NewsItem NewsItemInfo, HttpPostedFileBase NewsItemImage)
        {
            //gets cookie for security
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
                    //Send over image data for the specific news item
                    url = "NewsItemData/UpdateNewsItemImage/" + id;

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
        /// returns an erro in the event of an error
        public ActionResult Error()
        {
            return View();
        }
    }
}
