using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Services;

namespace TvTracker.Pages;

public class IndexModel(ProfileService service, TmdbService tmdbService) : PageModel
{
    private readonly ProfileService _service = service;
    private readonly TmdbService _tmdbService = tmdbService;
    public ICollection<Profile> Profiles{get;set;} = [];

    public async Task OnGet()
    {
        Profiles = await _service.FetchAllProfiles();
        Page();
    }

    public async Task<Microsoft.AspNetCore.Mvc.RedirectResult> OnPostSelect(int profileId)
    {
        _service.AuthorizeProfile(Response,profileId);
        return Redirect("/Home");
    }


}
