using GmailClient.Ui.Contracts;
using GmailClient.Ui.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Ui.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IGmailDataService _gmailDataService;

        public DashboardController(IGmailDataService gmailDataService)
        {
            _gmailDataService = gmailDataService;
        }

        [HttpGet]
        public async Task<IActionResult> Main()
        {
            var messages = await _gmailDataService.GetAllMessages();
            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesPage(string pageToken)
        {
            var messages = await _gmailDataService.GetAllMessages(pageToken);
            return Json(new
            {
                messages = messages.Messages.Select(m => new
                {
                    subject = m.Subject,
                    from = m.From,
                    date = m.Date,
                    isSent = m.IsSent,
                    body = m.Body
                }),
                nextPageToken = messages.NextPageToken
            });
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] SendEmailRequest sendEmailRequest)
        {
            var result = await _gmailDataService.SendEmailAsync(sendEmailRequest);
            return Json(new {});
        }
    }
}
