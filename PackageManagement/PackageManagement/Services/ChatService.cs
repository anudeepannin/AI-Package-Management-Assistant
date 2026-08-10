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
    private readonly AgentOrchestratorService _agentOrchestrator;
    private readonly UserSessionService _session;

    public ChatService(
        Kernel kernel,
        SearchService searchService,
        PackageContextService packageContextService, ChatHistoryService chatHistoryService, AgentOrchestratorService agentOrchestrator, UserSessionService session)
    {
        _kernel = kernel;
        _searchService = searchService;
        _packageContextService = packageContextService;
        _chatHistoryService = chatHistoryService;
        _agentOrchestrator = agentOrchestrator;
        _session = session;
    }

    public async Task<string> AskAsync(string question)
    {
        // Route question to the appropriate agent
        var agentResponse =
           await _agentOrchestrator.RouteAsync(question);
        //return $"DEBUG CHAT SERVICE => {agentResponse}";

        if (agentResponse.StartsWith("Renewal Request") ||agentResponse.Contains("Approved") ||agentResponse.Contains("Rejected"))
        {
            return agentResponse;
        }

        if (!string.IsNullOrEmpty(_session.ActiveAgent))
        {
            return agentResponse;
        }

    



        var chatService =
            _kernel.GetRequiredService<IChatCompletionService>();

        var prompt = $"""
                            You are a Package Management Assistant.

                            Agent Result:
                            {agentResponse}

                            Use the conversation history when answering follow-up questions.

                            Question:
                            {question}
                            """;

        _chatHistoryService.History.AddUserMessage(prompt);

        var executionSettings =
            new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior =
                    FunctionChoiceBehavior.Auto()
            };

        var result =
            await chatService.GetChatMessageContentAsync(
                _chatHistoryService.History,
                executionSettings,
                _kernel);

        _chatHistoryService.History.AddAssistantMessage(
            result.Content ?? string.Empty);

        return result.Content ?? string.Empty;
    }
}