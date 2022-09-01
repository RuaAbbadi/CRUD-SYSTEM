using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace OnlineLibrary.Controllers
{
    public class ServiceController : Controller
    {
        public IEmailSender EmailSender { get; set; }

        public ServiceController(IEmailSender emailSender)
        {
            EmailSender = emailSender;
        }

        public async Task<IActionResult> Send(string toAddress)
        {
            var subject = "sample subject";
            var body = "sample body";
            await EmailSender.SendEmailAsync(toAddress, subject, body);
            return View();
        }
    }
}
