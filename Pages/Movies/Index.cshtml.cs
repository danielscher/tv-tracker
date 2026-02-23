using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Services;
using TvTracker.Utils;

namespace TvTracker.Pages.Movies;

public class MoviesModel: PageModel
{
    private readonly UserMediaService _userMediaService;
    private readonly MediaService<Movie> _movieService;

    public List<Movie> SearchResult {get; private set;} = [];
    public List<UserMovie> WatchList {get; private set;}= [];
    public List<UserMovie> AlreadyWatch {get; private set;} = [];

    public MoviesModel(UserMediaService userMediaService, MediaService<Movie> movieService)
    {
        _userMediaService = userMediaService;
        _movieService = movieService;
    }

    public async Task OnGet()
    {
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        WatchList = await _userMediaService.GetUserMovieWatchList(profileId);
        AlreadyWatch = await _userMediaService.GetUserMovieAlreadyWatchedList(profileId);
    }

    public async Task<IActionResult> OnPostSearchAsync([FromBody] string searchQuery)
    {
        SearchResult = await _movieService.SearchMedia(searchQuery);
        return new JsonResult(SearchResult);
    }
}