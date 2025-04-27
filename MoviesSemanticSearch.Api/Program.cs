using MoviesSemanticSearch.Api.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddMilvusClient("milvusdb");

builder.AddOllamaSharpEmbeddingGenerator("embed-model");

builder.Services.AddSingleton<IMovieService, MovieService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/movies", async (IMovieService movieService, string? term = null, int limit = 10) =>
{
    await movieService.GetMoviesAsync(term, limit);

    return Results.Ok();
});

app.Run();

