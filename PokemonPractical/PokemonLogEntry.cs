using System;

public class PokemonLogEntry
{
    public string Name { get; }
    public int GainedLevels { get; }
    public DateTime Timestamp { get; }
    
    public PokemonLogEntry(string name, int gainedLevels)
    {
        Name = name;
        GainedLevels = gainedLevels;
        Timestamp = DateTime.Now;
    }
}