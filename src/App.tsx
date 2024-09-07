import React from "react";
import { BrowserRouter as Router, Route, Routes, Link } from "react-router-dom";
import MaterialSearch from "./components/MaterialSearch";
import MaterialDetails from "./components/MaterialDetails";
import MaterialStats from "./components/MaterialStats";
import CreateMaterial from "./components/MaterialCreate";
import { Button } from "./components/ui/button";
import ConnectionTest from "./components/HealthCheck";

const App: React.FC = () => {
  return (
    <Router>
      <div className="container mx-auto p-4">
        <nav className="mb-4">
          <ul className="flex space-x-4">
            <li>
              <Link to="/" className="text-blue-500 hover:text-blue-700">
                <Button>Home</Button>
              </Link>
            </li>
            <li>
              <Link to="/stats" className="text-blue-500 hover:text-blue-700">
                <Button>Material Stats</Button>
              </Link>
            </li>
            <li>
              <Link to="/create" className="text-blue-500 hover:text-blue-700">
                <Button className="text-blue-600">Create Material</Button>
              </Link>
            </li>
            <li>
              <Link
                to="/healthcheck"
                className="text-blue-500 hover:text-blue-700"
              >
                HealthCheck
              </Link>
            </li>
          </ul>
        </nav>

        <Routes>
          <Route path="/" element={<MaterialSearch />} />
          <Route path="/material/:id" element={<MaterialDetails />} />
          <Route path="/stats" element={<MaterialStats />} />
          <Route path="/create" element={<CreateMaterial />} />
          <Route path="/healthcheck" element={<ConnectionTest />} />
        </Routes>
      </div>
    </Router>
  );
};

export default App;
