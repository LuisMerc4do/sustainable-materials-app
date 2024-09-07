import React, { useState, useEffect } from "react";
import axios from "axios";
import { Card, CardContent, CardHeader, CardTitle } from "./ui/card";
import config from "../libs/config";

interface Stats {
  totalMaterials: number;
  averageSustainabilityScore: number;
  topCategories: { category: string; count: number }[];
}

const MaterialStats: React.FC = () => {
  const [stats, setStats] = useState<Stats | null>(null);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const response = await axios.get(`${config.apiUrl}/materials/stats`);
        setStats(response.data);
      } catch (error) {
        console.error("Error fetching material stats:", error);
      }
    };

    fetchStats();
  }, []);

  if (!stats) {
    return <div>Loading stats...</div>;
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Material Statistics</CardTitle>
      </CardHeader>
      <CardContent>
        <p>
          <strong>Total Materials:</strong> {stats.totalMaterials}
        </p>
        <p>
          <strong>Average Sustainability Score:</strong>{" "}
          {stats.averageSustainabilityScore.toFixed(2)}
        </p>
        <h3 className="mt-4 font-bold">Top Categories:</h3>
        <ul className="list-disc list-inside">
          {stats.topCategories.map((category, index) => (
            <li key={index}>
              {category.category}: {category.count} materials
            </li>
          ))}
        </ul>
      </CardContent>
    </Card>
  );
};

export default MaterialStats;
