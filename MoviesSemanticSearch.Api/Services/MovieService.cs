using Microsoft.Extensions.AI;
using MoviesSemanticSearch.Api.Entities;
using MoviesSemanticSearch.Api.Models;

namespace MoviesSemanticSearch.Api.Services
{
    public class MovieService : IMovieService
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
        private readonly IElasticService _elasticService;

        public MovieService(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator, IElasticService elasticService)
        {
            _embeddingGenerator = embeddingGenerator;
            _elasticService = elasticService;
        }

        public async Task<List<Movie>> GetMoviesAsync(string term, int limit = 10)
        {
            var vectorEmbeddings = await GenerateEmbeddings(term);

            var movies = await _elasticService.SearchMoviesAsync(vectorEmbeddings, limit);

            return movies.Select(m => new Movie
            {
                Title = m.Title,
                ImageUrl = m.ImageUrl,
                ReleasedYear = m.ReleasedYear,
                Overview = m.Overview
            }).ToList();
        }

        private async Task<float[]> GenerateEmbeddings(string term)
        {
            var generatedEmbeddings = await _embeddingGenerator.GenerateAsync([term]);

            var embedding = generatedEmbeddings.Single();

            return embedding.Vector.ToArray();
        }

        public async Task InsertarMoviesAsync(List<Movie> movies)
        {
            var tasks = movies.Select(async movie =>
            {
                var embeddings = await GenerateEmbeddings(movie.Overview);

                MovieEntity movieEntity = new()
                {
                    Title = movie.Title,
                    ImageUrl = movie.ImageUrl,
                    ReleasedYear = movie.ReleasedYear,
                    Overview = movie.Overview,
                    Embeddings = embeddings
                };
                return movieEntity;
            });
            await Task.WhenAll(tasks);

            var movieEntities = tasks.Select(t => t.Result).ToList();

            await _elasticService.InsertMoviesAsync(movieEntities);
        }
    }
}
