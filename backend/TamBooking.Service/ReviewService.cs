using TamBooking.Common;
using TamBooking.Model;
using TamBooking.Repository.Common;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IJwtService _jwtService;

        public ReviewService(IJwtService jwtService, IReviewRepository reviewRepository)
        {
            _jwtService = jwtService;
            _reviewRepository = reviewRepository;
        }

        public async Task<List<Review>> GetAllReviewsByBandIdAsync(Guid bandId)
        {
            return await _reviewRepository.GetAllReviewsByBandIdAsync(bandId);
        }

        public async Task<List<Review>> GetReviewsAsync(Paging paging, Sorting sorting)
        {
            return await _reviewRepository.GetReviewsAsync(paging, sorting);
        }

        public async Task<List<Review>> GetReviewsByBandIdAsync(Guid bandId, Paging paging, Sorting sorting)
        {
            return await _reviewRepository.GetReviewsByBandIdAsync(bandId, paging, sorting);
        }

        public async Task<Guid> InsertReviewAsync(Review review)
        {
            var userId = Guid.Parse(_jwtService.GetCurrentUserClaims().Id);
            review.ClientId = userId;
            review.CreatedByUserId = userId;
            review.UpdatedByUserId = userId;
            return await _reviewRepository.InsertReviewAsync(review);
        }

        public async Task<bool> DeleteReviewByIdAsync(Guid reviewId)
        {
            return await _reviewRepository.DeleteReviewByIdAsync(reviewId);
        }
    }
}