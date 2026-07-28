using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace PackageManagement.Services;

public class ChatService
{
    private readonly Kernel _kernel;
    private readonly SearchService _searchService;
    private readonly PackageContextService _packageService;

    public ChatService(
        Kernel kernel,
        SearchService searchService,
        PackageContextService packageService)
    {
        _kernel = kernel;
        _searchService = searchService;
        _packageService = packageService;
    }

    public async Task<string> ChatWithSqlAsync(
        string message)
    {
        var documentContext =
            await _searchService
                .SearchDocumentsAsync(message);

        var packageContext =
            _packageService
                .GetPackageInfo(message);

        var prompt = $"""
        Document Information:
        {documentContext}

        Package Information:
        {packageContext}

        Question:
        {message}
        """;

        var result =
            await _kernel.InvokePromptAsync(
                prompt,
                new(
                    new OpenAIPromptExecutionSettings
                    {
                        FunctionChoiceBehavior =
                            FunctionChoiceBehavior.Auto()
                    }));

        return result.ToString();
    }
}