using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SustainableMaterialsRecommender.Application.DTOs;
using SustainableMaterialsRecommender.Application.Interfaces;
using SustainableMaterialsRecommender.Domain.Entities;
using SustainableMaterialsRecommender.Domain.Interfaces;

namespace SustainableMaterialsRecommender.Application.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IMapper _mapper;
        private readonly ICachingService _cachingService;

        public MaterialService(IMaterialRepository materialRepository, IMapper mapper, ICachingService cachingService)
        {
            _materialRepository = materialRepository;
            _mapper = mapper;
            _cachingService = cachingService;
        }

        public async Task<MaterialDto> GetMaterialByIdAsync(int id)
        {
            string cacheKey = $"material_{id}";
            var cachedMaterial = await _cachingService.GetAsync<MaterialDto>(cacheKey);
            if (cachedMaterial != null)
            {
                return cachedMaterial;
            }

            var material = await _materialRepository.GetByIdAsync(id);
            if (material == null)
            {
                return null;
            }

            var materialDto = _mapper.Map<MaterialDto>(material);
            await _cachingService.SetAsync(cacheKey, materialDto, TimeSpan.FromMinutes(10));
            return materialDto;
        }

        public async Task<IEnumerable<MaterialDto>> GetAllMaterialsAsync()
        {
            string cacheKey = "all_materials";
            var cachedMaterials = await _cachingService.GetAsync<IEnumerable<MaterialDto>>(cacheKey);
            if (cachedMaterials != null)
            {
                return cachedMaterials;
            }

            var materials = await _materialRepository.GetAllAsync();
            var materialDtos = _mapper.Map<IEnumerable<MaterialDto>>(materials);
            await _cachingService.SetAsync(cacheKey, materialDtos, TimeSpan.FromMinutes(10));
            return materialDtos;
        }

        public async Task<MaterialDto> CreateMaterialAsync(CreateMaterialDto createMaterialDto)
        {
            var material = _mapper.Map<Material>(createMaterialDto);
            material.CreatedAt = DateTime.UtcNow;
            material.UpdatedAt = DateTime.UtcNow;

            var createdMaterial = await _materialRepository.AddAsync(material);
            await _cachingService.RemoveAsync("all_materials");
            return _mapper.Map<MaterialDto>(createdMaterial);
        }

        public async Task UpdateMaterialAsync(int id, UpdateMaterialDto updateMaterialDto)
        {
            var existingMaterial = await _materialRepository.GetByIdAsync(id);
            if (existingMaterial == null)
            {
                throw new KeyNotFoundException($"Material with id {id} not found.");
            }

            _mapper.Map(updateMaterialDto, existingMaterial);
            existingMaterial.UpdatedAt = DateTime.UtcNow;

            await _materialRepository.UpdateAsync(existingMaterial);
            await _cachingService.RemoveAsync($"material_{id}");
            await _cachingService.RemoveAsync("all_materials");
        }

        public async Task DeleteMaterialAsync(int id)
        {
            await _materialRepository.DeleteAsync(id);
            await _cachingService.RemoveAsync($"material_{id}");
            await _cachingService.RemoveAsync("all_materials");
        }
    }
}