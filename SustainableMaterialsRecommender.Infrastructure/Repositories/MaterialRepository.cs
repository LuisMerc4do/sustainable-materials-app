using Microsoft.EntityFrameworkCore;
using Serilog;
using SustainableMaterialsRecommender.Core.Entities;
using SustainableMaterialsRecommender.Core.Interfaces;
using SustainableMaterialsRecommender.Infrastructure.Data;

namespace SustainableMaterialsRecommender.Infrastructure.Repositories;

public class MaterialRepository : IMaterialRepository
{
    private readonly AppDbContext _context;

    public MaterialRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Material> GetByIdAsync(int id)
    {
        Log.Information("Getting material by id: {MaterialId}", id);
        return await _context.Materials.FindAsync(id);
    }

    public async Task<IEnumerable<Material>> ListAllAsync()
    {
        Log.Information("Listing all materials");
        return await _context.Materials.ToListAsync();
    }

    public async Task<IEnumerable<Material>> ListAsync(ISpecification<Material> spec)
    {
        Log.Information("Listing materials with specification: {Specification}", spec.GetType().Name);
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<Material> AddAsync(Material entity)
    {
        Log.Information("Adding new material: {@Material}", entity);
        _context.Materials.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Material entity)
    {
        Log.Information("Updating material: {@Material}", entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Material entity)
    {
        Log.Information("Deleting material: {@Material}", entity);
        _context.Materials.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private IQueryable<Material> ApplySpecification(ISpecification<Material> spec)
    {
        return SpecificationEvaluator<Material>.GetQuery(_context.Set<Material>().AsQueryable(), spec);
    }
}