using AspNetClientFastApi.Services;
using AspNetClientFastApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetClientFastApi.Controllers
{
    public class ActorsController : Controller
    {
        private readonly ActorService _actorService;

        // Constructeur avec injection de dépendance du service API
        public ActorsController(ActorService actorService)
        {
            _actorService = actorService;
        }

        /// <summary>
        /// Affiche la liste de tous les acteurs
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var actors = await _actorService.GetActorsAsync();
            return View(actors);
        }

        /// <summary>
        /// Affiche le formulaire de création d’un nouvel acteur
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Enregistre un nouvel acteur via POST
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(Actor actor)
        {
            if (!ModelState.IsValid)
                return View(actor); // Affiche les erreurs si données invalides

            await _actorService.CreateActorAsync(actor);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Affiche les détails d’un acteur par ID
        /// </summary>
      public async Task<IActionResult> Details(int id)
{
    var actor = await _actorService.GetActorByIdAsync(id);
    if (actor == null)
        return RedirectToAction("Error", "Home", new { message = $"Aucun acteur trouvé avec l'ID {id}" });

    return View(actor);
}
        /// <summary>
        /// Redirige vers la page de détails (utilisé pour les recherches depuis les formulaires)
        /// </summary>
        [HttpGet]
        public IActionResult Search(int id)
        {
            return RedirectToAction("Details", new { id });
        }

        /// <summary>
        /// Affiche le formulaire de modification d’un acteur existant
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var actor = await _actorService.GetActorByIdAsync(id);
            if (actor == null)
                return RedirectToAction("Error", "Home", new { message = $"Impossible de modifier : acteur {id} introuvable." });

            return View(actor);
        }

        /// <summary>
        /// Enregistre les modifications d’un acteur
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(Actor actor)
        {
            if (!ModelState.IsValid)
                return View(actor);

            await _actorService.UpdateActorAsync(actor.Id, actor);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Supprime un acteur par son ID
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _actorService.GetActorByIdAsync(id);
            if (actor == null)
                return RedirectToAction("Error", "Home", new { message = $"Impossible de supprimer : acteur {id} introuvable." });

            await _actorService.DeleteActorAsync(id);
            return RedirectToAction("Index");
        }
    }
}
