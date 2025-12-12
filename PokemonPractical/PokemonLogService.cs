using System.Collections.Generic;

public class PokemonLogService
{
    private List<PokemonLogEntry> _logs = new List<PokemonLogEntry>();
    
    public void Save(string pokemonName, int gainedLevels)
    {
        var log = new PokemonLogEntry(pokemonName, gainedLevels);
        _logs.Add(log);
    }
    
    public List<PokemonLogEntry> GetAll()
    {
        return _logs;
    }
}
