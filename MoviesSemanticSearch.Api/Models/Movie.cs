namespace MoviesSemanticSearch.Api.Models
{
    public class Movie
    {
        public required string Title { get; set; }
        public required string ImageUrl { get; set; }
        public required int ReleasedYear { get; set; }
        public required string Overview { get; set; }
    }
}
