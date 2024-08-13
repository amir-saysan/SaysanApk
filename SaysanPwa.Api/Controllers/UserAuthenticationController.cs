using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SaysanPwa.Api.Filters;
using SaysanPwa.Api.Logging;
using SaysanPwa.Application.DTOs.Login;
using SaysanPwa.Application.Query;
using SaysanPwa.Domain.AggregateModels.UserAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SaysanPwa.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserAuthenticationController : Controller
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;
    private readonly ILoggerService _logger;

    public UserAuthenticationController(IMediator mediator, IConfiguration configuration, ILoggerService logger)
    {
        _mediator = mediator;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ExceptionHandling.ApplicationException))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ExceptionHandling.ApplicationException))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ExceptionHandling.ApplicationException))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ExceptionHandling.ApplicationException))]
    [ValidateModelState]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto loginRequestDto)
    {
        var result = await _mediator.Send(new GetUserByUsernameQuery(loginRequestDto.Username));
        if (result.Succeeded)
        {
            var canUserLoginResult = await _mediator.Send(new CanUserLoginQuery(result.Result, loginRequestDto.Password));
            if (canUserLoginResult.Succeeded)
            {
                return Ok(new LoginResponseDto(GenerateTokenJwtByUser(result.Result)));
            }
        }
        return BadRequest();
    }


    private string GenerateTokenJwtByUser(User user)
    {
        SigningCredentials signingCredentials = new(
    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecretToken"]!)), SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.ID_tbl_Users.ToString())
            };

        var tokenOptions = new JwtSecurityToken(_configuration["Issuer"], _configuration["Audience"], claims, null, DateTime.Now.AddMinutes(10), signingCredentials);


        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}
