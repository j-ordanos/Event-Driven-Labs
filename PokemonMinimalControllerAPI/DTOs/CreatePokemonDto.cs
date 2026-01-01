using System.ComponentModel.DataAnnotations;

namespace PokemonMinimalControllerAPI.DTOs;

public class CreatePokemonDto
{
    [Required(ErrorMessage = "Pokémon name is required")]
    [MinLength(3, ErrorMessage = "Pokémon name must be at least 3 characters")]
    [MaxLength(50, ErrorMessage = "Pokémon name cannot exceed 50 characters")]
    public string Name { get; set; }
}