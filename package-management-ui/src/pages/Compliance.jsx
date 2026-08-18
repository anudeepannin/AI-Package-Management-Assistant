import { useEffect, useState } from "react";
import axios from "axios";

function Compliance() {

  const [reports, setReports] = useState([]);

  useEffect(() => {
    loadReports();
  }, []);

  const loadReports = async () => {

    const response = await axios.get(
      "https://packagemanagement-api-hcevhrcwhmbsa6d7.centralus-01.azurewebsites.net/api/compliance/reportAll"
    );

    setReports(response.data);
  };

  return (
    <div>
      <h2>Compliance Reports</h2>

      <table border="1" cellPadding="10">
        <thead>
          <tr>
            <th>Package Id</th>
            <th>Owner</th>
            <th>Status</th>
            <th>Risk</th>
            <th>OpenTickets</th>
            <th>PendingRenewals</th>
          </tr>
        </thead>

        <tbody>
          {reports.map(report => (
            <tr key={report.packageId}>
              <td>{report.packageId}</td>
              <td>{report.owner}</td>
              <td>{report.packageStatus}</td>
              <td>{report.complianceStatus}</td>
              <td>{report.openTickets}</td>
              <td>{report.pendingRenewals}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default Compliance;