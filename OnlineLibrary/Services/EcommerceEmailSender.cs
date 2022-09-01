using Microsoft.AspNetCore.Identity.UI.Services;

namespace OnlineLibrary.Services
{
    public class EcommerceEmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await Task.Run(() => { });
        }
    }
}

