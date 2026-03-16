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

    public List<MediaView> MovieCollection {get; private set;} = [];
    public UserMediaInfo? UserMediaInfo {get;private set;}


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

        if (response!.CollectionInfo!=null)
        {
            MovieCollection = (await _tmdbService.GetMovieCollection(response.CollectionInfo.Id)).Where(x => x.Id != tmdbId).Select(x=>
            {
                var posterUrl = x.PosterPath != null ? _tmdbService.PosterUrlBuilder(x.PosterPath) : null;
                DateTime? releaseDate = x.ReleaseDate != null ? DateTime.Parse(x.ReleaseDate) : null;
                return new MediaView(x.Id,MediaType.Movie,x.Title,posterUrl,releaseDate,null,false,false,null);
                
            }).ToList();  
        }


        var profileId = CookieUtils.GetProfileId(Request);
        UserMediaInfo = (await _userMediaService
        .GetUserMediaByProfileIdAndTmdbIdOptional(profileId,tmdbId))?
        .Flatten() ?? null;

        return Page();
    }   

     public async Task<IActionResult> OnPostRate(int tmdbId, int? rating) 
    {
        var profileId = CookieUtils.GetProfileId(Request);
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
        var profileId = CookieUtils.GetProfileId(Request);
        UserMediaInfo ??= await FetchOrCreateUserMedia(tmdbId,profileId);
        UserMediaInfo = (await _userMediaService
        .MarkAsSaved(profileId,UserMediaInfo.UserMediaId))
        .Flatten();
        return new JsonResult( new {saved = UserMediaInfo.Saved});
    }

    public async Task<IActionResult> OnPostMarkAsWatched(int tmdbId)
    {   
        var profileId = CookieUtils.GetProfileId(Request);
        UserMediaInfo ??= await FetchOrCreateUserMedia(tmdbId,profileId);
        UserMediaInfo = (await _userMediaService
        .MarkAsWatched(profileId,UserMediaInfo.UserMediaId))
        .Flatten();
        return new JsonResult( new {watchedAt = UserMediaInfo.WatchDate?.ToString("dd/MM/yy")});
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