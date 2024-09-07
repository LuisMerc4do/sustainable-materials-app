import React, { useState, useEffect } from "react";
import axios from "axios";

const ConnectionTest = () => {
  const [status, setStatus] = useState("Loading...");
  const [error, setError] = useState(null);

  useEffect(() => {
    const testConnection = async () => {
      try {
        const response = await axios.get(
          `https://sustainablematerialsbackend-evfxhfbeghczayds.australiasoutheast-01.azurewebsites.net/api/healthcheck`
        );
        setStatus(response.data.message);
      } catch (err) {
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
