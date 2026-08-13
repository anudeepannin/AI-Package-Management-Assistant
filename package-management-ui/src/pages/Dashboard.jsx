import { Grid, Paper, Typography } from "@mui/material";

function Dashboard() {
  return (
    <Grid container spacing={3}>
      <Grid item xs={3}>
        <Paper sx={{ p: 3 }}>
          <Typography variant="h5">
            Active Packages
          </Typography>

          <Typography variant="h3">
            15
          </Typography>
        </Paper>
      </Grid>

      <Grid item xs={3}>
        <Paper sx={{ p: 3 }}>
          <Typography variant="h5">
            Expired Packages
          </Typography>

          <Typography variant="h3">
            3
          </Typography>
        </Paper>
      </Grid>

      <Grid item xs={3}>
        <Paper sx={{ p: 3 }}>
          <Typography variant="h5">
            Open Tickets
          </Typography>

          <Typography variant="h3">
            5
          </Typography>
        </Paper>
      </Grid>

      <Grid item xs={3}>
        <Paper sx={{ p: 3 }}>
          <Typography variant="h5">
            Pending Renewals
          </Typography>

          <Typography variant="h3">
            2
          </Typography>
        </Paper>
      </Grid>
    </Grid>
  );
}

export default Dashboard;