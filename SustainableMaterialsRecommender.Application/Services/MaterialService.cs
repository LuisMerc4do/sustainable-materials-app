using AutoMapper;
using Serilog;
using SustainableMaterialsRecommender.Application.DTOs;
using SustainableMaterialsRecommender.Application.Interfaces;
using SustainableMaterialsRecommender.Core.Entities;
using SustainableMaterialsRecommender.Core.Interfaces;
using SustainableMaterialsRecommender.Core.Specifications;
namespace SustainableMaterialsRecommender.Application.Services;

public class MaterialService : IMaterialService
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IMapper _mapper;
    private readonly TimeProvider _timeProvider;

    public MaterialService(IMaterialRepository materialRepository, IMapper mapper, TimeProvider timeProvider)
    {
        _materialRepository = materialRepository;
        _mapper = mapper;
        _timeProvider = timeProvider;
    }

    public async Task<MaterialDto> GetMaterialByIdAsync(int id)
    {
        Log.Information("Getting material with id: {MaterialId}", id);
        var material = await _materialRepository.GetByIdAsync(id);
        if (material == null)
        {
            Log.Warning("Material with id: {MaterialId} not found", id);
            return null;
        }
        return _mapper.Map<MaterialDto>(material);
    }

    public async Task<IEnumerable<MaterialDto>> GetAllMaterialsAsync()
    {
        Log.Information("Getting all materials");
        var materials = await _materialRepository.ListAllAsync();
        return _mapper.Map<IEnumerable<MaterialDto>>(materials);
    }

    public async Task<IEnumerable<MaterialDto>> SearchMaterialsAsync(string searchTerm)
    {
        Log.Information("Searching materials with term: {SearchTerm}", searchTerm);
        var spec = new MaterialBySearchTermSpecification(searchTerm);
        var materials = await _materialRepository.ListAsync(spec);
        return _mapper.Map<IEnumerable<MaterialDto>>(materials);
    }

    public async Task<MaterialDto> CreateMaterialAsync(MaterialDto materialDto)
    {
        Log.Information("Creating new material: {@MaterialDto}", materialDto);
        var material = _mapper.Map<Material>(materialDto);
        var createdMaterial = await _materialRepository.AddAsync(material);
        return _mapper.Map<MaterialDto>(createdMaterial);
    }

    public async Task UpdateMaterialAsync(MaterialDto materialDto)
    {
        Log.Information("Updating material: {@MaterialDto}", materialDto);
        var material = _mapper.Map<Material>(materialDto);
        await _materialRepository.UpdateAsync(material);
    }

    public async Task DeleteMaterialAsync(int id)
    {
        Log.Information("Deleting material with id: {MaterialId}", id);
        var material = await _materialRepository.GetByIdAsync(id);
        if (material != null)
        {
            await _materialRepository.DeleteAsync(material);
        }
        else
        {
            Log.Warning("Attempted to delete non-existent material with id: {MaterialId}", id);
        }
    }
}