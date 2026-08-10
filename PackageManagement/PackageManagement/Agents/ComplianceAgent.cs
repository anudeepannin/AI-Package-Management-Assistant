using PackageManagement.Services;

namespace PackageManagement.Agents
{
    public class ComplianceAgent
    {
        private readonly SearchService _searchService;
        private readonly RenewalRequestService _renewalRequestService;

        private readonly PackageContextService _packageContextService;

        public ComplianceAgent(
            SearchService searchService,
            RenewalRequestService renewalRequestService,
            PackageContextService packageContextService)
        {
            _searchService = searchService;
            _renewalRequestService = renewalRequestService;
            _packageContextService = packageContextService;
        }

        //public async Task<string> ExecuteAsync(
        //    string question)
        //{
        //    return await _searchService
        //        .SearchDocumentsAsync(question);
        //}
        public async Task<string> ExecuteAsync(string question)
        {
            // return "COMPLIANCE AGENT HIT";


            if (question.StartsWith(
    "approve",
    StringComparison.OrdinalIgnoreCase))
            {
                var requestId =
                    question.Replace(
                        "Approve",
                        "",
                        StringComparison.OrdinalIgnoreCase)
                    .Trim();

                var request =
                    await _renewalRequestService
                        .GetRequestByIdAsync(requestId);

                if (request == null)
                {
                    return "Request not found.";
                }

                await _renewalRequestService
                    .UpdateStatusAsync(
                        requestId,
                        "Approved");

                _packageContextService
                    .ActivatePackage(
                        request.PackageId);

                return $"""
                            Renewal Request Approved

                            Request Id: {requestId}

                            Package Id: {request.PackageId}

                            Status: Approved

                            Package Status Updated: Active
                            """;
            }

            return "Compliance Agent Ready.";
        }
    }
}
