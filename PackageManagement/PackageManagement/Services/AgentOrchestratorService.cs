using PackageManagement.Agents;

namespace PackageManagement.Services
{
    public class AgentOrchestratorService
    {
        private readonly PackageAgent _packageAgent;
        private readonly RenewalAgent _renewalAgent;
        private readonly ComplianceAgent _complianceAgent;
        private readonly SupportAgent _supportAgent;
        private readonly UserSessionService _session;

        public AgentOrchestratorService(
            PackageAgent packageAgent,
            RenewalAgent renewalAgent,
            ComplianceAgent complianceAgent,
            SupportAgent supportAgent,
            UserSessionService session)
        {
            _packageAgent = packageAgent;
            _renewalAgent = renewalAgent;
            _complianceAgent = complianceAgent;
            _supportAgent = supportAgent;
            _session = session;
        }

        public async Task<string> RouteAsync(string question)
        {
            if (_session.ActiveAgent == "Renewal")
            {
                if (_session.RenewalStep == "Duration")
                {
                    switch (question)
                    {
                        case "1":
                            _session.SelectedDuration = "1 Year";
                            break;

                        case "2":
                            _session.SelectedDuration = "2 Years";
                            break;

                        case "3":
                            _session.SelectedDuration = "3 Years";
                            break;

                        default:
                            return """
                                        Invalid selection.

                                        Choose:

                                        1. One Year
                                        2. Two Years
                                        3. Three Years
                                        """;
                    }

                    _session.RenewalStep = "Confirmation";

                    return $"""
                                Renewal Request Summary

                                Package Id: {_session.CurrentPackageId}
                                Duration: {_session.SelectedDuration}

                                Proceed?

                                1. Submit
                                2. Cancel
                                """;
                }

                if (_session.RenewalStep == "Confirmation")
                {
                    if (question == "1")
                    {
                        var requestId =
                            $"REN-{DateTime.Now:yyyyMMddHHmmss}";

                        _session.ActiveAgent = null;
                        _session.RenewalStep = null;

                        return $"""
                                    Renewal Request Created

                                    Request Id: {requestId}

                                    Package Id: {_session.CurrentPackageId}

                                    Duration: {_session.SelectedDuration}

                                    Status: Pending Approval

                                    Renewal team will review the request.
                                    """;
                    }

                    if (question == "2")
                    {
                        _session.ActiveAgent = null;
                        _session.RenewalStep = null;

                        return """
                                    Renewal Request Cancelled.
                                    """;
                                            }

                      return """
                                Invalid option.

                                1. Submit
                                2. Cancel
                                """;
                }
            }

            var q = question.ToLower();

            if (_session.CurrentMenu == "AgentSelection")
            {
                switch (q)
                {
                    case "1":
                        _session.ActiveAgent = "Renewal";
                        _session.CurrentMenu = null;
                        _session.RenewalStep = "Duration";

                        return """
                               Renewal Agent

                               Choose duration:

                               1. One Year
                               2. Two Years
                               3. Three Years
                               """;

                    case "2":
                        _session.ActiveAgent = "Compliance";
                        _session.CurrentMenu = null;

                        return await _complianceAgent.ExecuteAsync(
                            $"Package {_session.CurrentPackageId}");

                    case "3":
                        _session.ActiveAgent = "Support";
                        _session.CurrentMenu = null;

                        return await _supportAgent.ExecuteAsync(
                            $"Package {_session.CurrentPackageId}");

                    case "4":
                        _session.ActiveAgent = "Owner";
                        _session.CurrentMenu = null;

                        return $"Owner details for Package {_session.CurrentPackageId} coming next.";
                }
            }

            

            var packageResponse =
                _packageAgent.Execute(question);

            if (packageResponse.Contains(
                "Expired",
                StringComparison.OrdinalIgnoreCase))
            {
                _session.CurrentMenu = "AgentSelection";
                return $"""
                        {packageResponse}

                        Available Actions:

                        1. Renewal Agent
                        2. Compliance Agent
                        3. Support Agent
                        4. Package Owner Details

                        Please select an option.
                        """;
                                    }

            return packageResponse;
        }

    }
}