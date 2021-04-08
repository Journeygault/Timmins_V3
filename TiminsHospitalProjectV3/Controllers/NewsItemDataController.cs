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



//Port Number:44346
namespace TiminsHospitalProjectV3.Controllers
{
    public class NewsItemDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: api/NewsItemData/GetNewsItems
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
        [ResponseType(typeof(NewsItem))]
        [HttpPost]
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

        [HttpPost]
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

        [ResponseType(typeof(void))]
        [HttpPost]
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NewsItemExists(int id)
        {
            return db.NewsItems.Count(e => e.NewsItemID == id) > 0;
        }
    }

       
}
