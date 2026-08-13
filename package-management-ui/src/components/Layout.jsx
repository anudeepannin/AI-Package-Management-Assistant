import { Link, Outlet } from "react-router-dom";

function Layout() {
  return (
    <div style={{ display: "flex", height: "100vh" }}>
      <div
        style={{
          width: "250px",
          background: "#1976d2",
          color: "white",
          padding: "20px"
        }}
      >
        <h2>Package Management</h2>

        <div><Link to="/" style={{ color: "white" }}>Dashboard</Link></div>
        <div><Link to="/chat" style={{ color: "white" }}>Chat</Link></div>
        <div><Link to="/packages" style={{ color: "white" }}>Packages</Link></div>
        <div><Link to="/renewals" style={{ color: "white" }}>Renewals</Link></div>
        <div><Link to="/support" style={{ color: "white" }}>Support</Link></div>
        <div><Link to="/compliance" style={{ color: "white" }}>Compliance</Link></div>
      </div>

      <div style={{ flex: 1, padding: "20px" }}>
        <Outlet />
      </div>
    </div>
  );
}

export default Layout;