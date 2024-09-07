import React, { useState } from "react";
import axios from "axios";
import { Input } from "./ui/input";
import { Button } from "./ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "./ui/card";

interface Material {
  id: number;
  name: string;
  description: string;
  sustainabilityScore: number;
  category: string;
}

const MaterialSearch: React.FC = () => {
  const [searchTerm, setSearchTerm] = useState("");
  const [materials, setMaterials] = useState<Material[]>([]);

  const handleSearch = async () => {
    try {
      const API_URL = process.env.REACT_APP_API_URL;
      const response = await axios.get(
        `https://sustainablematerialsapp-cbdackd0dgd7cehx.australiaeast-01.azurewebsites.net/api/materials/search?searchTerm=${searchTerm}`
      );
      setMaterials(response.data);
    } catch (error) {
      console.error("Error searching materials:", error);
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Search Materials</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="flex space-x-2">
          <Input
            type="text"
            value={searchTerm}
            onChange={(e: {
              target: { value: React.SetStateAction<string> };
            }) => setSearchTerm(e.target.value)}
            placeholder="Enter search term"
          />
          <Button onClick={handleSearch}>Search</Button>
        </div>
        <ul className="mt-4 space-y-2">
          {materials.map((material) => (
            <li key={material.id} className="p-2 bg-gray-100 rounded">
              {material.name} - Sustainability Score:{" "}
              {material.sustainabilityScore}
            </li>
          ))}
        </ul>
      </CardContent>
    </Card>
  );
};

export default MaterialSearch;
