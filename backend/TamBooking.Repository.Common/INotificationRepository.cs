using TamBooking.Model;

namespace TamBooking.Repository.Common
{
    public interface INotificationRepository
    {
        public Task<List<Notification>> GetNotificationAsync(Guid id);

        public Task<Notification?> GetNotificationByIdAsync(Guid id);

        public Task<Guid> CreateNotificationAsync(Guid bandId, string role, Guid userId, string userEmail, string bandEmail, Guid gigId, Gig gig);

        public Task<Guid> CreateConfirmNotificationAsync(Guid bandId, string role, Guid userId, string userEmail, string bandEmail, Guid gigId, Guid clientId);

        public Task<Guid> CreateCancelNotificationAsync(Guid bandId, string role, Guid userId, string userEmail, string bandEmail, Guid gigId);

        public Task<bool> DeleteNotificationAsync(Guid id);
    }
}