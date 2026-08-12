using Azure.AI.Projects;
using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects.Agents;
using Azure.Identity;
using OpenAI.Responses;

namespace PackageManagement.Services;

#pragma warning disable OPENAI001

public class FoundryAgentService
{
    private readonly AIProjectClient _projectClient;
    private readonly IConfiguration _configuration;

    public FoundryAgentService(
        IConfiguration configuration)
    {
        _configuration = configuration;

        var endpoint =
            _configuration["FoundryAgents:ProjectEndpoint"];
        var tenantId =
            _configuration["FoundryAgents:TenantId"];

        if (string.IsNullOrWhiteSpace(endpoint))
        {
            throw new InvalidOperationException(
                "FoundryAgents:ProjectEndpoint is not configured.");
        }

        if (string.IsNullOrWhiteSpace(tenantId))
        {
            throw new InvalidOperationException(
                "FoundryAgents:TenantId is not configured.");
        }

        var credential = new DefaultAzureCredential(
            new DefaultAzureCredentialOptions
            {
                TenantId = tenantId,
                ExcludeVisualStudioCredential = true,
                ExcludeVisualStudioCodeCredential = true,
                ExcludeInteractiveBrowserCredential = true,
                ExcludeBrokerCredential = true
            });

        _projectClient =
            new AIProjectClient(
                new Uri(endpoint),
                credential);
    }

    public string ExecuteAgent(
        string agentName,
        string version,
        string prompt)
    {
        AgentReference agentReference = new(name: agentName, version: version);
        Console.WriteLine($"Agent={agentName}");
        Console.WriteLine($"Version={version}");
        var responseClient =
            _projectClient.ProjectOpenAIClient
                .GetProjectResponsesClientForAgent(
                    agentReference);

        ResponseResult response =
            responseClient.CreateResponse(
                prompt);
       // return $"Endpoint={_configuration["FoundryAgents:ProjectEndpoint"]}";

        return response.GetOutputText();
    }
}