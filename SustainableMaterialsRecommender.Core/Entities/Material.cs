namespace SustainableMaterialsRecommender.Core.Entities;

public class Material
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public double SustainabilityScore { get; set; }
    public required string Category { get; set; }
    public string? LegalRequirements { get; set; }
    public string? ThreeDModelUrl { get; set; }
}