using PackageManagement.Services;

namespace PackageManagement.Agents
{
    public class ComplianceAgent
    {
        private readonly SearchService _searchService;

        public ComplianceAgent(
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
