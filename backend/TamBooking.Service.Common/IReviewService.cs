using TamBooking.Common;
using TamBooking.Model;

namespace TamBooking.Service.Common
{
    public interface IReviewService
    {
        Task<List<Review>> GetReviewsAsync(Paging paging, Sorting sorting);

        Task<List<Review>> GetReviewsByBandIdAsync(Guid bandId, Paging paging, Sorting sorting);

        Task<Guid> InsertReviewAsync(Review review);

        Task<bool> DeleteReviewByIdAsync(Guid reviewId);
        public Task<List<Review>> GetAllReviewsByBandIdAsync(Guid bandId);
    }
}