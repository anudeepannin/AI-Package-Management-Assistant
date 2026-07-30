using PackageManagement.Agents;

namespace PackageManagement.Services
{
    public class AgentOrchestratorService
    {
        private readonly PackageAgent _packageAgent;
        private readonly RenewalAgent _renewalAgent;
        private readonly ComplianceAgent _complianceAgent;
        private readonly SupportAgent _supportAgent;

        public AgentOrchestratorService(
            PackageAgent packageAgent,
            RenewalAgent renewalAgent,
            ComplianceAgent complianceAgent,
            SupportAgent supportAgent)
        {
            _packageAgent = packageAgent;
            _renewalAgent = renewalAgent;
            _complianceAgent = complianceAgent;
            _supportAgent = supportAgent;
        }

        public async Task<string> RouteAsync(string question)
        {
            var q = question.ToLower();

            if (q.Contains("renew"))
            {
                return await _renewalAgent.ExecuteAsync(question);
            }

            if (q.Contains("policy") ||
                q.Contains("compliance"))
            {
                return await _complianceAgent.ExecuteAsync(question);
            }

            if (q.Contains("support") ||
                q.Contains("ticket") ||
                q.Contains("complaint"))
            {
                return await _supportAgent.ExecuteAsync(question);
            }

            return _packageAgent.Execute(question);
        }
    }
}