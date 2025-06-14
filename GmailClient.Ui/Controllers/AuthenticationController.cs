using GmailClient.Ui.Contracts;
using GmailClient.Ui.Helpers;
using GmailClient.Ui.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Ui.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationDataService _authenticationService;

        public AuthenticationController(IAuthenticationDataService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthenticationViewModel request)
        {
            var result = await _authenticationService.Login(request.LoginRequest);

            if (!result.IsSuccess)
            {
                return Json(new { success = false, error = result.ErrorText });
            }

            return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] AuthenticationViewModel request)
        {
            var result = await _authenticationService.Register(request.RegistrationRequest);

            if (!result.IsSuccess)
            {
                return Json(new { success = false, error = result.ErrorText });
            }

            return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
        }
    }
}
