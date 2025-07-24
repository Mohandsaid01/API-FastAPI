using AspNetClientFastApi.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace AspNetClientFastApi.Services
{
    /// <summary>
    /// Service qui communique avec l'API externe FastAPI pour gérer les opérations CRUD sur les acteurs.
    /// </summary>
    public class ActorService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructeur recevant un HttpClient injecté via DI (Dependency Injection).
        /// </summary>
        public ActorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Récupère la liste complète des acteurs depuis l'API FastAPI.
        /// </summary>
        public async Task<List<Actor>> GetActorsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("http://127.0.0.1:8001/acteurs");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var actors = JsonSerializer.Deserialize<List<Actor>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return actors ?? new List<Actor>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des acteurs : {ex.Message}");
            }

            // Retour vide en cas d'erreur
            return new List<Actor>();
        }

        /// <summary>
        /// Récupère un acteur spécifique via son ID.
        /// </summary>
        public async Task<Actor?> GetActorByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://127.0.0.1:8001/acteurs/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var actor = JsonSerializer.Deserialize<Actor>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return actor;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération de l'acteur #{id} : {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Envoie une requête POST à l'API pour créer un nouvel acteur.
        /// </summary>
        public async Task CreateActorAsync(Actor actor)
        {
            try
            {
                var json = JsonSerializer.Serialize(actor);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                await _httpClient.PostAsync("http://127.0.0.1:8001/acteurs/", content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création de l'acteur : {ex.Message}");
            }
        }

        /// <summary>
        /// Met à jour les informations d'un acteur via une requête PUT.
        /// </summary>
        public async Task UpdateActorAsync(int id, Actor actor)
        {
            try
            {
                var updateData = new
                {
                    name = actor.Name,
                    bio = actor.Bio,
                    picture = actor.Picture
                };

                var json = JsonSerializer.Serialize(updateData);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                await _httpClient.PutAsync($"http://127.0.0.1:8001/acteurs/{id}", content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour de l'acteur #{id} : {ex.Message}");
            }
        }

        /// <summary>
        /// Supprime un acteur via son ID avec une requête DELETE.
        /// </summary>
        public async Task DeleteActorAsync(int id)
        {
            try
            {
                await _httpClient.DeleteAsync($"http://127.0.0.1:8001/acteurs/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression de l'acteur #{id} : {ex.Message}");
            }
        }
    }
}
