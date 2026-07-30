using PackageManagement.Services;

namespace PackageManagement.Agents
{
    public class RenewalAgent
    {
        private readonly SearchService _searchService;

        public RenewalAgent(
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
