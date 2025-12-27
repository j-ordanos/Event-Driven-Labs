using Microsoft.AspNetCore.Mvc;
using PokemonMinimalControllerAPI.Services;

namespace PokemonMinimalControllerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PokemonController : ControllerBase
{
    private readonly PokemonService _pokemonService;

    public PokemonController(PokemonService pokemonService)
    {
        _pokemonService = pokemonService;
    }
    
    // GET: api/pokemon
    [HttpGet]
    public IActionResult GetAll()
    {
        var pokemons = _pokemonService.GetAll();
        return Ok(pokemons);
    }

    // GET: api/pokemon/{name}
    [HttpGet("{name}")]
    public IActionResult GetByName(string name)
    {
        var pokemon = _pokemonService.GetByName(name);
        
        if (pokemon == null)
        {
            return NotFound($"Pokémon '{name}' not found");
        }
        
        return Ok(pokemon);
    }

    // POST: api/pokemon
    [HttpPost]
    public IActionResult Create([FromBody] Pokemon newPokemon)
    {
        // Check if Pokémon already exists
        if (_pokemonService.Exists(newPokemon.Name))
        {
            return Conflict($"Pokémon '{newPokemon.Name}' already exists");
        }
        
        var pokemon = _pokemonService.Create(newPokemon);
        
        // Return 201 Created with location header
        return CreatedAtAction(
            nameof(GetByName), 
            new { name = pokemon.Name }, 
            pokemon);
    }

    // DELETE: api/pokemon/{name}
    [HttpDelete("{name}")]
    public IActionResult Delete(string name)
    {
        if (!_pokemonService.Exists(name))
        {
            return NotFound($"Pokémon '{name}' not found");
        }
        
        var pokemons = _pokemonService.Delete(name);
        return Ok(pokemons);
    }

    // POST: api/pokemon/{name}/train/{amount}
    [HttpPost("{name}/train/{amount}")]
    public IActionResult Train(string name, int amount)
    {
        var pokemon = _pokemonService.Train(name, amount);
        
        if (pokemon == null)
        {
            return NotFound($"Pokémon '{name}' not found");
        }
        
        return Ok(pokemon);
    }
    
}
