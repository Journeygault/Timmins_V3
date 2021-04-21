﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TiminsHospitalProjectV3.Models;
using System.Web.Script.Serialization;
using TiminsHospitalProjectV3.Models.ViewModels;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Net.Http;

namespace TiminsHospitalProjectV3.Controllers
{
    public class ReviewController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        static ReviewController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44346/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }


        // GET: Review
        public ActionResult List()
        {
            string url = "ReviewData/ListReviews";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Review> SelectedReviews = response.Content.ReadAsAsync<IEnumerable<Review>>().Result;
                return View(SelectedReviews);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// To show the details of a review including user ID, first name, content
        /// </summary>
        /// <param name="id"></param>
        /// <returns>gives a view of the information associated with each review</returns>
        // GET: Review/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            ShowReview ViewModel = new ShowReview();

            string url = "ReviewData/FindReview/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                Review SelectedReview = response.Content.ReadAsAsync<Review>().Result;
                ViewModel.Review = SelectedReview;

                return View(ViewModel);
            }

            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Review/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// This will allow the user to create a new review
        /// </summary>
        /// <param name="NewReview"></param>
        /// <returns>A view of the new review that was created</returns>
        // POST: Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Review NewReview)
        {
            string url = "ReviewData/AddReview";
            Debug.WriteLine(jss.Serialize(NewReview));
            HttpContent content = new StringContent(jss.Serialize(NewReview));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    int reviewId = response.Content.ReadAsAsync<int>().Result;
                    return RedirectToAction("Details", new
                    {
                        id = reviewId
                    });
                }
                catch (Exception e)
                {
                    Debug.WriteLine(NewReview);
                    return RedirectToAction("List");
                }
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Review/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateReview ViewModel = new UpdateReview();

            string url = "ReviewData/FindReview" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                ReviewDto SelectedREview = response.Content.ReadAsAsync<ReviewDto>().Result;
                ViewModel.Review = SelectedREview;
                return View(ViewModel);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Review/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Review ReviewInfo)
        {
            string url = "ReviewData/UpdateReview/" + id;
            HttpContent content = new StringContent(jss.Serialize(ReviewInfo));
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

        // GET: Review/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ReviewData/FindReview/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                Review SelectedReview = response.Content.ReadAsAsync<Review>().Result;
                return View(SelectedReview);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Review/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "ReviewData/DeleteReview/" + id;
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
    }
}