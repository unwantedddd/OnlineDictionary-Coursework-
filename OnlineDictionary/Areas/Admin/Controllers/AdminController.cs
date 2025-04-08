using Microsoft.AspNetCore.Mvc;

namespace OnlineDictionary.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }
        public IActionResult ManageWords()
        {
            return View();
        }
        public IActionResult ManageLanguage()
        {
            return View();
        }
    }
}

    

