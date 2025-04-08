using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineDictionary.DataAccess.Repository.IRepository;
using OnlineDictionary.Models;
using OnlineDictionary.Utility;

namespace OnlineDictionary.Areas.User.Controllers
{
    [Area("User")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Dictionary()
        {
            return View();
        }
        public IActionResult Translate()
        {
            return View();
        }
    }
}
