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

        //public async Task<string> ExecuteAsync(
        //    string question)
        //{
        //    return await _searchService
        //        .SearchDocumentsAsync(question);
        //}

        public Task<string> ExecuteAsync(string question)
        {
            return Task.FromResult(
                """
                Renewal Agent

                Package selected for renewal.

                Choose duration:

                1. One Year
                2. Two Years
                3. Three Years
                """
            );
        }
    }
}
