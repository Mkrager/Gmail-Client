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
        public async Task<IActionResult> Login(AuthenticationViewModel request)
        {
            var result = await _authenticationService.Login(request.LoginRequest);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["LoginErrorMessage"] = HandleErrors.HandleResponse<bool>(result);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(AuthenticationViewModel request)
        {
            var result = await _authenticationService.Register(request.RegistrationRequest);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["LoginErrorMessage"] = HandleErrors.HandleResponse<bool>(result);

            }

            return RedirectToAction("Index", "Home");
        }
    }
}
