using Arshid.Aspire.ApiDocs.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var ollama = builder.AddOllama("ollama").WithGPUSupport()
    .WithLifetime(ContainerLifetime.Persistent);

var embedModel = ollama.AddHuggingFaceModel("embed-model", "limcheekin/snowflake-arctic-embed-l-v2.0-GGUF");

var milvus = builder.AddMilvus("milvus")
    .WithAttu()
    .WithLifetime(ContainerLifetime.Persistent);

var milvusdb = milvus.AddDatabase("milvusdb");

builder.AddProject<Projects.MoviesSemanticSearch_Api>("moviessemanticsearch-api")
    .WithScalar()
    .WithReference(embedModel)
    .WaitFor(embedModel)
    .WithReference(milvusdb)
    .WaitFor(milvusdb);

builder.Build().Run();
