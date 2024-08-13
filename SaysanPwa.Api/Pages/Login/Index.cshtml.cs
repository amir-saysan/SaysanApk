using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Api.Logging;
using SaysanPwa.Api.Modules;
using SaysanPwa.Application.InputModels.Login;
using SaysanPwa.Application.Query;
using SaysanPwa.Application.Query.Company;
using SaysanPwa.Application.Query.FiscalYear;
using SaysanPwa.Application.Query.Marketer;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.Login;

[AllowAnonymous]
[AutoValidateAntiforgeryToken]
public class IndexModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly ILoggerService _logger;
    private readonly IConfiguration _configuration;

    public IndexModel(IMediator mediator, ILoggerService logger, IConfiguration configuration)
    {
        _mediator = mediator;
        _logger = logger;
        _configuration = configuration;
    }

    [FromForm]
    [BindProperty]
    public LoginInput Input { get; set; } = null!;

    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = await _mediator.Send(new GetUserByUsernameQuery(Input.Username));
                if (user.Succeeded)
                {
                    var canUserLoginResult = await _mediator.Send(new CanUserLoginQuery(user.Result, Input.Password));
                    if (canUserLoginResult.Succeeded)
                    {
                        var lastFiscalYearResult = await _mediator.Send(new GetLastFiscalYearIdQuery());
                        if (lastFiscalYearResult.Succeeded)
                        {
                            var currentCompanyInformationResult = await _mediator.Send(new GetCurrentCompanyQuery());
                            if (currentCompanyInformationResult.Succeeded)
                            {
                                if (SessionManager.IsSessionLimitReached())
                                {
                                    ModelState.AddModelError("All", "تعداد کاربران بیشتر از حد مجاز است.");
                                    return Page();
                                }
                                else
                                {
                                    List<Claim> claims = new List<Claim>()
                                        {
                                            new(ClaimTypes.Name, user.Result.Username),
                                            new(ClaimTypes.NameIdentifier, user.Result.ID_tbl_Users.ToString()),
                                            new("State_User", user.Result.State_User.ToString()),
                                            new("ID_tbl_TarafHesab", user.Result.ID_tbl_TarafHesab.ToString()),
                                            new("Last_Fiscal_Year", lastFiscalYearResult.Result.Id.ToString()),
                                            new("FiscalYearBeginDate", lastFiscalYearResult.Result.BeginDate),
                                            new("FiscalYearEndDate", lastFiscalYearResult.Result.EndDate),
                                            new("FiscalYearTitle", lastFiscalYearResult.Result.Title),
                                            new("Comapny_Name", currentCompanyInformationResult.Result.Name_Comp),
                                            new("Company_Address", currentCompanyInformationResult.Result.Address1_Comp),
                                            new("S_ID", $"S_{user.Result.ID_tbl_Users}")
                                        };
                                    var getMarketerIdByPartnerIdResult = await _mediator.Send(new GetMarketerIdByPartnerIdQuery(user.Result.ID_tbl_TarafHesab));
                                    claims.Add(new("ID_tbl_Bzy", getMarketerIdByPartnerIdResult.Result.ToString()));
                                    await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

                                    SessionManager.AddSession($"S_{user.Result.ID_tbl_Users}");
                                    HttpContext.Session.SetInt32($"S_{user.Result.ID_tbl_Users}", user.Result.ID_tbl_Users);

                                    return Redirect("/");
                                }
                                
                            }
                        }

                    }
                }
                ModelState.AddModelError("All", "نام کاربری یا رمز عبور اشتباه است.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("All", "مشگلی در سرور رخ داده است. لطفا با پشتیبانی تماس بگیرید.");
            }
        }
        return Page();
    }
}
