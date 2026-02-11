using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Services;

namespace TvTracker.Pages.Profiles
{
    public class CreateModel(ProfileService service) : PageModel
    {
        private readonly ProfileService _service = service;

        public void OnGet() {}

        public async Task<IActionResult> OnPostAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError(string.Empty, "Name cannot be empty.");
                return Page();
            }

            try
            {
                await _service.CreateProfile(name);
                return RedirectToPage("/Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
