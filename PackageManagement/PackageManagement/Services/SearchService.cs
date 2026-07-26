using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
namespace PackageManagement.Services
{
    public class SearchService
    {

        private readonly SearchClient _searchClient;

        public SearchService(IConfiguration configuration)
        {
            string endpoint =
                configuration["AzureSearch:Endpoint"]!;

            string apiKey =
                configuration["AzureSearch:ApiKey"]!;

            string indexName =
                configuration["AzureSearch:IndexName"]!;

            _searchClient = new SearchClient(
                new Uri(endpoint),
                indexName,
                new AzureKeyCredential(apiKey));
        }


        public async Task<string> SearchDocumentsAsync(string query)
        {
            var results =
                await _searchClient.SearchAsync<SearchDocument>(query);

            string context = "";

            await foreach (var result in results.Value.GetResultsAsync())
            {
                if (result.Document.ContainsKey("chunk"))
                {
                    context += result.Document["chunk"]?.ToString();
                    context += "\n\n";
                }
            }

            return context;
        }
    }
}
