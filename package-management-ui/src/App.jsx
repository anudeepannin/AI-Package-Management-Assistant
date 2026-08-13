import {
  BrowserRouter,
  Routes,
  Route
} from "react-router-dom";

import Layout from "./components/Layout";

import Dashboard from "./pages/Dashboard";
import Chat from "./pages/Chat";
import Packages from "./pages/Packages";
import Renewals from "./pages/Renewals";
import Support from "./pages/Support";
import Compliance from "./pages/Compliance";

function App() {
  return (
    <BrowserRouter>
      <Routes>

        <Route path="/" element={<Layout />}>
          <Route index element={<Dashboard />} />
          <Route path="chat" element={<Chat />} />
          <Route path="packages" element={<Packages />} />
          <Route path="renewals" element={<Renewals />} />
          <Route path="support" element={<Support />} />
          <Route path="compliance" element={<Compliance />} />
        </Route>

      </Routes>
    </BrowserRouter>
  );
}

export default App;