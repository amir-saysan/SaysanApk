using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Api.Modules;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.Logout;

[Authorize]
public class IndexModel : PageModel
{
    public async Task<IActionResult> OnGetAsync()
    {
        await HttpContext.SignOutAsync();
        DeleteSession();
        return Redirect("/");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync();
        DeleteSession();
        return Redirect("/");
    }

    private void DeleteSession()
    {
        string sessionId = User.FindFirstValue("S_ID");
        if (!string.IsNullOrEmpty(sessionId))
        {
            SessionManager.RemoveSession(sessionId);
            HttpContext.Session.Remove(sessionId);
        }
    }
}
