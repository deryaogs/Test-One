using TavernSystem.Models;
using TavernSystem.Application;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("UniversityDatabase");
builder.Services.AddSingleton<ITavernSystemService, TavernSystemService>(containerService => new TavernSystemService(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/GetAllAdventurers", (ITavernSystemService service) =>
{
    try
    {
        return Results.Ok(service.GetAdventurers());
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});
app.MapGet("/api/GetAdventurerById", (ITavernSystemService service, int id) =>
{
    try
    {
        var device = service.GetAdventurer(id);
        if (device is not null)
        {
            return Results.Ok(service.GetAdventurers());
        }
        else
        {
            return Results.NotFound($"Adventurer with ID '{id}' not found.");
        }
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});
app.MapPost("/api/AddAdventurer", (ITavernSystemService service, Adventurer adventurer) =>
{
    try
    {
        var result = service.AddAdventurer(adventurer);
        if (result is true)
        {
            return Results.Created("/api/AddAdventurer", result);
        }
        else
        {
            return Results.BadRequest();
        }
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});



app.Run();