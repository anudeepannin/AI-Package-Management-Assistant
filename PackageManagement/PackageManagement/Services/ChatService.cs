using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;

namespace PackageManagement.Services;

public class ChatService
{
    private readonly Kernel _kernel;
    private readonly SearchService _searchService;
    private readonly PackageContextService _packageContextService;
    private readonly ChatHistoryService _chatHistoryService;

    public ChatService(
        Kernel kernel,
        SearchService searchService,
        PackageContextService packageContextService, ChatHistoryService chatHistoryService)
    {
        _kernel = kernel;
        _searchService = searchService;
        _packageContextService = packageContextService;
        _chatHistoryService = chatHistoryService;
    }

    public async Task<string> AskAsync(string question)
    {
        var documentContext =
            await _searchService.SearchDocumentsAsync(question);

        var packageContext =
            _packageContextService.GetPackageInfo(question);

        var chatService =
            _kernel.GetRequiredService<IChatCompletionService>();

        var contextPrompt = $"""
You are a Package Management Assistant.

Use previous conversation history when answering follow-up questions.

Document Information:
{documentContext}

Package Information:
{packageContext}

Current Question:
{question}
""";

        _chatHistoryService.History.AddUserMessage(contextPrompt);

        var settings =
            new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior =
                    FunctionChoiceBehavior.Auto()
            };

        var result =
            await chatService.GetChatMessageContentAsync(
                _chatHistoryService.History,
                settings,
                _kernel);

        _chatHistoryService.History.AddAssistantMessage(
            result.Content ?? string.Empty);

        return result.Content ?? string.Empty;
    }
}