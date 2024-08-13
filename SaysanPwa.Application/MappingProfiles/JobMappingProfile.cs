using AutoMapper;
using SaysanPwa.Application.DTOs.Jobs;
using SaysanPwa.Application.DTOs.Partner;
using SaysanPwa.Domain.AggregateModels.JobAggregate;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;

namespace SaysanPwa.Application.MappingProfiles
{
    public class JobMappingProfile : Profile
    {
        public JobMappingProfile()
        {
            CreateMap<Job, GetAllJobsResponseDto>();
        }
    }
}
