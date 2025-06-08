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

        public async Task<IActionResult> Index()
        {
            var user = await _userDataService.GetUserDetails();
            return View(user);
        }
    }
}
