using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Exception;
using TvTracker.Models;
using TvTracker.Models.DTOs;
using TvTracker.Models.Enums;
using TvTracker.Models.View;
using TvTracker.Services;
using TvTracker.Utils;

namespace TvTracker.Pages.Shows;

public class DetailsModel: PageModel
{
    private readonly UserMediaService _userMediaService;
    private readonly MediaService<Series> _seriesService;
    private readonly ProfileService _profileService;

    public Series? Series {get; private set;}

    public UserMediaInfo? UserMediaInfo{get; private set;}

    public UserSeries? UserSeries {get; private set;}

    public MediaControlsViewModel? MediaControls {get; set;}


    public DetailsModel(UserMediaService userMediaService, MediaService<Series> seriesService,ProfileService profileService)
    {
        _userMediaService = userMediaService;
        _seriesService = seriesService;
        _profileService = profileService;
    }

    public async Task<IActionResult> OnGet(Guid id)
    {

        Series = await _seriesService.GetMedia(id);
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        UserMediaInfo = (await _userMediaService.GetUserMediaByProfileIdAndMediaIdOptional(profileId,id))?.Flatten() ?? null;
        UserSeries = (UserSeries?)await _userMediaService.GetUserMediaByProfileIdAndMediaId(profileId,id);


        if (Series == null)
        {
            return NotFound();
        }

        // setup partial view model.
        MediaControls = new()
        {
            TmdbId = -1,
            Status = UserMediaInfo?.Status ?? WatchStatus.None,
            Rating = UserMediaInfo?.Rating,
            WatchDate = UserMediaInfo?.WatchDate
        };


        return Page();
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
            Series = await _seriesService.GetMedia(mediaId);
            var profile = await _profileService.FetchProfile(profileId);
            UserMediaInfo = (await _userMediaService.CreateUserSeries(profile,Series)).Flatten();
        } 

        UserMediaInfo = (await _userMediaService.UpdateWatchStatus<UserSeries>(profileId,UserMediaInfo.UserMediaId,WatchStatus.WantToWatch)).Flatten();
        return new JsonResult( new {status = UserMediaInfo.Status});
    }

    public async Task<IActionResult> OnPostRate(Guid mediaId, int rating) 
    {
        Series = await _seriesService.GetMedia(mediaId);
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        UserMediaInfo = (await _userMediaService.GetUserMediaByProfileIdAndMediaIdOptional(profileId,mediaId))?.Flatten() ?? null;

        // create UserMedia if not already exists.
        if (UserMediaInfo == null)
        {
            var profile = await _profileService.FetchProfile(profileId);
            UserMediaInfo = (await _userMediaService.CreateUserSeries(profile,Series)).Flatten();
        }
        UserMediaInfo = (await _userMediaService.RateUserMedia(profileId,UserMediaInfo.UserMediaId,rating)).Flatten();
        return new JsonResult( new {rating = UserMediaInfo.Rating});  
    } 
}   