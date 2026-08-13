import { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function Packages() {
  const [packages, setPackages] = useState([]);
  const navigate = useNavigate();
  useEffect(() => {
    loadPackages();
  }, []);

  const loadPackages = async () => {
    try {
      const response = await axios.get(
        "https://packagemanagement-api-hcevhrcwhmbsa6d7.centralus-01.azurewebsites.net/api/package"
      );

      setPackages(response.data);
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div>
      <h2>Packages</h2>

      <table
        border="1"
        cellPadding="10"
        style={{ width: "100%" }}
      >
        <thead>
          <tr>
            <th>Package Id</th>
            <th>Name</th>
            <th>Owner</th>
            <th>Status</th>
            <th>Actions</th>
          </tr>
        </thead>

        <tbody>
          {packages.map((pkg) => (
            <tr key={pkg.packageId}>
              <td>{pkg.packageId}</td>
              <td>{pkg.packageName}</td>
              <td>{pkg.ownerName}</td>
              <td>{pkg.status}</td>
              <td>
                    <button
                      onClick={() =>
                        navigate(`/chat?packageId=${pkg.packageId}&action=renew`)
                      }
                    >
                      Renew
                    </button>
                    <button
                      onClick={() =>
                        navigate(`/support?packageId=${pkg.packageId}&action=support`)
                      }
                    >
                      Support
                    </button>
                    <button
                      onClick={() =>
                        navigate(`/compliance?packageId=${pkg.packageId}&action=compliance`)
                      }
                    >
                      Compliance
                    </button>
             </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default Packages;