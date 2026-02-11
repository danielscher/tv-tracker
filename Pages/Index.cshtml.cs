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
        Profiles = await _service.GetAllProfiles();
        Page();
    }

    public async Task OnPost(string name)
    {
        var profile = await _service.CreateProfile(name);
        Page();
    }


}
