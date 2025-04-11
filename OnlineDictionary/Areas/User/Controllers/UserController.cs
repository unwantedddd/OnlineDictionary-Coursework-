using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineDictionary.DataAccess.Repository.IRepository;
using OnlineDictionary.Models;
using OnlineDictionary.Models.ViewModels;
using OnlineDictionary.Utility;
using static Google.Apis.Translate.v2.TranslationsResource;

namespace OnlineDictionary.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GoogleTranslateService _googleTranslateService;

        public UserController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, GoogleTranslateService googleTranslateService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _googleTranslateService = googleTranslateService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["LanguageCatalog"] = _unitOfWork.Language.GetAllAsNoTracking().ToList();
        }
        public IActionResult Dictionary(string? language, string? search)
        {

            IEnumerable<Word> words = _unitOfWork.Word.GetAll(includeProperties: "Language");

            if (!string.IsNullOrEmpty(search))
            {
                string lowerSearch = search.ToLower();
                words = words.Where(u => u.Name.ToLower().Contains(lowerSearch) ||
                                       (!string.IsNullOrEmpty(u.Description) && u.Description.ToLower().Contains(lowerSearch)));
            }


            if (!string.IsNullOrEmpty(language))
            {
                string lowerLanguage = language.ToLowerInvariant();
                words = words.Where(u => u.Language != null && u.Language.Name.ToLowerInvariant() == lowerLanguage);
            }

            return View(words);
        }

        [HttpPost, ActionName("Dictionary")]
        [ValidateAntiForgeryToken]
        public IActionResult DictionaryPOST(string? language)
        {
            return RedirectToAction("Dictionary", language);
        }

        public class TranslateRequest
        {
            public string Text { get; set; }
            public string Language { get; set; }
        }
        public IActionResult Translate()
        {
            var languages = _unitOfWork.Language.GetAll();

            WordVM wordVM = new WordVM
            {
                Languages = languages,
                Name = "",
                Description = ""
            };
            return View(wordVM);
        }

        [HttpPost]
        public async Task<IActionResult> Translate([FromBody] TranslateRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Text) || string.IsNullOrEmpty(request.Language))
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                var translatedText = await _googleTranslateService.Translate(request.Text, request.Language);
                return Json(new { translatedText });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
