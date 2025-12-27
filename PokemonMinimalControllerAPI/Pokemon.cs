using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Pokemon
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public string Name { get; set; }
    public int Level { get; set; }
    
    // Constructor for creating new Pokémon
    public Pokemon(string name)
    {
        Name = name;
        Level = 1;
    }
    
    // Empty constructor for MongoDB deserialization
    public Pokemon() { }
    
    public void GainExperience(int amount)
    {
        Level += amount;
    }
}