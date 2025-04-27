namespace MoviesSemanticSearch.Api.Entities
{
    public class MovieEntity
    {
        public required string Title { get; set; }
        public required string ImageUrl { get; set; }
        public required string ReleasedYear { get; set; }
        public required string Overview { get; set; }
        public required float[] Embeddings { get; set; }
    }
}
