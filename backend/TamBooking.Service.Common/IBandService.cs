using TamBooking.Common;
using TamBooking.Model;

namespace TamBooking.Service.Common
{
    public interface IBandService
    {
        Task<List<Band>> GetAllBandsAsync(BandFilter filter, Paging paging, Sorting sorting);

        Task<bool> DeleteBandAsync();

        Task<bool> UpdateBandAsync(Band band);

        Task CreateBandAsync(Band band);

        public Task<List<Band>> GetAllBandsCountAsync(BandFilter filter);
    }
}