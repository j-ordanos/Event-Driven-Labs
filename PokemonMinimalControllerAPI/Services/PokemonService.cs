using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace PokemonMinimalControllerAPI.Services;

public class PokemonService
{
    private readonly IMongoCollection<Pokemon> _pokemonCollection;
    
    // Constructor with dependency injection
    public PokemonService(IOptions<DBSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _pokemonCollection = database.GetCollection<Pokemon>("Pokemon");
        
        // Seed initial data if collection is empty
        SeedInitialData();
    }
    
    private void SeedInitialData()
    {
        if (_pokemonCollection.CountDocuments(_ => true) == 0)
        {
            var initialPokemons = new List<Pokemon>
            {
                new Pokemon("Pikachu"),
                new Pokemon("Charmander"),
                new Pokemon("Bulbasaur")
            };
            
            _pokemonCollection.InsertMany(initialPokemons);
        }
    }
    
    // Get all Pokémon
    public List<Pokemon> GetAll() => 
        _pokemonCollection.Find(_ => true).ToList();
    
    // Get Pokémon by name
    public Pokemon? GetByName(string name) => 
        _pokemonCollection.Find(p => p.Name == name).FirstOrDefault();
    
    // Create new Pokémon
    public Pokemon Create(Pokemon newPokemon)
    {
        newPokemon.Id = null;

        _pokemonCollection.InsertOne(newPokemon);
        return newPokemon;
    }
    
    // Delete Pokémon by name
    public List<Pokemon> Delete(string name)
    {
        _pokemonCollection.DeleteOne(p => p.Name == name);
        return GetAll();
    }
    
    // Train Pokémon (gain experience)
    public Pokemon? Train(string name, int amount)
    {
        var pokemon = GetByName(name);
        if (pokemon != null)
        {
            pokemon.GainExperience(amount);
            _pokemonCollection.ReplaceOne(p => p.Id == pokemon.Id, pokemon);
        }
        return pokemon;
    }
    
    // Check if Pokémon exists
    public bool Exists(string name) => 
        GetByName(name) != null;
}