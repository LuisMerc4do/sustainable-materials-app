import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { Button } from "./ui/button";
import { Input } from "./ui/input";
import { Card, CardHeader, CardTitle, CardContent } from "./ui/card";
import { Textarea } from "./ui/textarea";

const CreateMaterial: React.FC = () => {
  const navigate = useNavigate();
  const [material, setMaterial] = useState({
    name: "",
    description: "",
    sustainabilityScore: 0,
    category: "",
    legalRequirements: "",
    threeDModelUrl: "",
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setMaterial((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.post(
        `https://sustainablematerialsapp-cbdackd0dgd7cehx.australiaeast-01.azurewebsites.net/api/materials`,
        material
      );
      navigate("/");
    } catch (error) {
      console.error("Error creating material:", error);
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Create New Material</CardTitle>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit}>
          <div className="space-y-4">
            <Input
              name="name"
              value={material.name}
              onChange={handleChange}
              placeholder="Material Name"
              required
            />
            <Textarea
              name="description"
              value={material.description}
              onChange={handleChange}
              placeholder="Description"
              required
            />
            <Input
              type="number"
              name="sustainabilityScore"
              value={material.sustainabilityScore}
              onChange={handleChange}
              placeholder="Sustainability Score"
              required
            />
            <Input
              name="category"
              value={material.category}
              onChange={handleChange}
              placeholder="Category"
              required
            />
            <Textarea
              name="legalRequirements"
              value={material.legalRequirements}
              onChange={handleChange}
              placeholder="Legal Requirements"
            />
            <Input
              name="threeDModelUrl"
              value={material.threeDModelUrl}
              onChange={handleChange}
              placeholder="3D Model URL"
            />
            <Button type="submit">Create Material</Button>
          </div>
        </form>
      </CardContent>
    </Card>
  );
};

export default CreateMaterial;
