using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Services;

namespace TvTracker.Pages;

public class IndexModel(ProfileService service) : PageModel
{
    private readonly ProfileService _service = service;
    public ICollection<Profile> Profiles{get;set;} = [];

    public async Task OnGet()
    {
        Profiles = await _service.FetchAllProfiles();
        Page();
    }

    public async Task OnPostSelect(int profileId)
    {
        _service.AuthorizeProfile(Response,profileId);
    }


}
