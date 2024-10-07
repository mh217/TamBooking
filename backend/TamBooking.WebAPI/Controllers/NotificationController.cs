using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TamBooking.Model;
using TamBooking.Service.Common;
using TamBooking.WebAPI.RESTModels;

namespace TamBooking.WebAPI.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationAsync()
        {
            var notification = await _notificationService.GetNotificationAsync();
            if (notification is null)
            {
                return BadRequest("Not Found");
            }
            List<GetNotification> notifications = new List<GetNotification>();
            foreach (var note in notification)
            {
                GetNotification newNotification = new GetNotification();
                newNotification.Id = note.Id;
                newNotification.From = note.From;
                newNotification.To = note.To;
                newNotification.Text = note.Text;
                newNotification.Title = note.Title;
                newNotification.GigId = note.GigId;
                notifications.Add(newNotification);
            }
            return Ok(notifications);
        }

        [HttpPost]
        [Route("{notificationId}")]
        [Authorize(Roles = "client,band,admin")]
        public async Task<IActionResult> CreateConfirmNotificationAsync(Gig gig)
        {
            await _notificationService.CreateConfirmNotificationAsync(gig);
            return Ok("Notification created");
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "band, client")]
        public async Task<IActionResult> DeleteNotificationAsync(Guid id)
        {
            var isNotificationDeleted = await _notificationService.DeleteNotificationAsync(id);
            if (!isNotificationDeleted)
            {
                return BadRequest("Notification not deleted");
            }
            return Ok("Notification deleted");
        }
    }
}