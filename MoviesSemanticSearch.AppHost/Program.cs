using Arshid.Aspire.ApiDocs.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var ollama = builder.AddOllama("ollama").WithGPUSupport();

var embedModel = ollama.AddModel("embed-model", "mxbai-embed-large");

builder.AddProject<Projects.MoviesSemanticSearch_Api>("moviessemanticsearch-api")
    .WithScalar();

builder.Build().Run();
