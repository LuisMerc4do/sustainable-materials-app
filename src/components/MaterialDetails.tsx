import React, { useState, useEffect } from "react";
import axios from "axios";
import { useParams } from "react-router-dom";
import { Card, CardContent, CardHeader, CardTitle } from "./ui/card";
import ThreeDViewer from "./ThreeDViewer";

interface Material {
  id: number;
  name: string;
  description: string;
  sustainabilityScore: number;
  category: string;
  legalRequirements: string;
  threeDModelUrl: string;
}

const MaterialDetails: React.FC = () => {
  const [material, setMaterial] = useState<Material | null>(null);
  const { id } = useParams<{ id: string }>();

  useEffect(() => {
    const fetchMaterial = async () => {
      try {
        const response = await axios.get(`/api/materials/${id}`);
        setMaterial(response.data);
      } catch (error) {
        console.error("Error fetching material details:", error);
      }
    };

    fetchMaterial();
  }, [id]);

  if (!material) {
    return <div>Loading...</div>;
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>{material.name}</CardTitle>
      </CardHeader>
      <CardContent>
        <p>
          <strong>Description:</strong> {material.description}
        </p>
        <p>
          <strong>Sustainability Score:</strong> {material.sustainabilityScore}
        </p>
        <p>
          <strong>Category:</strong> {material.category}
        </p>
        <p>
          <strong>Legal Requirements:</strong> {material.legalRequirements}
        </p>
        {material.threeDModelUrl && (
          <div className="mt-4">
            <h3 className="text-lg font-semibold mb-2">3D Model</h3>
            <ThreeDViewer modelUrl={material.threeDModelUrl} />
          </div>
        )}
      </CardContent>
    </Card>
  );
};

export default MaterialDetails;
