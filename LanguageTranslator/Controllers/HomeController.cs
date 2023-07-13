
using YourProjectName.Services;
using YourProjectName.Models;
using Microsoft.AspNetCore.Mvc;
using YourProjectName.Services;

namespace YourProjectName.Controllers
{
    public class HomeController : Controller
    {
        private readonly TranslationService _translationService;

        public HomeController(TranslationService translationservice )
        {
            _translationService = translationservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Translate(TranslationModel model)
        {
            ModelState.Remove("TranslatedText");
            if (!ModelState.IsValid)
            {
                // Handle validation errors
                return View("Index", model);
            }

          //  var translatedText = _translationService.TranslateText(model.InputText, model.InputLanguage, model.OutputLanguage);
            var translatedText = await _translationService.TranslateText(model.InputText, model.InputLanguage, model.OutputLanguage);
            if (!string.IsNullOrEmpty(translatedText))
            {
                model.TranslatedText = translatedText;
            }
            else
            {
                // Handle translation error
                ModelState.AddModelError(string.Empty, "Translation failed.");
            }

            return View("Index", model);
        }
    }
}
