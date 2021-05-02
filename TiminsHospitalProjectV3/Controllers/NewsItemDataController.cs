using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http.Description;
using TiminsHospitalProjectV3.Models;
using System.Diagnostics;
using System.IO;
using System.Web;



//Port Number:44346
namespace TiminsHospitalProjectV3.Controllers
{
    public class NewsItemDataController : ApiController
    {
       
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: api/NewsItemData/GetNewsItems
        /// <summary>
        /// THe following gets a list of all NewsItems
        /// </summary>
        /// <returns> A list of all news Items</returns>
        [ResponseType(typeof(IEnumerable<NewsItemDto>))]
        public IHttpActionResult GetNewsItems()
        {
            List<NewsItem> NewsItems = db.NewsItems.ToList();
            List<NewsItemDto> NewsItemDtos = new List<NewsItemDto> { };

            foreach (var NewsItem in NewsItems)
            {
                NewsItemDto NewNewsItem = new NewsItemDto
                {
                    NewsItemID = NewsItem.NewsItemID,
                    Title = NewsItem.Title,
                    NewsBody = NewsItem.NewsBody,
                    NewItemDate = NewsItem.NewItemDate,
                    NewsItemHasPic = NewsItem.NewsItemHasPic,
                    NewsItemPicExtension = NewsItem.NewsItemPicExtension,
                    UserID = NewsItem.UserID
                };
                NewsItemDtos.Add(NewNewsItem);
            }

            return Ok(NewsItemDtos);
        }
        /// <summary>
        /// Finds the information for one specific news item
        /// </summary>
        /// <param name="id">The ID of the desired news item</param>
        /// <returns> All the information for a specific new item</returns>
        [HttpGet]
        // GET: api/NewsItemData/FindNewsItem/id

        [ResponseType(typeof(NewsItemDto))]

        public IHttpActionResult FindNewsItem(int id)
        {
            //Find the data
            NewsItem NewsItem = db.NewsItems.Find(id);
            //if not found, return 404 status code.
            if (NewsItem == null)
            {
                return NotFound();
            }

            //An easy to access entry into the Hop information, safely through the DTO
            NewsItemDto NewsItemDto = new NewsItemDto
            {
                NewsItemID = NewsItem.NewsItemID,
                Title = NewsItem.Title,
                NewsBody = NewsItem.NewsBody,
                NewItemDate = NewsItem.NewItemDate,
                NewsItemHasPic = NewsItem.NewsItemHasPic,
                NewsItemPicExtension = NewsItem.NewsItemPicExtension,
                UserID = NewsItem.UserID
            };


            //pass along data as 200 status code OK response
            return Ok(NewsItemDto);
        }
        /// <summary>
        /// Allows the admin to create a new NewsItem
        /// </summary>
        /// <param name="newsItem">The News Item info being created</param>
        /// <returns> a new NEws ITem</returns>
        [ResponseType(typeof(NewsItem))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddNewsItem([FromBody] NewsItem newsItem)
        {
            //Just checks to see if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NewsItems.Add(newsItem);
            db.SaveChanges();

            return Ok(newsItem.NewsItemID); ;
        }
        // GET: api/NewsItemData/DeleteNewsItem
        /// <summary>
        /// Allows the Admin to delete a specific news item
        /// </summary>
        /// <param name="id">The ID of the specific news item being deleted</param>
        /// <returns> Returns OK but dosent really return anything other than having 1 less item</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteNewsItem(int id)
        {

            NewsItem newsItem = db.NewsItems.Find(id);
            if (newsItem == null)
            {
                return NotFound();
            }
            Debug.WriteLine(newsItem);

            db.NewsItems.Remove(newsItem);
            db.SaveChanges();

            return Ok();
        }
        /// <summary>
        /// Allows for the updating of one newsitem
        /// </summary>
        /// <param name="id">The specific news item ID</param>
        /// <param name="newsItem">Represents the information being added</param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateNewsItem(int id, [FromBody] NewsItem newsItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != newsItem.NewsItemID)
            {
                return BadRequest();
            }

            db.Entry(newsItem).State = EntityState.Modified;
            //The following two preevnt updates here
            db.Entry(newsItem).Property(p => p.NewsItemHasPic).IsModified = false;
            db.Entry(newsItem).Property(p => p.NewsItemPicExtension).IsModified = false;

            try
            {
            //    Debug.WriteLine(hop);
            
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        /// <summary>
        /// Updates/ adds an image to a specific news item
        /// This technique adds the image to the newsItems folder in the content folder and assigns each photo a id matching its newsitem ID this method only allows one photo per newsitem
        /// </summary>
        /// <param name="id"> The ID of the news item the photo is being added to</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateNewsItemImage(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var NewsItemImage = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (NewsItemImage.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(NewsItemImage.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //Speciies which folder the image is being stored in
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/NewsItems/"), fn);

                                //save the file
                                NewsItemImage.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the player haspic and picextension fields in the database
                                NewsItem SelectedNewsItems = db.NewsItems.Find(id);
                                SelectedNewsItems.NewsItemHasPic = haspic;
                                SelectedNewsItems.NewsItemPicExtension = extension;
                                db.Entry(SelectedNewsItems).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("News Item Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                            }
                        }
                    }

                }
            }

            return Ok();
        }
        /// <summary>
        /// Disposes 
        /// </summary>
        /// <param name="disposing" >The info being disposed( deleted)</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Checks to see if the news item exists
        /// </summary>
        /// <param name="id">The ID of the news item being checked</param>
        /// <returns></returns>
        private bool NewsItemExists(int id)
        {
            return db.NewsItems.Count(e => e.NewsItemID == id) > 0;
        }
    }

       
}
