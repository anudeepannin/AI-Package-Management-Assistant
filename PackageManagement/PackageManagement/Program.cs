using Azure;
using Microsoft.SemanticKernel;
using PackageManagement.Models;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AzureAIOptions>(builder.Configuration.GetSection("AzureAI"));

var azureAI = builder.Configuration.GetSection("AzureAI");

string endpoint = azureAI["Endpoint"]!;
string apiKey = azureAI["ApiKey"]!;
string deploymentName = azureAI["DeploymentName"]!;

builder.Services.AddSingleton<Kernel>(sp =>
{
    var kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName,
        endpoint,
        apiKey);
    return kernelBuilder.Build();

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/ask", async (string question, Kernel kernel) =>
{
    var result = await kernel.InvokePromptAsync(question);

    return Results.Ok(result.ToString());
});

app.Run();