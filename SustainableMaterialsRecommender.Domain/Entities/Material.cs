namespace SustainableMaterialsRecommender.Domain.Entities
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Composition { get; set; }
        public double SustainabilityScore { get; set; }
        public double CarbonFootprint { get; set; }
        public bool IsRecyclable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}