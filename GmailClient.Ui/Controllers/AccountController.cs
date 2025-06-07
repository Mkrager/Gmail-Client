using GmailClient.Ui.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Ui.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserDataService _userDataService;

        public AccountController(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public async Task<IActionResult> Index(string userId)
        {
            var user = await _userDataService.GetUserDetails(userId);
            return View(user);
        }
    }
}
