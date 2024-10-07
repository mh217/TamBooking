using TamBooking.Model;

namespace TamBooking.Service.Common
{
    public interface IGigService
    {
        public Task<Guid> CreateGigAsync(Gig gig);

        public Task<List<Gig>> GetGigsAsync(Guid id);

        public Task<bool> DeleteGigAsync(Guid id);
        public Task<bool> ConfirmGigAsync(Guid id);
        public Task<Gig> GetGigById(Guid id);
    }
}