using AutoMapper;
using SaysanPwa.Application.DTOs.VisitPath;
using SaysanPwa.Domain.AggregateModels.VisitPathAggregate;

namespace SaysanPwa.Application.MappingProfiles;

public class VisitPathMappingProfile : Profile
{
    public VisitPathMappingProfile()
    {
        CreateMap<GetVisitPathForUserModel, GetPathsForUserResponseDto>();
    }
}
