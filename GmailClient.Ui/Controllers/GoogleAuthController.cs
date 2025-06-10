using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Ui.Controllers
{
    public class GoogleAuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
