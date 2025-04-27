using CsvHelper;
using MoviesSemanticSearch.Api.Models;
using MoviesSemanticSearch.Api.Services;
using Scalar.AspNetCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddOllamaApiClient("embed-model").AddEmbeddingGenerator();

builder.AddElasticsearchClient(connectionName: "elasticsearch");

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IElasticService, ElasticService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/movies", async (IMovieService movieService, string term, int limit = 10) =>
{
    var movies = await movieService.GetMoviesAsync(term, limit);

    return Results.Ok(movies);
});

app.MapPost("/upload-movies", async (IMovieService movieService, IFormFile file) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("No se ha seleccionado ningún archivo o el archivo está vacío.");
    }

    var movies = new List<Movie>();

    try
    {
        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);

        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            movies = csv.GetRecords<Movie>().ToList();
        }

        await movieService.InsertarMoviesAsync(movies);

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.InternalServerError("Error processing the file: " + ex.Message);
    }
}).DisableAntiforgery();

app.Run();

