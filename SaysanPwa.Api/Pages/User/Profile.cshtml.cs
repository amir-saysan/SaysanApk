using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SaysanPwa.Api.Pages.User;

[Authorize]
public class ProfileModel : PageModel
{
    public void OnGet()
    {
    }
}
