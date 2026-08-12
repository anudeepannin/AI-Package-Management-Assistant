using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using PackageManagement.Agents;
using PackageManagement.Data;
using PackageManagement.Models;
using PackageManagement.Plugins;
using PackageManagement.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomOperationIds(apiDescription =>
    {
        apiDescription.ActionDescriptor.RouteValues.TryGetValue("action", out var action);
        if (!string.IsNullOrWhiteSpace(action))
        {
            return action;
        }

        var method = apiDescription.HttpMethod?.ToLowerInvariant() ?? "operation";
        var route = apiDescription.RelativePath?
            .Split('?', 2)[0]
            .Replace("/", "_")
            .Replace("{", "")
            .Replace("}", "")
            .Trim('_');

        return string.IsNullOrWhiteSpace(route) ? method : $"{method}_{route}";
    });
});
builder.Services.AddScoped<PackagePlugin>();
builder.Services.AddScoped<PackageContextService>();
builder.Services.AddControllers();
builder.Services.Configure<AzureAIOptions>(builder.Configuration.GetSection("AzureAI"));
builder.Services.AddSingleton<ChatHistoryService>();
builder.Services.AddSingleton<SearchService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<PackageAgent>();
builder.Services.AddScoped<RenewalAgent>();
builder.Services.AddScoped<ComplianceAgent>();
builder.Services.AddScoped<SupportAgent>();
builder.Services.AddScoped<SupportTicketService>();
builder.Services.AddScoped<
    AgentOrchestratorService>();
builder.Services.AddSingleton<UserSessionService>();
builder.Services.AddScoped<RenewalRequestService>();
builder.Services.AddScoped<OwnerAgent>();
builder.Services.AddSingleton<FoundryAgentService>();
var azureAI = builder.Configuration.GetSection("AzureAI");

string endpoint = azureAI["Endpoint"]!;
string apiKey = azureAI["ApiKey"]!;
string deploymentName = azureAI["DeploymentName"]!;

builder.Services.AddScoped<Kernel>(sp =>
{
    var kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName,
        endpoint,
        apiKey);
    var kernel = kernelBuilder.Build();
    // kernel.Plugins.AddFromType<PackagePlugin>();

    var dbContext = sp.GetRequiredService<PackageDbContext>();

    kernel.Plugins.AddFromObject(
        new PackagePlugin(dbContext));
    return kernel;
});
builder.Services.AddDbContext<PackageDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PackageDb")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();
app.UseCors("ReactPolicy");
app.Use(async (context, next) =>
{
    if (!context.Request.Path.Equals(
            "/swagger/v1/swagger.json",
            StringComparison.OrdinalIgnoreCase))
    {
        await next();
        return;
    }

    var responseBody = context.Response.Body;
    await using var buffer = new MemoryStream();
    context.Response.Body = buffer;

    try
    {
        await next();
        buffer.Position = 0;

        using var reader = new StreamReader(buffer);
        var document = await reader.ReadToEndAsync();
        document = document.Replace(
            "\"openapi\": \"3.0.4\"",
            "\"openapi\": \"3.0.1\"",
            StringComparison.Ordinal);

        context.Response.Body = responseBody;
        context.Response.ContentLength = System.Text.Encoding.UTF8.GetByteCount(document);
        await context.Response.WriteAsync(document);
    }
    finally
    {
        context.Response.Body = responseBody;
    }
});
app.UseSwagger(options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
    options.PreSerializeFilters.Add((document, request) =>
    {
        document.Servers =
        [
            new Microsoft.OpenApi.OpenApiServer
            {
                Url = $"{request.Scheme}://{request.Host.Value}"
            }
        ];
    });
});
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}
app.MapControllers();

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


//app.MapPost("/chat", async (ChatRequest request, Kernel kernel) =>
//{
//    var executionSettings = new OpenAIPromptExecutionSettings
//    {
//        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
//    };

//    var result = await kernel.InvokePromptAsync(
//        request.Message,
//        new(executionSettings));

//    return Results.Ok(result.ToString());
//});

//app.MapGet("/packages", async (PackageDbContext db) =>
//{
//    var packages = await db.Packages.ToListAsync();

//    return Results.Ok(packages);
//});

//app.MapPost("/chat",
//async (
//    ChatRequest request,
//    Kernel kernel,
//    ChatHistoryService chatHistoryService) =>
//{
//    var chatService =
//        kernel.GetRequiredService<IChatCompletionService>();

//    chatHistoryService.History.AddUserMessage(
//        request.Message);

//    var executionSettings =
//        new OpenAIPromptExecutionSettings
//        {
//            FunctionChoiceBehavior =
//                FunctionChoiceBehavior.Auto()
//        };

//    var result =
//        await chatService.GetChatMessageContentAsync(
//            chatHistoryService.History,
//            executionSettings,
//            kernel);

//    chatHistoryService.History.AddAssistantMessage(
//        result.Content ?? string.Empty);

//    return Results.Ok(result.Content);
//});

//app.MapPost("/reset-chat",
//(ChatHistoryService chatHistoryService) =>
//{
//    chatHistoryService.History.Clear();

//    return Results.Ok("Chat history cleared");
//});
//app.MapPost("/chat-RAG",
//async (
//    ChatRequest request,
//    Kernel kernel,
//    SearchService searchService) =>
//{
//    var documents =
//        await searchService.SearchDocumentsAsync(
//            request.Message);

//    var prompt = $"""
//    Use the following package management
//    documentation to answer the question.

//    Documentation:
//    {documents}

//    Question:
//    {request.Message}
//    """;

//    var executionSettings =
//        new OpenAIPromptExecutionSettings
//        {
//            FunctionChoiceBehavior =
//                FunctionChoiceBehavior.Auto()
//        };

//    var result =
//        await kernel.InvokePromptAsync(
//            prompt,
//            new(executionSettings));

//    return Results.Ok(result.ToString());
//});

//app.MapPost("/chat-SQL",
//async (
//    ChatRequest request,
//    Kernel kernel,
//    SearchService searchService,
//    PackageContextService packageService) =>
//{
//    var documentContext =
//        await searchService.SearchDocumentsAsync(
//            request.Message);

//    var packageContext =
//        packageService.GetPackageInfo(
//            request.Message);

//    var prompt = $"""
//You are a Package Management Assistant.

//Document Information:
//{documentContext}

//Package Information:
//{packageContext}

//Answer the following question:

//{request.Message}
//""";

//    var executionSettings =
//        new OpenAIPromptExecutionSettings
//        {
//            FunctionChoiceBehavior =
//                FunctionChoiceBehavior.Auto()
//        };

//    var result =
//        await kernel.InvokePromptAsync(
//            prompt,
//            new(executionSettings));

//    return Results.Ok(result.ToString());
//});
app.Run();