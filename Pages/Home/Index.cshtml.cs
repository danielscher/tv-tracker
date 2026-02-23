using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Services;
using TvTracker.Utils;

namespace TvTracker.Pages.Home;

public class HomeModel: PageModel
{
    private readonly UserMediaService _service;

    public List<UserSeries> UserSeries {get; private set;} = [];
    public List<UserMovie> UserMovies {get; private set;}= [];

    public HomeModel(UserMediaService service)
    {
        _service = service;
    }

    public async Task OnGet()
    {
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        UserMovies = await _service.GetUserMovieWatchList(profileId);
        UserSeries = await _service.GetUserSeriesWatchList(profileId);
    }
}
