using AutoMapper;
using SustainableMaterialsRecommender.Core.Entities;
using SustainableMaterialsRecommender.Application.DTOs;

namespace SustainableMaterialsRecommender.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Material, MaterialDto>().ReverseMap();
    }
}