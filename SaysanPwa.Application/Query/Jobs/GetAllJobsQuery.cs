using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Jobs;
using SaysanPwa.Domain.AggregateModels.JobAggregate;

namespace SaysanPwa.Application.Query.Jobs;

public record GetAllJobsQuery : IRequest<IEnumerable<GetAllJobsResponseDto>>;

public class GetAllJobsQueryHandler : IRequestHandler<GetAllJobsQuery, IEnumerable<GetAllJobsResponseDto>>
{
    private readonly IJobRepository _jobRepository;
    private readonly IMapper _mapper;

    public GetAllJobsQueryHandler(IMapper mapper, IJobRepository jobRepository)
    {
        _mapper = mapper;
        _jobRepository = jobRepository;
    }

    public async Task<IEnumerable<GetAllJobsResponseDto>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
    {
        var sysResult = await _jobRepository.GetAllJobs(cancellationToken);
        return _mapper.Map<IEnumerable<GetAllJobsResponseDto>>(sysResult.Result);
    }
}

