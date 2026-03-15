using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Models.DTOs;
using TvTracker.Models.View;
using TvTracker.Services;
using TvTracker.Utils;

namespace TvTracker.Pages.Movies;

public class MoviesModel: PageModel
{
    private readonly UserMediaService _userMediaService;

    private readonly TmdbService _tmdbService;

    public List<MediaView> WatchList {get; private set;}= [];
    public List<MediaView> AlreadyWatch {get; private set;} = [];
    public List<MediaView> Trending {get; private set;} = [];
    public List<MediaView> Upcoming {get; private set;} = [];
    public List<MediaView> InTheaters {get; private set;} = [];




    public MoviesModel(UserMediaService userMediaService, TmdbService tmdbService)
    {
        _userMediaService = userMediaService;
        _tmdbService = tmdbService;
    }

    public async Task OnGet()
    {
        var profileId = CookieUtils.GetProfileId(Request);
        WatchList = (await _userMediaService.GetUserMovieWatchList(profileId)).Select(x=>x.ToView()).ToList();
        AlreadyWatch = (await _userMediaService.GetUserMovieAlreadyWatchedList(profileId)).Select(x=>x.ToView()).ToList();
        Trending = (await _tmdbService.GetTrendingMovies()).Select(MapToView).ToList();
        Upcoming = (await _tmdbService.GetUpcomingMovies()).Select(MapToView).ToList();
    }

    public async Task<IActionResult> OnPostSearchAsync([FromBody] string searchQuery)
    {
        var data = await _tmdbService.SearchMovies(searchQuery);
        return new JsonResult(data);
    }

    private MediaView MapToView(SearchResponseView response)
    {
        return new MediaView(
                response.TmdbId,
                Models.Enums.MediaType.Movie,
                response.Title!,
                response.PosterUrl,
                null,null,null, null
            );
    }
}