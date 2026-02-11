using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;

namespace TvTracker.Pages;

public class HomeModel : PageModel
{


    public void OnGet()
    {

        Console.WriteLine("GET /home");
    }


}
