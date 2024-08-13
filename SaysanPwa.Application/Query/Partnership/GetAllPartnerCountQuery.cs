using MediatR;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;

namespace SaysanPwa.Application.Query.Partnership;

public class GetAllPartnerCountQuery : IRequest<int>
{
    public string? SearchPattern { get; set; }

    public GetAllPartnerCountQuery(string searchPattern)
    {
        SearchPattern = searchPattern;
    }
}

public class GetAllPartnerCountQueryHandler : IRequestHandler<GetAllPartnerCountQuery, int>
{
    private readonly IPartnerRepository _repository;

    public GetAllPartnerCountQueryHandler(IPartnerRepository repository)
    {
        _repository = repository;
    }


    public Task<int> Handle(GetAllPartnerCountQuery request, CancellationToken cancellationToken) => _repository.GetAllPartnerCountAsync(request.SearchPattern, cancellationToken);
}