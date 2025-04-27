using Arshid.Aspire.ApiDocs.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var ollama = builder.AddOllama("ollama").WithGPUSupport()
    .WithLifetime(ContainerLifetime.Persistent);

var embedModel = ollama.AddHuggingFaceModel("embed-model", "limcheekin/snowflake-arctic-embed-l-v2.0-GGUF");

var elasticsearch = builder.AddElasticsearch("elasticsearch")
    .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.MoviesSemanticSearch_Api>("moviessemanticsearch-api")
    .WithScalar()
    .WithReference(embedModel)
    .WaitFor(embedModel)
    .WithReference(elasticsearch)
    .WaitFor(elasticsearch);

builder.Build().Run();
