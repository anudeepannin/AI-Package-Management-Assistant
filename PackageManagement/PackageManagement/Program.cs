using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using PackageManagement.Data;
using PackageManagement.Models;
using PackageManagement.Plugins;

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
    var kernel = kernelBuilder.Build();
    kernel.Plugins.AddFromType<PackagePlugin>();
    return kernel;
});
builder.Services.AddDbContext<PackageDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PackageDb")));

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

app.MapGet("/status", async (Kernel kernel) =>
{
    var plugin = kernel.Plugins["PackagePlugin"];

    var result = await kernel.InvokeAsync(
        plugin["GetPackageStatus"],
        new()
        {
            ["packageId"] = "123"
        });

    return Results.Ok(result.ToString());
});


app.MapPost("/chat", async (ChatRequest request, Kernel kernel) =>
{
    var executionSettings = new OpenAIPromptExecutionSettings
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    };

    var result = await kernel.InvokePromptAsync(
        request.Message,
        new(executionSettings));

    return Results.Ok(result.ToString());
});

app.MapGet("/packages", async (PackageDbContext db) =>
{
    var packages = await db.Packages.ToListAsync();

    return Results.Ok(packages);
});
app.Run();