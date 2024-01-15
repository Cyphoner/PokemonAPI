
using System.Linq.Expressions;
using Newtonsoft.Json;
using PokemonAPI.Models;

namespace PokemonAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.MapGet("/hello", () =>
            {
                return "hello world";
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/randomjoke", async () =>
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://api.chucknorris.io/jokes/random");
                    var content = await response.Content.ReadAsStringAsync();

                    var joke = JsonConvert.DeserializeObject<Joke>(content);

                    return Results.Ok(content);
                }
            });

            var pokemons = new List<Pokemon> 
            {
                /*new Pokemon {Id = 1, Name = "Bulbasaur", Type = "Grass"},
                new Pokemon {Id = 2, Name = "Ivysaur", Type = "Grass"},
                new Pokemon {Id = 3, Name = "Venosaur", Type = "Grass"},
                new Pokemon {Id = 4, Name = "Charmander", Type = "Fire"},
                new Pokemon {Id = 5, Name = "Charmeleon", Type = "Fire"},
                new Pokemon {Id = 6, Name = "Charizard", Type = "Fire"},
                new Pokemon {Id = 7, Name = "Squirtle", Type = "Water"},
                new Pokemon {Id = 8, Name = "Wartortle", Type = "Water"},
                new Pokemon {Id = 9, Name = "Blastoise", Type = "Water"}*/
            };

            app.MapGet("/pokemons", () =>
            {
                return Results.Ok(pokemons);
            });

            app.MapGet("/pokemon/{id}", async (int id) =>
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        var response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon/{id}");
                        var content = await response.Content.ReadAsStringAsync();

                        var pokemon = JsonConvert.DeserializeObject<Pokemon>(content);
                        return Results.Ok(pokemon);
                    }
                    catch
                    {
                        return Results.NotFound("Could not found Pokemon");
                    }
                }

            });

            //Digimon add
            var digimons = new List<Digimon>
            {
                new Digimon {Id = 1, Name = "Bulbasaurmon", Type = "Grass"},
                new Digimon {Id = 2, Name = "Ivysaurmon", Type = "Grass"},
                new Digimon {Id = 3, Name = "Venosaurmon", Type = "Grass"},
                new Digimon {Id = 4, Name = "Charmandermon", Type = "Fire"},
                new Digimon {Id = 5, Name = "Charmeleonmon", Type = "Fire"},
                new Digimon {Id = 6, Name = "Charizardmon", Type = "Fire"},
                new Digimon {Id = 7, Name = "Squirtlemon", Type = "Water"},
                new Digimon {Id = 8, Name = "Wartortlemon", Type = "Water"},
                new Digimon {Id = 9, Name = "Blastoisemon", Type = "Water"}
            };

            app.MapGet("/digimon", () =>
            {
                return Results.Ok(digimons);
            });

            app.MapGet("/digimon/{id}", (int id) =>
            {
                var digimon = digimons.Find(p => p.Id == id);

                if (digimon == null)
                {
                    return Results.NotFound("Sorry, this Digimon does not exist.");
                }

                return Results.Ok(digimon);

            });

            app.MapPost("/pokemon", (Pokemon pokemon) =>
            {
                pokemons.Add(pokemon);
                return Results.Ok("Added to the list successfully!");
            });

            //Digimon Post
            app.MapPost("/digimon", (Digimon digimon) =>
            {
                digimons.Add(digimon);
                return Results.Ok("Added to the list successfully!");
            });

            
            //Uppdaterar pokemon/data 
            app.MapPut("/pokemon/{id}", (int id, Pokemon pokemon) =>
            {
                var pokemonToUpdate = pokemons.Find(p => p.Id == id);
                if (pokemonToUpdate == null)
                {
                    return Results.NotFound("Sorry a Pokemon with this ID does not exist.");
                }

                return Results.Ok("Updated Pokemon successfully!");
            });

            app.MapDelete("/pokemon/{id}", (int id) =>
            {
                var pokemonToRemove = pokemons.Find(p => p.Id == id);
                
                if (pokemonToRemove == null)
                {
                    return Results.NotFound("Sorry, there is no Pokemon at this Id to remove.");
                }

                pokemons.Remove(pokemonToRemove);
                return Results.Ok("Pokemon was removed successfully!");
            });

            //Digimon Update och Delete
            app.MapPut("/digimon/{id}", (int id, Digimon digimon) =>
            {
                var digimonToUpdate = digimons.Find(p => p.Id == id);
                if (digimonToUpdate == null)
                {
                    return Results.NotFound("Sorry a Digimon with this ID does not exist.");
                }

                digimonToUpdate.Name = digimon.Name;
                digimonToUpdate.Type = digimon.Type;

                return Results.Ok("Updated Digimon successfully!");
            });

            app.MapDelete("/digimon/{id}", (int id) =>
            {
                var digimonToRemove = digimons.Find(p => p.Id == id);

                if (digimonToRemove == null)
                {
                    return Results.NotFound("Sorry, there is no Digimon at this Id to remove.");
                }

                digimons.Remove(digimonToRemove);
                return Results.Ok("Digimon was removed successfully!");
            });









            app.Run();
        }
    }
}
