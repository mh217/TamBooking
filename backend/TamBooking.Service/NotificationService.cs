using Microsoft.AspNetCore.Http;
using TamBooking.Model;
using TamBooking.Model.DomainModels;
using TamBooking.Repository.Common;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public NotificationService(INotificationRepository notificationRepository, IEmailService emailService, IHttpContextAccessor httpContextAccessor,
            IJwtService jwtService, IUserService userService)
        {
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public async Task<List<Notification>> GetNotificationAsync()
        {
            var currentUserId = _jwtService.GetCurrentUserClaims();
            var id = Guid.Parse(currentUserId.Id);
            var notifications = await _notificationRepository.GetNotificationAsync(id);
            return notifications;
        }

        public async Task CreateNotificationAsync(Guid bandId, Guid gigId, Gig gig)
        {
            var currentUserId = Guid.Parse(_jwtService.GetCurrentUserClaims().Id);
            var role = _jwtService.GetCurrentUserClaims().Role;
            var userId = currentUserId;
            var userEmail = _jwtService.GetCurrentUserClaims().Email;
            var toEmail = await _userService.GetUserByIdAsync(bandId);
            var bandEmail = toEmail.Email;
            Guid id = await _notificationRepository.CreateNotificationAsync(bandId, role, userId, userEmail, bandEmail, gigId, gig);
            Notification? notification = await _notificationRepository.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                throw new Exception();
            }
            EmailModel emailModel = new()
            {
                ToEmail = notification.To,
                Subject = notification.Title,
                Body = notification.Text
            };
            await _emailService.SendEmailAsync(emailModel);
        }

        public async Task CreateConfirmNotificationAsync(Gig gig)
        {
            var currentUserId = Guid.Parse(_jwtService.GetCurrentUserClaims().Id);
            var role = _jwtService.GetCurrentUserClaims().Role;
            var userId = currentUserId;
            var toBandEmail = await _userService.GetUserByIdAsync(gig.BandId);
            var bandEmail = toBandEmail.Email;
            var toClientEmail = await _userService.GetUserByIdAsync(gig.ClientId);
            var clientEmail = toClientEmail.Email;
            Guid id = await _notificationRepository.CreateConfirmNotificationAsync(gig.BandId, role, userId, clientEmail, bandEmail, gig.Id, gig.ClientId);
            Notification? notification = await _notificationRepository.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                throw new Exception();
            }
            EmailModel emailModel = new()
            {
                ToEmail = bandEmail,
                Subject = notification.Title,
                Body = notification.Text
            };
            await _emailService.SendEmailAsync(emailModel);
            EmailModel emailModel2 = new()
            {
                ToEmail = clientEmail,
                Subject = notification.Title,
                Body = notification.Text
            };
            await _emailService.SendEmailAsync(emailModel2);
        }

        public async Task CreateCancelNotificationAsync(Gig gig)
        {
            var currentUserId = _jwtService.GetCurrentUserClaims();
            var role = currentUserId.Role;
            var userId = Guid.Parse(currentUserId.Id);
            var toBandEmail = await _userService.GetUserByIdAsync(gig.BandId);
            var bandEmail = toBandEmail.Email;
            var toClientEmail = await _userService.GetUserByIdAsync(gig.ClientId);
            var clientEmail = toClientEmail.Email;
            Guid id = await _notificationRepository.CreateCancelNotificationAsync(gig.BandId, role, userId, clientEmail, bandEmail, gig.Id);
            Notification? notification = await _notificationRepository.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                throw new Exception();
            }
            EmailModel emailModel = new()
            {
                ToEmail = bandEmail,
                Subject = notification.Title,
                Body = notification.Text
            };
            await _emailService.SendEmailAsync(emailModel);
            EmailModel emailModel2 = new()
            {
                ToEmail = clientEmail,
                Subject = notification.Title,
                Body = notification.Text
            };
            await _emailService.SendEmailAsync(emailModel2);
        }

        public async Task<bool> DeleteNotificationAsync(Guid id)
        {
            var isNotificationDeleted = await _notificationRepository.DeleteNotificationAsync(id);
            return isNotificationDeleted;
        }
    }
}