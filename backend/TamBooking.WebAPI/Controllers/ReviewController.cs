using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TamBooking.Common;
using TamBooking.Model;
using TamBooking.Service.Common;
using TamBooking.WebAPI.RESTModels;

namespace TamBooking.WebAPI.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviewsAsync(int pageSize = 10, int pageNumber = 1, string sortBy = "Id",
            string sortDirection = "desc")
        {
            Paging paging = new()
            {
                Rpp = pageSize,
                PageNumber = pageNumber,
            };

            Sorting sorting = new()
            {
                OrderBy = sortBy,
                OrderDirection = sortDirection
            };

            List<Review> reviews = await _reviewService.GetReviewsAsync(paging, sorting);

            return Ok(
                reviews.Select(review => new ReviewWithBandGet
                {
                    Id = review.Id,
                    Rating = review.Rating,
                    Text = review.Text,
                    DateCreated = review.DateCreated,
                    BandId = review.Band.Id,
                    BandName = review.Band.Name
                }).ToList());
        }

        [HttpGet]
        [Route("GetAllReviews/{id}")]
        public async Task<IActionResult> GetAllReviewsByBandIdAsync(Guid id)
        {
            var allReviews = await _reviewService.GetAllReviewsByBandIdAsync(id);
            if (allReviews == null || allReviews.Count == 0)
            {
                return Ok(0); 
            }
            return Ok(allReviews.Count);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewsByBandIdAsync(Guid id, int pageSize = 10, int pageNumber = 1, string sortBy = "Id",
            string sortDirection = "desc")
        {
            Paging paging = new()
            {
                Rpp = pageSize,
                PageNumber = pageNumber,
            };

            Sorting sorting = new()
            {
                OrderBy = sortBy,
                OrderDirection = sortDirection
            };

            List<Review> reviews = await _reviewService.GetReviewsByBandIdAsync(id, paging, sorting);
            var allReviews = await _reviewService.GetAllReviewsByBandIdAsync(id);

            int totalPages = allReviews.Count;


            return Ok(
                reviews.Select(review => new ReviewGet
                {
                    Id = review.Id,
                    Rating = review.Rating,
                    Text = review.Text,
                    DateCreated = review.DateCreated
                }).ToList());
        }

        [HttpPost("add")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> InsertReviewAsync(ReviewAdd reviewAdd)
        {
            Review review = new()
            {
                Rating = reviewAdd.Rating,
                Text = reviewAdd.Text,
                BandId = reviewAdd.BandId,
            };
            await _reviewService.InsertReviewAsync(review);

            return StatusCode(201);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteReviewByIdAsync(Guid id)
        {
            bool isDeleted = await _reviewService.DeleteReviewByIdAsync(id);

            if (isDeleted == false)
            {
                return NotFound("Review not found");
            }
            return NoContent();
        }
    }
}