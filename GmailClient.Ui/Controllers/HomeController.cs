using GmailClient.Ui.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Ui.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            var model = new HomeErrorViewModel()
            {
                Message = TempData["Message"] as string
            };
            return View(model);
        }
    }
}
