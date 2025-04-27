using MoviesSemanticSearch.Api.Entities;

namespace MoviesSemanticSearch.Api.Services
{
    public interface IElasticService
    {
        Task InsertMoviesAsync(List<MovieEntity> movies);
        Task<List<MovieEntity>> SearchMoviesAsync(float[] queryEmbedding, int limit = 10);
    }
}