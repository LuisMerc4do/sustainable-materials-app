import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import MaterialSearch from "./components/MaterialSearch";
import MaterialDetails from "./components/MaterialDetails";
import MaterialStats from "./components/MaterialStats";

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
                Material Stats Test
              </Link>
            </li>
          </ul>
        </nav>

        <Routes>
          <Route path="/" element={<MaterialSearch />} />
          <Route path="/material/:id" element={<MaterialDetails />} />
          <Route path="/stats" element={<MaterialStats />} />
        </Routes>
      </div>
    </Router>
  );
};

export default App;
