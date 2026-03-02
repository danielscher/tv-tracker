using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Models.DTOs;
using TvTracker.Models.Enums;
using TvTracker.Models.View;
using TvTracker.Services;
using TvTracker.Utils;

namespace TvTracker.Pages.Movies;

public class DetailsModel: PageModel
{
    private readonly UserMediaService _userMediaService;
    private readonly MediaService<Movie> _movieService;
    private readonly ProfileService _profileService;
    private readonly TmdbService _tmdbService;

    // public Movie? Movie {get; set;}
    public MovieView? MovieView {get;private set;}
    public UserMediaInfo? UserMediaInfo {get;private set;}
    public List<CastMember> Cast {get; private set;} = [];


    public DetailsModel(UserMediaService userMediaService, MediaService<Movie> movieService, ProfileService profileService, TmdbService tmdbService)
    {
        _userMediaService = userMediaService;
        _movieService = movieService;
        _profileService = profileService;
        _tmdbService = tmdbService;
    }

    public async Task<IActionResult> OnGet(int tmdbId)
    {
        var response = await _tmdbService.GetMovieDetails(tmdbId);
        MovieView = response != null ? new(response,_tmdbService.PosterUrlBuilder) : null;

        if (MovieView == null)
        {
            return NotFound();
        }

        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        UserMediaInfo = (await _userMediaService
        .GetUserMediaByProfileIdAndTmdbIdOptional(profileId,tmdbId))?
        .Flatten() ?? null;

        return Page();
    }   

     public async Task<IActionResult> OnPostRate(int tmdbId, int? rating) 
    {
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        UserMediaInfo ??= await FetchOrCreateUserMedia(tmdbId,profileId);
        UserMediaInfo = (await _userMediaService
        .RateUserMedia(profileId,UserMediaInfo.UserMediaId,rating))
        .Flatten();
        return new JsonResult( new {rating = UserMediaInfo.Rating});  
    } 

    /// <summary>
    /// Adds this media to the watch list.
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnPostToggleWatchLater(int tmdbId)
    {   
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        UserMediaInfo ??= await FetchOrCreateUserMedia(tmdbId,profileId);
        UserMediaInfo = (await _userMediaService
        .UpdateWatchStatus<UserMovie>(profileId,UserMediaInfo.UserMediaId,WatchStatus.WantToWatch))
        .Flatten();
        return new JsonResult( new {status = UserMediaInfo.Status});
    }

    private async Task<UserMediaInfo> FetchOrCreateUserMedia(int tmdbId, int profileId)
    {
        // fetch existing
        var existing = (await _userMediaService
            .GetUserMediaByProfileIdAndTmdbIdOptional(profileId, tmdbId))
            ?.Flatten();

        if (existing is not null)
            return existing;

        // otherwise create new
        var dto = await _tmdbService.GetMovieDetails(tmdbId);
        var movie = Movie.CreateMovie(dto!,_tmdbService.PosterUrlBuilder);

        movie = await _movieService.PersistMedia(movie);

        var profile = await _profileService.FetchProfile(profileId);

        return (await _userMediaService
            .CreateUserMovie(profile, movie))
            .Flatten();
    }
}