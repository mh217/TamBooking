using TamBooking.Model.DomainModels;

namespace TamBooking.Service.Common
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailModel emailModel);
    }
}