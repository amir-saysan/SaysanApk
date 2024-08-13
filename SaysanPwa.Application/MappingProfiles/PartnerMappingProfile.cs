using AutoMapper;
using SaysanPwa.Application.Commands.Partnership;
using SaysanPwa.Application.DTOs.Partner;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;

namespace SaysanPwa.Application.MappingProfiles;

public class PartnerMappingProfile : Profile
{
    public PartnerMappingProfile()
    {
        CreateMap<Partner, GetPartnersResponseDto>();
        CreateMap<PartnerGroup, GetAllPartnerGroupsResponseDto>();
        CreateMap<PartnerDetailViewModel, PartnerDetailDto>();
        CreateMap<EditPartnerRequestDto, EditPartnerCommand>();
        CreateMap<EditPartnerCommand, Partner>();

        CreateMap<PartnerDetailDto, EditPartnerRequestDto>();

        CreateMap<AddBranchDto, Branch>();
        CreateMap<Branch, EditBranchDto>().ReverseMap();

        CreateMap<PartnerDetailViewModel, GetPartnersForFactorResponseDto>();
        CreateMap<Branch, GetPartnerBranchesForFactorResponseDto>();
    }
}
