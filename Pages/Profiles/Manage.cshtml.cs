using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Services;
using TvTracker.Utils;

namespace TvTracker.Pages.Profiles
{
    public class CreateModel(ProfileService service) : PageModel
    {
        private readonly ProfileService _service = service;

        public Profile? Profile {get; private set;}

        public async Task OnGet(int? id)
        {
            Profile = id != null ? await _service.FetchProfile(id.Value) : null;
        }

        public async Task<IActionResult> OnPostAsync(string name,string imgUrl)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError(string.Empty, "Name cannot be empty.");
                return Page();
            }

            try
            {
                await _service.CreateProfile(name,imgUrl);
                return RedirectToPage("/Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostUpdateAsync(string name,string imgUrl)
        {
            var profileId = CookieUtils.GetProfileId(Request); 
            
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError(string.Empty, "Name cannot be empty.");
                return Page();
            }

            Profile = await _service.UpdateProfile(profileId,name,imgUrl);
            return Page();
        }
    }
}
