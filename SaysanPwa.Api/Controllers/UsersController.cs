using MediatR;
using Microsoft.AspNetCore.Mvc;
using SaysanPwa.Api.Logging;
using SaysanPwa.Application.DTOs.User;
using SaysanPwa.Application.Query;

namespace SaysanPwa.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    private IMediator _mediator;
    private ILoggerService _logger;


    public UsersController(IMediator mediator, ILoggerService logger) => (_mediator, _logger) = (mediator, logger);


    [HttpGet]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ExceptionHandling.ApplicationException))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ExceptionHandling.ApplicationException))]
    public async Task<IActionResult> UsersAsync()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        if (result.Succeeded)
        {
            return Ok(result.Result);
        }
        return BadRequest(new ExceptionHandling.ApplicationException(StatusCodes.Status400BadRequest, result.ErrorMessages.ToList()));
    }
}