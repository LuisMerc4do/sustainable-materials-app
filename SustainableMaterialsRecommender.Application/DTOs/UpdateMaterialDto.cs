namespace SustainableMaterialsRecommender.Application.DTOs;

public class UpdateMaterialDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Composition { get; set; }
    public double SustainabilityScore { get; set; }
    public double CarbonFootprint { get; set; }
    public bool IsRecyclable { get; set; }
}
