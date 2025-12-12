public class PokemonService
{
    private PokemonRepository _repository = new PokemonRepository();
    
    public void RegisterPokemon(string name)
    {
        var pokemon = new Pokemon(name);
        _repository.Add(pokemon);
    }
    
    public void Train(string pokemonName, int experience)
    {
        var pokemon = _repository.GetByName(pokemonName);
        if (pokemon != null)
        {
            pokemon.GainExperience(experience);
        }
    }
    
    public List<Pokemon> GetPokemons()
    {
        return _repository.GetAll();
    }
}