using Microsoft.SemanticKernel.ChatCompletion;

namespace PackageManagement.Services
{
    public class ChatHistoryService
    {
        public ChatHistory History { get; } = new();
    }
}
