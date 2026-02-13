namespace TvTracker.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Services;
using TvTracker.Utils;

public class HomeModel(UserMediaService userMediaService): PageModel
{
    private readonly UserMediaService _service = userMediaService;

    public List<UserSeries> UserSeries = [];
    public List<UserMovie> UserMovies = [];


    public async Task OnGet()
    {
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        UserMovies = await _service.GetUserMovieWatchList(profileId);
        UserSeries = await _service.GetUserSeriesWatchList(profileId);
    }

}