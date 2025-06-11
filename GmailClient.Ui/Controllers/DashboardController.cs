using GmailClient.Ui.Contracts;
using GmailClient.Ui.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Ui.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IGmailDataService _gmailDataService;
        private readonly IUserDataService _userDataService;

        public DashboardController(IGmailDataService gmailDataService, IUserDataService userDataService)
        {
            _gmailDataService = gmailDataService;
            _userDataService = userDataService;
        }

        [HttpGet]
        public async Task<IActionResult> Main()
        {
            var user = await _userDataService.GetUserDetails();

            if(user.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Login to account first";
                return RedirectToAction("Index", "Home");
            }

            if (user.Data.IsGoogleConnected)
            {
                var messages = await _gmailDataService.GetAllMessages();
                return View(messages.Data);
            }
            TempData["Message"] = "For use dashboard you need connect google account";
            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesPage(string pageToken)
        {
            var messages = await _gmailDataService.GetAllMessages(pageToken);
            return Json(new
            {
                messages = messages.Data.Messages.Select(m => new
                {
                    subject = m.Subject,
                    from = m.From,
                    date = m.Date,
                    isSent = m.IsSent,
                    body = m.Body
                }),
                nextPageToken = messages.Data.NextPageToken
            });
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] SendEmailRequest sendEmailRequest)
        {
            var result = await _gmailDataService.SendEmailAsync(sendEmailRequest);
            return Json(new { });
        }
    }
}
