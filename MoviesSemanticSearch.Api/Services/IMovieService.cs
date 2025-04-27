namespace MoviesSemanticSearch.Api.Services
{
    public interface IMovieService
    {
        Task GetMoviesAsync(string? term = null, int limit = 10);
    }
}
