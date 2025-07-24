using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AspNetClientFastApi.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Home/Error")]
        public IActionResult Error()
        {
            // Récupère les détails de l'erreur pour affichage/log si besoin
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            ViewBag.ErrorMessage = feature?.Error.Message;
            return View();
        }
    }
}
