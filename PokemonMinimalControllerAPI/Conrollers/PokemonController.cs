using Microsoft.AspNetCore.Mvc;
using PokemonMinimalControllerAPI.Services;
using PokemonMinimalControllerAPI.DTOs;

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
        var response = pokemons.Select(p => new PokemonResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Level = p.Level
        }).ToList();
        
        return Ok(response);
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
        
        var response = new PokemonResponseDto
        {
            Id = pokemon.Id,
            Name = pokemon.Name,
            Level = pokemon.Level
        };
        
        return Ok(response);
    }
    
    // POST: api/pokemon
    [HttpPost]
    public IActionResult Create([FromBody] CreatePokemonDto createDto)
    {
        if (string.IsNullOrEmpty(createDto.Name))
        {
            return BadRequest("Pokémon name is required");
        }
        
        if (_pokemonService.Exists(createDto.Name))
        {
            return Conflict($"Pokémon '{createDto.Name}' already exists");
        }
        
        // Convert DTO to Model
        var newPokemon = new Pokemon(createDto.Name);
        
        var createdPokemon = _pokemonService.Create(newPokemon);
        
        // Convert Model to Response DTO
        var response = new PokemonResponseDto
        {
            Id = createdPokemon.Id,
            Name = createdPokemon.Name,
            Level = createdPokemon.Level
        };
        
        return CreatedAtAction(
            nameof(GetByName), 
            new { name = response.Name }, 
            response);
    }
    
    // DELETE: api/pokemon/{name}
    [HttpDelete("{name}")]
    public IActionResult Delete(string name)
    {
        if (_pokemonService.GetByName(name) == null)
        {
            return NotFound($"Pokémon '{name}' not found");
        }
        
        var remainingPokemons = _pokemonService.Delete(name);
        var response = remainingPokemons.Select(p => new PokemonResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Level = p.Level
        }).ToList();
        
        return Ok(response);
    }
    
    // POST: api/pokemon/{name}/train/{amount}
    [HttpPost("{name}/train/{amount}")]
    public IActionResult Train(string name, int amount)
    {
        if (amount <= 0)
        {
            return BadRequest("Training amount must be positive");
        }
        
        var pokemon = _pokemonService.Train(name, amount);
        
        if (pokemon == null)
        {
            return NotFound($"Pokémon '{name}' not found");
        }
        
        var response = new PokemonResponseDto
        {
            Id = pokemon.Id,
            Name = pokemon.Name,
            Level = pokemon.Level
        };
        
        return Ok(response);
    }
}