using System;

class Program
{
    static void Main(string[] args)
    {
        var logService = new PokemonLogService();
        var bulbasaur = new Pokemon("Bulbasaur");
        
        bulbasaur.LeveledUp += logService.Save;
        
        bulbasaur.GainExperience(4);
        bulbasaur.GainExperience(2);
        
        foreach (var log in logService.GetAll())
        {
            Console.WriteLine($"{log.Name} gained {log.GainedLevels} at {log.Timestamp}");
        }
    }
}