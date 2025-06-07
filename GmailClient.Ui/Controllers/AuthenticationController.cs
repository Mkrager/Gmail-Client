using GmailClient.Ui.Contracts;
using GmailClient.Ui.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Ui.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthenticationViewModel request)
        {
            var result = await _authenticationService.Login(request.LoginRequest);
            //TempData["LoginErrorMessage"] = HandleErrors.HandleResponse<bool>(result, "Success");

            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(AuthenticationViewModel request)
        {
            var result = await _authenticationService.Register(request.RegistrationRequest);
            //TempData["LoginErrorMessage"] = HandleErrors.HandleResponse<bool>(result, "Success");

            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
