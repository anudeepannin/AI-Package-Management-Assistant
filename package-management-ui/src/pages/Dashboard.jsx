import { Grid, Paper, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import axios from "axios";

function Dashboard() {
  const [dashboard, setDashboard] = useState({
    activePackages: 0,
    expiredPackages: 0,
    openTickets: 0,
    pendingRenewals: 0
  });

  useEffect(() => {
    loadDashboard();
  }, []);

  const loadDashboard = async () => {
    try {
      const response = await axios.get(
        "https://packagemanagement-api-hcevhrcwhmbsa6d7.centralus-01.azurewebsites.net/api/dashboard"
      );

      console.log("Dashboard API:", response.data);

      setDashboard(response.data);
    } catch (error) {
      console.error("Failed to load dashboard:", error);
    }
  };

  return (
    <Grid container spacing={3}>
      <Grid item xs={3}>
        <Paper sx={{ p: 3 }}>
          <Typography variant="h5">
            Active Packages
          </Typography>

          <Typography variant="h3">
            {dashboard.activePackages}
          </Typography>
        </Paper>
      </Grid>

      <Grid item xs={3}>
        <Paper sx={{ p: 3 }}>
          <Typography variant="h5">
            Expired Packages
          </Typography>

          <Typography variant="h3">
            {dashboard.expiredPackages}
          </Typography>
        </Paper>
      </Grid>

      <Grid item xs={3}>
        <Paper sx={{ p: 3 }}>
          <Typography variant="h5">
            Open Tickets
          </Typography>

          <Typography variant="h3">
            {dashboard.openTickets}
          </Typography>
        </Paper>
      </Grid>

      <Grid item xs={3}>
        <Paper sx={{ p: 3 }}>
          <Typography variant="h5">
            Pending Renewals
          </Typography>

          <Typography variant="h3">
            {dashboard.pendingRenewals}
          </Typography>
        </Paper>
      </Grid>
    </Grid>
  );
}

export default Dashboard;