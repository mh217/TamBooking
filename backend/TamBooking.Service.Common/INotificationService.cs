using TamBooking.Model;

namespace TamBooking.Service.Common
{
    public interface INotificationService
    {
        public Task<List<Notification>> GetNotificationAsync();

        public Task CreateNotificationAsync(Guid bandId, Guid gigId, Gig gig);

        public Task CreateConfirmNotificationAsync(Gig gig);

        public Task CreateCancelNotificationAsync(Gig gig);

        public Task<bool> DeleteNotificationAsync(Guid id);
    }
}