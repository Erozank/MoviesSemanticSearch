using MoviesSemanticSearch.Api.Models;

namespace MoviesSemanticSearch.Api.Services
{
    public interface IMovieService
    {
        Task<List<Movie>> GetMoviesAsync(string term, int limit = 10);
        Task InsertarMoviesAsync(List<Movie> movies);
    }
}
