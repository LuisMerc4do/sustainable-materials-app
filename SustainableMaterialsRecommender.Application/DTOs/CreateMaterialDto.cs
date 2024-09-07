namespace SustainableMaterialsRecommender.Application.DTOs;

public class CreateMaterialDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double SustainabilityScore { get; set; }
    public string Category { get; set; }
    public string LegalRequirements { get; set; }
    public string ThreeDModelUrl { get; set; }
}