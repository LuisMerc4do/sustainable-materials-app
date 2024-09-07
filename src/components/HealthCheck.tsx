import React, { useState, useEffect } from "react";
import axios from "axios";
import config from "../libs/config";

const ConnectionTest: React.FC = () => {
  const [status, setStatus] = useState<string>("Loading...");
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const testConnection = async () => {
      try {
        const response = await axios.get(`${config.apiUrl}/healthcheck`);
        setStatus(response.data.message);
      } catch (err) {
        setError("Failed to connect to the backend");
        console.error(err);
      }
    };

    testConnection();
  }, []);

  return (
    <div>
      <h2>Backend Connection Test</h2>
      {error ? <p style={{ color: "red" }}>{error}</p> : <p>{status}</p>}
    </div>
  );
};

export default ConnectionTest;
