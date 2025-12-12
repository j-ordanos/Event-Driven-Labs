var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Our in-memory Pokémon storage
List<Pokemon> pokemons = new();

// Add some test Pokémon
pokemons.Add(new Pokemon("Pikachu"));
pokemons.Add(new Pokemon("Charmander"));
pokemons.Add(new Pokemon("Bulbasaur"));


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// get all pokemons
app.MapGet("/pokemon", ()=>{
    return Results.Ok(pokemons);
});

// get pokemon by name
app.MapGet("/pokemon/{name}", (string name) => {
    var pokemon = pokemons.FirstOrDefault(p => p.Name == name);
    
    if (pokemon == null)
    {
        return Results.NotFound($"Pokémon '{name}' not found");
    }
    
    return Results.Ok(pokemon);
});

// Add a new Pokémon
app.MapPost("/pokemon", (Pokemon newPokemon) => {
    // Check if Pokémon already exists
    if (pokemons.Any(p => p.Name == newPokemon.Name))
    {
        return Results.Conflict($"Pokémon '{newPokemon.Name}' already exists");
    }
    
    // Add to our list
    pokemons.Add(newPokemon);
    
    // Return 201 Created with location header
    return Results.Created($"/pokemon/{newPokemon.Name}", newPokemon);
});

// Delete a Pokémon by name
app.MapDelete("/pokemon/{name}", (string name) => {
    var pokemon = pokemons.FirstOrDefault(p => p.Name == name);
    
    if (pokemon == null)
    {
        return Results.NotFound($"Pokémon '{name}' not found");
    }
    
    // Remove from list
    pokemons.Remove(pokemon);
    
    // Return success with remaining Pokémon
    return Results.Ok(pokemons);
});

// Train a Pokémon (gain experience)
app.MapPost("/pokemon/{name}/train/{amount}", (string name, int amount) => {
    var pokemon = pokemons.FirstOrDefault(p => p.Name == name);
    
    if (pokemon == null)
    {
        return Results.NotFound($"Pokémon '{name}' not found");
    }
    
    // Use the GainExperience method from our Pokemon class
    pokemon.GainExperience(amount);
    
    // Return the updated Pokémon
    return Results.Ok(pokemon);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
