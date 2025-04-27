
using CsvHelper.Configuration.Attributes;


namespace MoviesSemanticSearch.Api.Models
{
    public class Movie
    {
        [Name("Series_Title")]
        public required string Title { get; set; }
        
        [Name("Poster_Link")]
        public required string ImageUrl { get; set; }
        
        [Name("Released_Year")]
        public required string ReleasedYear { get; set; }
        
        [Name("Overview")]
        public required string Overview { get; set; }
    }
}
