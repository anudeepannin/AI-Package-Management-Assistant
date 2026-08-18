import { useEffect, useState } from "react";
import axios from "axios";

function Support() {

  const [tickets, setTickets] = useState([]);

  useEffect(() => {
    loadTickets();
  }, []);

  const loadTickets = async () => {

    const response = await axios.get(
      "https://packagemanagement-api-hcevhrcwhmbsa6d7.centralus-01.azurewebsites.net/api/support/all"
    );

    setTickets(response.data);
  };

  return (
    <div>

      <h2>Support Tickets</h2>

      <table border="1" cellPadding="10">

        <thead>
          <tr>
            <th>Ticket Id</th>
            <th>Package Id</th>
            <th>Issue</th>
            <th>Status</th>
          </tr>
        </thead>

        <tbody>

          {tickets.map(ticket => (

            <tr key={ticket.ticketId}>

              <td>{ticket.ticketId}</td>
              <td>{ticket.packageId}</td>
              <td>{ticket.issue}</td>
              <td>{ticket.status}</td>

            </tr>

          ))}

        </tbody>

      </table>

    </div>
  );
}

export default Support;