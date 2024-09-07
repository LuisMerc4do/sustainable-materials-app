using SustainableMaterialsRecommender.Application.DTOs;

namespace SustainableMaterialsRecommender.Application.Interfaces;

public interface IMaterialService
{
    Task<MaterialDto> GetMaterialByIdAsync(int id);
    Task<IEnumerable<MaterialDto>> GetAllMaterialsAsync();
    Task<IEnumerable<MaterialDto>> SearchMaterialsAsync(string searchTerm);
    Task<MaterialDto> CreateMaterialAsync(MaterialDto materialDto);
    Task UpdateMaterialAsync(MaterialDto materialDto);
    Task DeleteMaterialAsync(int id);
}
