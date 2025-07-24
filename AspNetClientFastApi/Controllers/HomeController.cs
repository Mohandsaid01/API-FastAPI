using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspNetClientFastApi.Models;

namespace AspNetClientFastApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Error par défaut (ex: erreurs système internes)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(errorModel);
        }

        // Error avec message personnalisé (ex: erreur de logique ou donnée manquante)
        public IActionResult Error(string message)
        {
            // On passe le message dans ViewData pour l'afficher dans la vue Error.cshtml
            ViewData["ErrorMessage"] = message ?? "Une erreur inattendue est survenue.";
            return View("Error");
        }
    }
}
