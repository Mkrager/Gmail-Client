using GmailClient.Ui.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Ui.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserDataService _userDataService;
        private readonly IAuthenticationDataService _authenticationDataService;

        public AccountController(IUserDataService userDataService, IAuthenticationDataService authenticationDataService)
        {
            _userDataService = userDataService;
            _authenticationDataService = authenticationDataService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userDataService.GetUserDetails();

            user.Data.Message = TempData["Message"] as string;

            if (!user.IsSuccess)
            {
                TempData["Message"] = "Login to account first";
                return RedirectToAction("Index", "Home");
            }

            return View(user.Data);
        }

        public async Task<IActionResult> Logout()
        {
            await _authenticationDataService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
