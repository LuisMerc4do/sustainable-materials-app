using System.Collections.Generic;
using System.Threading.Tasks;
using SustainableMaterialsRecommender.Application.DTOs;

namespace SustainableMaterialsRecommender.Application.Interfaces
{
    public interface IMaterialService
    {
        Task<MaterialDto> GetMaterialByIdAsync(int id);
        Task<IEnumerable<MaterialDto>> GetAllMaterialsAsync();
        Task<MaterialDto> CreateMaterialAsync(CreateMaterialDto createMaterialDto);
        Task UpdateMaterialAsync(int id, UpdateMaterialDto updateMaterialDto);
        Task DeleteMaterialAsync(int id);
    }
}