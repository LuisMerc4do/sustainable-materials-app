using System.Collections.Generic;
using System.Threading.Tasks;
using SustainableMaterialsRecommender.Domain.Entities;

namespace SustainableMaterialsRecommender.Domain.Interfaces
{
    public interface IMaterialRepository
    {
        Task<Material> GetByIdAsync(int id);
        Task<IEnumerable<Material>> GetAllAsync();
        Task<Material> AddAsync(Material material);
        Task UpdateAsync(Material material);
        Task DeleteAsync(int id);
    }
}