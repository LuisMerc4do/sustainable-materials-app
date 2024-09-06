using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SustainableMaterialsRecommender.Application.DTOs;
using SustainableMaterialsRecommender.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace SustainableMaterialsRecommender.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialService _materialService;
        private readonly ILogger<MaterialsController> _logger;

        public MaterialsController(IMaterialService materialService, ILogger<MaterialsController> logger)
        {
            _materialService = materialService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialDto>> GetMaterial(int id)
        {
            try
            {
                var material = await _materialService.GetMaterialByIdAsync(id);
                if (material == null)
                {
                    return NotFound();
                }
                return Ok(material);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting material with id {MaterialId}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialDto>>> GetAllMaterials()
        {
            try
            {
                var materials = await _materialService.GetAllMaterialsAsync();
                return Ok(materials);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all materials");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MaterialDto>> CreateMaterial(CreateMaterialDto createMaterialDto)
        {
            try
            {
                var createdMaterial = await _materialService.CreateMaterialAsync(createMaterialDto);
                return CreatedAtAction(nameof(GetMaterial), new { id = createdMaterial.Id }, createdMaterial);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new material");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterial(int id, UpdateMaterialDto updateMaterialDto)
        {
            try
            {
                await _materialService.UpdateMaterialAsync(id, updateMaterialDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating material with id {MaterialId}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            try
            {
                await _materialService.DeleteMaterialAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting material with id {MaterialId}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}