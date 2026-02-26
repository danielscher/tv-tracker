using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Models.DTOs;
using TvTracker.Services;
using TvTracker.Utils;

namespace TvTracker.Pages.Movies;

public class MoviesModel: PageModel
{
    private readonly UserMediaService _userMediaService;
    private readonly MediaService<Movie> _movieService;

    private readonly TmdbService _tmbdService;

    public List<Movie> SearchResult {get; private set;} = [];
    public List<MediaView> WatchList {get; private set;}= [];
    public List<MediaView> AlreadyWatch {get; private set;} = [];

    public MoviesModel(UserMediaService userMediaService, MediaService<Movie> movieService, TmdbService tmbdService)
    {
        _userMediaService = userMediaService;
        _movieService = movieService;
        _tmbdService = tmbdService;
    }

    public async Task OnGet()
    {
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        WatchList = (await _userMediaService.GetUserMovieWatchList(profileId)).Select(_userMediaService.ToView).ToList();
        AlreadyWatch = (await _userMediaService.GetUserMovieAlreadyWatchedList(profileId)).Select(_userMediaService.ToView).ToList();
    }

    public async Task<IActionResult> OnPostSearchAsync([FromBody] string searchQuery)
    {
        // SearchResult = await _movieService.SearchMedia(searchQuery);
        var data = await _tmbdService.SearchMovies(searchQuery);
        return new JsonResult(data);
    }
}