using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.VisitPath;
using SaysanPwa.Domain.AggregateModels.VisitPathAggregate;
using System.Globalization;

namespace SaysanPwa.Application.Query.VisitPathQueries;

public record GetPathsForBazaryabQuery(long idBazaryab) : IRequest<IEnumerable<GetPathsForUserResponseDto>>;

public class GetPathsForBazaryabQueryHandler : IRequestHandler<GetPathsForBazaryabQuery, IEnumerable<GetPathsForUserResponseDto>>
{
    private readonly IVisitPathRepository _visitPathRepository;
    private readonly IMapper _mapper;
    public GetPathsForBazaryabQueryHandler(IVisitPathRepository visitPathRepository, IMapper mapper)
    {
        _visitPathRepository = visitPathRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetPathsForUserResponseDto>> Handle(GetPathsForBazaryabQuery request, CancellationToken cancellationToken)
    {
        PersianCalendar pc = new PersianCalendar();
        DateTime now = DateTime.Now;
        string persianDate = $"{pc.GetYear(now)}/{pc.GetMonth(now).ToString("D2")}/{pc.GetDayOfMonth(now).ToString("D2")}";
        IEnumerable<GetVisitPathForUserModel> dbResult = (await _visitPathRepository.GetPathesForUser(request.idBazaryab, persianDate)).Result;
        IEnumerable<GetPathsForUserResponseDto> pathes = _mapper.Map<IEnumerable<GetPathsForUserResponseDto>>(dbResult);
        return pathes;
    }
}
