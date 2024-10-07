using TamBooking.Model;

namespace TamBooking.Repository.Common
{
    public interface IGigRepository
    {
        public Task<Guid> CreateGigAsync(Gig gig);

        public Task<List<Gig>> GetGigsAsync(Guid id);

        public Task<bool> DeleteGigAsync(Guid id);

        public Task<bool> ConfirmGigAsync(Guid id);
        public Task<Gig> GetGigById(Guid id);

    }
}