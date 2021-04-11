using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TiminsHospitalProjectV3.Models;
using System.Diagnostics;

namespace TiminsHospitalProjectV3.Controllers
{
    public class ReviewDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gets a list of the reviews in the database
        /// </summary>
        /// <returns> A list of reviews in the database</returns>
        // GET: api/ReviewData
        [HttpGet]
        [ResponseType(typeof(IEnumerable<ReviewDto>))]
        public IHttpActionResult ListReviews()
        {
            List<Review> Reviews = db.Reviews.ToList();
            List<ReviewDto> ReviewDtos = new List<ReviewDto> { };

            foreach (var review in Reviews)
            {
                ReviewDto NewReview = new ReviewDto
                {
                    ReviewID = review.ReviewID,
                    ReviewDate = review.ReviewDate,
                    ReviewContent = review.ReviewContent,
                    ReviewRating = review.ReviewRating
                };
                ReviewDtos.Add(NewReview);
            }
            return Ok(ReviewDtos);
        } //end of List Reviews

        //GET: api/ReviewData/FindReview/id
        [HttpGet]
        [ResponseType(typeof(ReviewDto))]
        public IHttpActionResult FindReview(int id)
        {
            Review Review = db.Reviews.Find(id);
            if (Review == null)
            {
                return NotFound();
            }

            ReviewDto @ReviewDto = new ReviewDto
            {
                ReviewID = Review.ReviewID,
                ReviewDate = Review.ReviewDate,
                ReviewRating = Review.ReviewRating,
                ReviewContent = Review.ReviewContent
            };
            return Ok(ReviewDto);
        }

        //Add REview
        [ResponseType(typeof(Review))]
        [HttpPost]
        public IHttpActionResult AddReview([FromBody] Review @review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Reviews.Add(@review);
            db.SaveChanges();

            return Ok(review.ReviewID);
        }

        //delete Review
        [HttpPost]
        public IHttpActionResult DeleteReview(int id)
        {
            Review @review = db.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }
            db.Reviews.Remove(@review);
            db.SaveChanges();
            return Ok();
        }

        //update REview
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateReview (int id, [FromBody] Review @review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != review.ReviewID)
            {
                return BadRequest();
            }
            db.Entry(@review).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        private bool ReviewExists(int id)
        {
            return db.Reviews.Count(e => e.ReviewID == id) > 0;
        }

    }
}