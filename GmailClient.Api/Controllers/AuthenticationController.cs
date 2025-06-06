using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : Controller
    {
        [HttpGet("signin-google")]
        public IActionResult SignInWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest();

            var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims;

            var accessToken = authenticateResult.Properties.GetTokenValue("access_token");

            return Ok(new
            {
                AccessToken = accessToken,
                Claims = claims.Select(c => new { c.Type, c.Value })
            });
        }
    }
}
