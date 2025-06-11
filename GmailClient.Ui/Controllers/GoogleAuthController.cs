using GmailClient.Ui.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Ui.Controllers
{
    public class GoogleAuthController : Controller
    {
        private readonly IGoogleOAuthDataService _googleOAuthDataService;

        public GoogleAuthController(IGoogleOAuthDataService googleOAuthDataService)
        {
            _googleOAuthDataService = googleOAuthDataService;
        }

        [HttpPost]
        public async Task<IActionResult> Login()
       {
            var result = await _googleOAuthDataService.GetGoogleSignInUrlAsync();
            return Json(new { googleUrl = result});
        }
    }
}
