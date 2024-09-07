using SustainableMaterialsRecommender.Core.Entities;

namespace SustainableMaterialsRecommender.Core.Specifications;

public class MaterialBySearchTermSpecification : BaseSpecification<Material>
{
    public MaterialBySearchTermSpecification(string searchTerm)
        : base(m => m.Name.Contains(searchTerm) || m.Description.Contains(searchTerm))
    {
    }
}