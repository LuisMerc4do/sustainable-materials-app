using Microsoft.AspNetCore.Mvc;
using Serilog;
using SustainableMaterialsRecommender.Application.DTOs;
using SustainableMaterialsRecommender.Application.Interfaces;
using SustainableMaterialsRecommender.Core.Entities;

namespace SustainableMaterialsRecommender.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialsController : ControllerBase
{
    private readonly IMaterialService _materialService;

    public MaterialsController(IMaterialService materialService)
    {
        _materialService = materialService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaterialDto>>> GetAllMaterials()
    {
        Log.Information("Getting all materials");
        var materials = await _materialService.GetAllMaterialsAsync();
        return Ok(materials);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MaterialDto>> GetMaterial(int id)
    {
        Log.Information("Getting material with id: {MaterialId}", id);
        var material = await _materialService.GetMaterialByIdAsync(id);
        if (material == null)
        {
            Log.Warning("Material with id: {MaterialId} not found", id);
            return NotFound();
        }
        return Ok(material);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<MaterialDto>>> SearchMaterials([FromQuery] string searchTerm)
    {
        Log.Information("Searching materials with term: {SearchTerm}", searchTerm);
        var materials = await _materialService.SearchMaterialsAsync(searchTerm);
        return Ok(materials);
    }

    [HttpPost]
    public async Task<ActionResult<MaterialDto>> CreateMaterial(MaterialDto materialDto)
    {
        try
        {
            var createdMaterial = await _materialService.CreateMaterialAsync(materialDto);
            return CreatedAtAction(nameof(GetMaterial), new { id = createdMaterial.Id }, createdMaterial);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while creating a new material");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterial(int id, MaterialDto materialDto)
    {
        if (id != materialDto.Id)
        {
            Log.Warning("Mismatched id for update material. Path id: {PathId}, DTO id: {DtoId}", id, materialDto.Id);
            return BadRequest();
        }

        Log.Information("Updating material: {@MaterialDto}", materialDto);
        await _materialService.UpdateMaterialAsync(materialDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterial(int id)
    {
        Log.Information("Deleting material with id: {MaterialId}", id);
        await _materialService.DeleteMaterialAsync(id);
        return NoContent();
    }
}