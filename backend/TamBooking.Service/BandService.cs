using TamBooking.Common;
using TamBooking.Model;
using TamBooking.Repository;
using TamBooking.Repository.Common;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class BandService : IBandService
    {
        private readonly IBandRepository _bandRepository;
        private readonly IJwtService _jwtService;

        public BandService(IBandRepository bandRepository, IJwtService jwtService)
        {
            _bandRepository = bandRepository;
            _jwtService = jwtService;
        }

        public async Task<List<Band>> GetAllBandsCountAsync(BandFilter filter)
        {
            return await _bandRepository.GetAllBandsCountAsync(filter);
        }

        public async Task<List<Band>> GetAllBandsAsync(BandFilter filter, Paging paging, Sorting sorting)
        {
            return await _bandRepository.GetAllBandsAsync(filter, paging, sorting);
        }

        public async Task<bool> DeleteBandAsync()
        {
            return await _bandRepository.DeleteBandAsync(Guid.Parse(_jwtService.GetCurrentUserClaims().Id));
        }

        public async Task<bool> UpdateBandAsync(Band band)
        {
            return await _bandRepository.UpdateBandAsync(Guid.Parse(_jwtService.GetCurrentUserClaims().Id), band);
        }

        public async Task CreateBandAsync(Band band)
        {
            var currentUserId = _jwtService.GetCurrentUserClaims().Id;
            band.Id = Guid.Parse(currentUserId);
            await _bandRepository.CreateBandAsync(band);
        }
    }
}