using System.Collections.Generic;
using System.Linq;

public class PokemonRepository
{
    private List<Pokemon> _pokemons = new List<Pokemon>();
    
    public void Add(Pokemon pokemon)
    {
        _pokemons.Add(pokemon);
    }
    
    public Pokemon? GetByName(string name)
    {
        return _pokemons.FirstOrDefault(p => p.Name == name);
    }
    
    public List<Pokemon> GetAll()
    {
        return _pokemons;
    }
}