var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres").WithDataVolume(isReadOnly: false);
var pgsql = postgres.AddDatabase("postgres");

var apiService = builder.AddProject<Projects.AspireApp_ApiService>("apiservice")
    .WaitFor(pgsql)
    .WithReference(pgsql)
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.AspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
