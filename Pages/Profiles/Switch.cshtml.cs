using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Services;

namespace TvTracker.Pages.Profiles;

public class SwitchModel(ProfileService service) : PageModel
{
    private ProfileService _service = service;
    public IActionResult OnPost(int newProfileId)
    {
        _service.SwitchProfile(Response, newProfileId);

        // Redirect back to previous page
        var referer = Request.Headers["Referer"].ToString();
        if (!string.IsNullOrEmpty(referer))
            return Redirect(referer);

        return RedirectToPage("/Home");
    }
}