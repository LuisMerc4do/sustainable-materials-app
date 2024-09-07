import React from "react";
import { BrowserRouter as Router, Route, Routes, Link } from "react-router-dom";
import MaterialSearch from "./components/MaterialSearch";
import MaterialDetails from "./components/MaterialDetails";
import MaterialStats from "./components/MaterialStats";
import CreateMaterial from "./components/MaterialCreate";
import { Button } from "./components/ui/button";

const App: React.FC = () => {
  return (
    <Router>
      <div className="container mx-auto p-4">
        <nav className="mb-4">
          <ul className="flex space-x-4">
            <li>
              <Link to="/" className="text-blue-500 hover:text-blue-700">
                Home
              </Link>
            </li>
            <li>
              <Link to="/stats" className="text-blue-500 hover:text-blue-700">
                <Button>Material Stats</Button>
              </Link>
            </li>
            <li>
              <Link to="/create" className="text-blue-500 hover:text-blue-700">
                Create Material
              </Link>
            </li>
          </ul>
        </nav>

        <Routes>
          <Route path="/" element={<MaterialSearch />} />
          <Route path="/material/:id" element={<MaterialDetails />} />
          <Route path="/stats" element={<MaterialStats />} />
          <Route path="/create" element={<CreateMaterial />} />
        </Routes>
      </div>
    </Router>
  );
};

export default App;
