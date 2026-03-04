using Microsoft.AspNetCore.Mvc; 
using TvTracker.Services; 
using TvTracker.Utils; 

namespace TvTracker.ViewComponents;
public class NavbarProfileViewComponent : ViewComponent 
{ 
    private readonly ProfileService _profileService; 
    public NavbarProfileViewComponent(ProfileService profileService) { _profileService = profileService; } 
    public async Task<IViewComponentResult> InvokeAsync() 
    { 
        var profiles = await _profileService.FetchAllProfiles(); 
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request); 
        var model = new NavbarProfileView(profiles,profileId); 
        return View(model); 
    } 
}