using MediatR;
using Microsoft.AspNetCore.Mvc;
using SaysanPwa.Application.Commands.Job;
using SaysanPwa.Application.DTOs.Jobs;
using SaysanPwa.Application.Query.Jobs;

namespace SaysanPwa.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<GetAllJobsResponseDto>), 200)]
        public async Task<IEnumerable<GetAllJobsResponseDto>> GetAllJobs()
        {
            return await _mediator.Send(new GetAllJobsQuery());
        }

        [HttpPost("{title}")]
        [Produces("application/json")]
        public async Task<bool> AddNewJob([FromRoute] string title)
        {
            if (title != string.Empty)
            {
                var result = await _mediator.Send(new AddNewJobCommand(title));
                return result.Result;
            }
            return false;
        }
    }
}
