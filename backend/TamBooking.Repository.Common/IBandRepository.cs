using TamBooking.Common;
using TamBooking.Model;

namespace TamBooking.Repository.Common
{
    public interface IBandRepository
    {
        Task<List<Band>> GetAllBandsAsync(BandFilter filter, Paging paging, Sorting sorting);

        Task<bool> DeleteBandAsync(Guid id);

        Task<bool> UpdateBandAsync(Guid id, Band band);

        Task CreateBandAsync(Band band);

        public Task<List<Band>> GetAllBandsCountAsync(BandFilter filter);
    }
}