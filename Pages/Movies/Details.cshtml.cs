using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Exception;
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

    public Movie? Movie {get; set;}
    public UserMediaInfo? UserMediaInfo {get;private set;}
    public List<CastMember> Cast {get; private set;} = [];
    public MediaControlsViewModel? MediaControls {get; set;}


    public DetailsModel(UserMediaService userMediaService, MediaService<Movie> movieService, ProfileService profileService)
    {
        _userMediaService = userMediaService;
        _movieService = movieService;
        _profileService = profileService;
    }

    public async Task<IActionResult> OnGet(Guid id)
    {


        Movie = await _movieService.GetMedia(id);
        Cast = await _movieService.Experimental(id);

        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        UserMediaInfo = (await _userMediaService.GetUserMediaByProfileIdAndMediaIdOptional(profileId,id))?.Flatten() ?? null;
        // MovieView = movie?.ToResponse();

        if (Movie == null)
        {
            return NotFound();
        }

        MediaControls = new()
        {
            MediaId = Movie.Id,
            Rating = UserMediaInfo?.Rating,
            Status = UserMediaInfo?.Status,
            WatchDate = UserMediaInfo?.WatchDate
        };

        return Page();
    }

     public async Task<IActionResult> OnPostRate(Guid mediaId, int rating) 
    {
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        UserMediaInfo = (await _userMediaService.GetUserMediaByProfileIdAndMediaIdOptional(profileId,mediaId))?.Flatten() ?? null;

        // create UserMedia if not already exists.
        if (UserMediaInfo == null)
        {
            Movie = await _movieService.GetMedia(mediaId);
            var profile = await _profileService.FetchProfile(profileId);
            UserMediaInfo = (await _userMediaService.CreateUserMovie(profile,Movie)).Flatten();
        }
        UserMediaInfo = (await _userMediaService.RateUserMedia(profileId,UserMediaInfo.UserMediaId,rating)).Flatten();
        return new JsonResult( new {rating = UserMediaInfo.Rating});  
    } 

        /// <summary>
    /// Adds this media to the watch list.
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnPostToggleWatchLater(Guid mediaId)
    {   
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        UserMediaInfo = (await _userMediaService.GetUserMediaByProfileIdAndMediaIdOptional(profileId,mediaId))?.Flatten() ?? null;

        // create UserMedia if not already exists.
        if (UserMediaInfo == null)
        {
            Movie = await _movieService.GetMedia(mediaId);
            var profile = await _profileService.FetchProfile(profileId);
            UserMediaInfo = (await _userMediaService.CreateUserMovie(profile,Movie)).Flatten();
        } 

        UserMediaInfo = (await _userMediaService.UpdateWatchStatus<UserMovie>(profileId,UserMediaInfo.UserMediaId,WatchStatus.WantToWatch)).Flatten();
        return new JsonResult( new {status = UserMediaInfo.Status});
    }
}