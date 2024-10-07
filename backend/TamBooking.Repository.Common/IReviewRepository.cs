using TamBooking.Common;
using TamBooking.Model;

namespace TamBooking.Repository.Common
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewsAsync(Paging paging, Sorting sorting);

        Task<List<Review>> GetReviewsByBandIdAsync(Guid bandId, Paging paging, Sorting sorting);

        Task<Guid> InsertReviewAsync(Review review);

        Task<bool> DeleteReviewByIdAsync(Guid id);

        public Task<List<Review>> GetAllReviewsByBandIdAsync(Guid bandId);
    }
}