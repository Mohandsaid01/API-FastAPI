var builder = WebApplication.CreateBuilder(args);

// ------------------------------------
// 1. Configuration des services 
// ------------------------------------

// Ajoute les services nécessaires à MVC 
builder.Services.AddControllersWithViews();

// Enregistre ActorService avec HttpClient pour effectuer des appels vers l’API externe
// Ceci permet d'injecter `ActorService` dans les contrôleurs
builder.Services.AddHttpClient<AspNetClientFastApi.Services.ActorService>();

// ------------------------------------
// 2. Construction de l’application
// ------------------------------------
var app = builder.Build();

// ------------------------------------
// 3. Pipeline des middlewares HTTP
// ------------------------------------

// En production : redirige vers la page d’erreur générique en cas d'exception
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Active HSTS 
}

// Redirection automatique vers HTTPS si requête HTTP
app.UseHttpsRedirection();

// Autorise la lecture de fichiers statiques 
app.UseStaticFiles();

// Active le système de routage ASP.NET
app.UseRouting();

// Active le système d'autorisation 
app.UseAuthorization();

// ------------------------------------
// 4. Configuration du routage
// ------------------------------------

// Définir le contrôleur Actors et l'action Index comme page par défaut
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Actors}/{action=Index}/{id?}"
);

// ------------------------------------
// 5. Lancement de l’application
// ------------------------------------
app.Run();
