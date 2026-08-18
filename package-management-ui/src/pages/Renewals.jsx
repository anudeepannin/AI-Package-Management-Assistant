import { useEffect, useState } from "react";
import axios from "axios";

function Renewals() {

  const [renewals, setRenewals] = useState([]);
const approve = async (requestId) => {

  await axios.post(
    `https://packagemanagement-api-hcevhrcwhmbsa6d7.centralus-01.azurewebsites.net/api/renewal/approve?requestId=${requestId}`
  );

  loadRenewals();
};
  useEffect(() => {
    loadRenewals();
  }, []);

  const loadRenewals = async () => {

    const response = await axios.get(
      "https://packagemanagement-api-hcevhrcwhmbsa6d7.centralus-01.azurewebsites.net/api/renewal/all"
    );

    setRenewals(response.data);
  };

  return (
    <div>
      <h2>Renewal Requests</h2>

      <table border="1" cellPadding="10">
        <thead>
          <tr>
            <th>Request Id</th>
            <th>Package Id</th>
            <th>Duration</th>
            <th>Status</th>
          </tr>
        </thead>

        <tbody>
          {renewals.map((item) => (
            <tr key={item.requestId}>
              <td>{item.requestId}</td>
              <td>{item.packageId}</td>
              <td>{item.duration}</td>
              <td
                  style={{
                    color:
                      item.status === "Approved"
                        ? "green"
                        : "orange"
                  }}
                >
                  {item.status}
              </td>
              <td>
                <button
                  onClick={() => approve(item.requestId)}
                >
                  Approve
                </button>
              </td>
            </tr>
          ))}
        </tbody>

      </table>
    </div>
  );
}

export default Renewals;