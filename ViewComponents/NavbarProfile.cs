using Microsoft.AspNetCore.Mvc; 
using TvTracker.Services; 
using TvTracker.Utils; 

namespace TvTracker.ViewComponents;

/// <summary>
/// ViewComponent responsible for the navbar profile dropdown.
/// Retrieves the available profiles and the currently active profile
/// from the request cookie and passes the data to the component view.
/// If no active profile is present (e.g., landing page), no content is rendered.
/// </summary>
public class NavbarProfileViewComponent : ViewComponent 
{ 
    private readonly ProfileService _profileService; 
    public NavbarProfileViewComponent(ProfileService profileService) { _profileService = profileService; } 
    public async Task<IViewComponentResult> InvokeAsync() 
    { 
        var profiles = await _profileService.FetchAllProfiles(); 
        var profileId = CookieUtils.TryExtractProfileId(Request);

        if (profileId == null) { return Content(""); } // do not render.

        var model = new NavbarProfileView(profiles,profileId.Value); 
        return View(model); 
    } 
}