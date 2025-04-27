using Microsoft.Extensions.AI;

namespace MoviesSemanticSearch.Api.Services
{
    public class MovieService : IMovieService
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;

        public MovieService(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
        {
            _embeddingGenerator = embeddingGenerator;   
        }

        public async Task GetMoviesAsync(string? term = null, int limit = 10)
        {
            var vectorEmbeddings = await GenerateEmbeddings(term);
        }

        private async Task<float[]> GenerateEmbeddings(string term)
        {
            var generatedEmbeddings = await _embeddingGenerator.GenerateAsync([term]);

            var embedding = generatedEmbeddings.Single();

            return embedding.Vector.ToArray();
        }
    }
}
