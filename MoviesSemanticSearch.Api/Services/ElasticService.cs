using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using MoviesSemanticSearch.Api.Entities;

namespace MoviesSemanticSearch.Api.Services
{
    public class ElasticService : IElasticService
    {
        private readonly ElasticsearchClient _client;
        private readonly ILogger<ElasticService> _logger;

        public ElasticService(ElasticsearchClient client, ILogger<ElasticService> logger)
        {
            _client = client;
            _logger = logger;
        }

        private async Task CreateIndex()
        {
            var existsResponse = await _client.Indices.ExistsAsync("movies");
            if (!existsResponse.Exists)
            {
                var createIndexRequest = new CreateIndexRequest("movies")
                {
                    Mappings = new TypeMapping
                    {
                        Properties = new Properties
                        {
                            { "title", new TextProperty() },
                            { "overview", new TextProperty() },
                            { "releasedYear", new TextProperty() },
                            { "imageUrl", new KeywordProperty() },
                            { "embeddings", new DenseVectorProperty { Dims = 1024 } }
                        }
                    }
                };

                var createIndexResponse = await _client.Indices.CreateAsync(createIndexRequest);
                if (!createIndexResponse.IsValidResponse)
                {
                    _logger.LogError("Error creating index: {Errors}", createIndexResponse.ElasticsearchServerError);
                    throw new Exception("Error creating index");
                }
                _logger.LogInformation("Index created successfully");
            }
        }

        public async Task InsertMoviesAsync(List<MovieEntity> movies)
        {
            await CreateIndex();

            var bulkResponse = await _client.BulkAsync(b => b
                .Index("movies")
                .IndexMany(movies)
            );

            if (bulkResponse.Errors)
            {
                _logger.LogError("Error inserting movies into Elasticsearch: {Errors}", bulkResponse.ItemsWithErrors);
                throw new Exception("Error inserting movies into Elasticsearch");
            }

            _logger.LogInformation("Inserted {Count} movies into Elasticsearch", bulkResponse.Items.Count);
        }

        public async Task<List<MovieEntity>> SearchMoviesAsync(float[] queryEmbedding, int limit = 10)
        {
            var searchResponse = await _client.SearchAsync<MovieEntity>(s => s
                .Index("movies")
                .Knn(knn => knn
                    .Field(f => f.Embeddings)
                    .QueryVector(queryEmbedding)
                    .k(limit) // número de resultados que quieres
                    .NumCandidates(200) // número de candidatos a considerar (tradeoff speed/accuracy)
                )
            );

            if (!searchResponse.IsValidResponse)
            {
                _logger.LogError("Error searching movies: {Errors}", searchResponse.ElasticsearchServerError);
                throw new Exception("Error searching movies");
            }
            return searchResponse.Documents.ToList();
        }
    }
}
