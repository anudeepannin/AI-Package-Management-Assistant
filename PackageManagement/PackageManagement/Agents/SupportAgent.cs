using PackageManagement.Services;

namespace PackageManagement.Agents
{
    public class SupportAgent
    {
        private readonly SearchService _searchService;

        public SupportAgent(
            SearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task<string> ExecuteAsync(
            string question)
        {
            return await _searchService
                .SearchDocumentsAsync(question);
        }
    }
}
