using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineDictionary.DataAccess.Repository.IRepository;
using OnlineDictionary.Models;
using OnlineDictionary.Models.ViewModels;

namespace OnlineDictionary.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _hostEnviroment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IWebHostEnvironment hostEnviroment, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _hostEnviroment = hostEnviroment;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["LanguageList"] = _unitOfWork.Language.GetAllAsNoTracking().ToList();
            var userId = _userManager.GetUserId(User);
            base.OnActionExecuting(context);
        }
        public IActionResult Index()
        {
            return View();
        }

        // ------------------------------- WORDS --------------------------------------------
        public IActionResult ManageWords()
        {
            var Words = _unitOfWork.Word.GetAll(includeProperties: "Language");
            return View(Words);
        }
        public IActionResult AddWord()
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
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddWord([FromForm] WordVM obj)
        {
            if (ModelState.IsValid)
            {
                var existingWord = _unitOfWork.Word.GetFirstOrDefault(
                    w => w.Name == obj.Name && w.LanguageId == obj.LanguageId
                );

                if (existingWord != null)
                {
                    ModelState.AddModelError("Name", "Such a word already exists for this language.");
                    obj.Languages = _unitOfWork.Language.GetAll();
                    return View(obj);
                }

                Word word = new()
                {
                    Name = obj.Name,
                    Description = obj.Description,
                    LanguageId = obj.LanguageId
                };

                _unitOfWork.Word.Add(word);
                _unitOfWork.Save();
                TempData["success"] = "Word added successfully!";
                return RedirectToAction("ManageWords");
            }

            obj.Languages = _unitOfWork.Language.GetAll();
            return View(obj);
        }

        public IActionResult EditWord(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var WordFromDb = _unitOfWork.Word.GetFirstOrDefault(u => u.Id == id);
            if (WordFromDb == null)
            {
                return NotFound();
            }
            var languages = _unitOfWork.Language.GetAll();
            WordVM wordVM = new()
            {
                Languages = languages,
                Name = WordFromDb.Name,
                Description = WordFromDb.Description,
                LanguageId = WordFromDb.LanguageId
            };

            return View(wordVM);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditWord(WordVM obj)
        {
            if (ModelState.IsValid)
            {
                Word word = new()
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Description= obj.Description,
                    LanguageId = obj.LanguageId
                };
                var WordFromDb = _unitOfWork.Word.GetFirstOrDefault(u => u.Id == obj.Id, tracked: false);

                _unitOfWork.Word.Update(word);
                _unitOfWork.Save();
                TempData["success"] = "Word updated successfully!";
                return RedirectToAction("ManageWords");
            }
            return View(obj);
        }
        public IActionResult RemoveWord(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var WordFromDb = _unitOfWork.Word.GetFirstOrDefault(u => u.Id == id);
            if (WordFromDb == null)
            {
                return NotFound();
            }
            return View(WordFromDb);
        }

        // POST
        [HttpPost, ActionName("RemoveWord")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveWordPOST(int? id)
        {
            var obj = _unitOfWork.Word.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Word.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Word deleted succsessfully!";
            return RedirectToAction("ManageWords");
        }

        // ---------------------------------- LANGUAGE -----------------------------------
        public IActionResult ManageLanguage()
        {
            var Languages = _unitOfWork.Language.GetAll();
            return View(Languages);
        }
        public IActionResult AddLanguage()
        {
            return View();
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLanguage(Language obj)
        {
            if (ModelState.IsValid)
            {
                var existingLanguage = _unitOfWork.Language.GetFirstOrDefault(
                    l => l.Name.ToLower() == obj.Name.ToLower()
                );
                var existingCode = _unitOfWork.Language.GetFirstOrDefault(
                   l => l.Code.ToLower() == obj.Code.ToLower()
               );

                if (existingLanguage != null)
                {
                    ModelState.AddModelError("Name", "Such a language already exists.");
                    return View(obj);
                }

                if (existingCode != null)
                {
                    ModelState.AddModelError("Code", "Such a code already exists.");
                    return View(obj);
                }

                _unitOfWork.Language.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Language added successfully!";
                return RedirectToAction("ManageLanguage");
            }
            return View(obj);
        }

        public IActionResult EditLanguage(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var LanguageFromDb = _unitOfWork.Language.GetFirstOrDefault(u => u.Id == id);
            
            if (LanguageFromDb == null)
            {
                return NotFound();
            }

            return View(LanguageFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditLanguage(Language obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Language.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Language updated succsessfully!";
                return RedirectToAction("ManageLanguage");
            }
            return View(obj);
        }
        public IActionResult RemoveLanguage(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var LanguageFromDb = _unitOfWork.Language.GetFirstOrDefault(u => u.Id == id);
            if (LanguageFromDb == null)
            {
                return NotFound();
            }
            return View(LanguageFromDb);
        }

        // POST
        [HttpPost, ActionName("RemoveLanguage")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveLanguagePOST(int? id)
        {
            if (_unitOfWork.Word.GetFirstOrDefault(u => u.LanguageId == id) != null)
            {
                TempData["failure"] = "Language cannot be deleted!";
                return RedirectToAction("ManageLanguage");
            }

            var obj = _unitOfWork.Language.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Language.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Language deleted succsessfully!";
            return RedirectToAction("ManageLanguage");
        }
    }
}

    

