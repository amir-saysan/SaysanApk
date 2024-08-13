using MediatR;
using SaysanPwa.Domain.AggregateModels.JobAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Commands.Job;

public class AddNewJobCommand : IRequest<SysResult<bool>>
{
    public string JobTitle { get; set; } = string.Empty;

    public AddNewJobCommand(string jobTitle) => JobTitle = jobTitle;
}

public class AddNewJobCommandHandler : IRequestHandler<AddNewJobCommand, SysResult<bool>>
{
    private readonly IJobRepository _repository;

    public AddNewJobCommandHandler(IJobRepository repostiory) => _repository = repostiory;

    public async Task<SysResult<bool>> Handle(AddNewJobCommand request, CancellationToken cancellationToken) =>
        await _repository.AddNewJob(request.JobTitle, cancellationToken);
}