using Microsoft.AspNetCore.Http;
using TamBooking.Model;
using TamBooking.Repository.Common;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class GigService : IGigService
    {
        private readonly IGigRepository _gigRepository;
        private readonly INotificationService _notificationService;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GigService(IGigRepository gigRepository, IHttpContextAccessor httpContextAccessor, IJwtService jwtService, INotificationService notificationService)
        {
            _gigRepository = gigRepository;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
        }

        public async Task<Guid> CreateGigAsync(Gig gig)
        {
            var currentUserId = Guid.Parse(_jwtService.GetCurrentUserClaims().Id);
            gig.CreatedByUserId = currentUserId;
            gig.UpdatedByUserId = currentUserId;
            gig.ClientId = Guid.Parse(_jwtService.GetCurrentUserClaims().Id);
            var gigId = await _gigRepository.CreateGigAsync(gig);
            var gigForNotification = await GetGigById(gigId);
            await _notificationService.CreateNotificationAsync(gig.BandId, gigId, gigForNotification);
            return gigId;
        }

        public async Task<List<Gig>> GetGigsAsync(Guid id)
        {
            var gigs = await _gigRepository.GetGigsAsync(id);
            return gigs;
        }

        public async Task<bool> DeleteGigAsync(Guid id)
        {
            var isGigDeleted = await _gigRepository.DeleteGigAsync(id);
            if (!isGigDeleted)
            {
                return false;
            }
            var gig = await GetGigById(id);
            await _notificationService.CreateCancelNotificationAsync(gig);
            return true;
        }

        public async Task<bool> ConfirmGigAsync(Guid id)
        {
            var isGigDeleted = await _gigRepository.ConfirmGigAsync(id);
            if (!isGigDeleted)
            {
                return false;
            }
            var gig = await GetGigById(id);
            await _notificationService.CreateConfirmNotificationAsync(gig);
            return true;
        }

        public async Task<Gig> GetGigById(Guid id)
        {
            var gig = await _gigRepository.GetGigById(id);
            return gig;
        }
    }
}