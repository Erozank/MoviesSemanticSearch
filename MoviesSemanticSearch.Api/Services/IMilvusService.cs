using MoviesSemanticSearch.Api.Entities;
using MoviesSemanticSearch.Api.Models;

namespace MoviesSemanticSearch.Api.Services
{
    public interface IMilvusService
    {
        Task CreateIndexAsync();
        Task InsertarMoviesAsync(List<MovieEntity> movies);
        Task<List<Movie>> SearchMoviesAsync(float[] queryVector, int limit);
    }
}