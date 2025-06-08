using GmailClient.Ui.Contracts;
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
        public async Task<IActionResult> Index()
        {
            var messages = await _gmailDataService.GetAllMessages();
            return View(messages);
        }
    }
}
