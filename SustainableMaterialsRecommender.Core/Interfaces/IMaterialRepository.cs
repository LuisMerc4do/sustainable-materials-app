using SustainableMaterialsRecommender.Core.Entities;

namespace SustainableMaterialsRecommender.Core.Interfaces;

public interface IMaterialRepository
{
    Task<Material> GetByIdAsync(int id);
    Task<IEnumerable<Material>> ListAllAsync();
    Task<IEnumerable<Material>> ListAsync(ISpecification<Material> spec);
    Task<Material> AddAsync(Material entity);
    Task UpdateAsync(Material entity);
    Task DeleteAsync(Material entity);
}
