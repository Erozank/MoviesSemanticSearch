using Milvus.Client;
using MoviesSemanticSearch.Api.Entities;
using MoviesSemanticSearch.Api.Models;

namespace MoviesSemanticSearch.Api.Services
{
    public class MilvusService : IMilvusService
    {
        private readonly MilvusClient _client;
        private readonly string _collectionName = "movies";
        private readonly ILogger<MilvusService> _logger;

        public MilvusService(MilvusClient client, ILogger<MilvusService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task InsertMoviesAsync(List<MovieEntity> movies)
        {
            // Check if this collection exists
            var hasCollection = await _client.HasCollectionAsync(_collectionName);

            if (!hasCollection)
            {
                _logger.LogInformation("Collection doesn't exist. Creating collection...");
                await CrearColeccionAsync();
            }

            var titles = movies.Select(m => m.Title).ToList();
            var imageUrls = movies.Select(m => m.ImageUrl).ToList();
            var releasedYears = movies.Select(m => m.ReleasedYear.ToString()).ToList();
            var overviews = movies.Select(m => m.Overview).ToList();
            List<ReadOnlyMemory<float>> embeddings = [.. movies.Select(m => new ReadOnlyMemory<float>(m.Embeddings))];

            var collection = _client.GetCollection(_collectionName);

            // Insert movies
            MutationResult result = await collection.InsertAsync(
            [
                FieldData.Create("title", titles),
                FieldData.Create("image_url", imageUrls),
                FieldData.Create("released_year", releasedYears),
                FieldData.Create("overview", overviews),
                FieldData.CreateFloatVector("embeddings", embeddings)
            ]);
            _logger.LogInformation("Movies inserted.");
        }

        private async Task CrearColeccionAsync()
        {
            FieldSchema[] fields =
            [
                FieldSchema.Create<long>("id", isPrimaryKey: true, autoId: true),
                FieldSchema.CreateVarchar("title", 256),
                FieldSchema.CreateVarchar("image_url", 1024),
                FieldSchema.CreateVarchar("released_year", 64),
                FieldSchema.CreateVarchar("overview", 2048),
                FieldSchema.CreateFloatVector("embeddings", 1024)
            ];
            await _client.CreateCollectionAsync(_collectionName, fields);
        }

        public async Task CreateIndexAsync()
        {
            MilvusCollection collection = _client.GetCollection(_collectionName);
            await collection.CreateIndexAsync(
                "embeddings",
                IndexType.Hnsw,
                SimilarityMetricType.L2);

            await collection.LoadAsync();
        }

        public async Task<List<Movie>> SearchMoviesAsync(float[] queryVector, int limit)
        {
            MilvusCollection collection = _client.GetCollection(_collectionName);

            List<string> search_output_fields = ["title", "image_url", "released_year", "overview"];
            SearchResults searchResult = await collection.SearchAsync(
            "embeddings",
            new ReadOnlyMemory<float>[] { queryVector },
            SimilarityMetricType.L2,
            limit: limit);


            List<Movie> movies = [];
            return movies;
        }
    }
}
