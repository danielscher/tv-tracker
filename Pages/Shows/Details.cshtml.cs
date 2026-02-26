using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    private readonly TmdbService _tmdbService;

    public SeriesView? SeriesView {get; private set;}

    public UserMediaInfo? UserMediaInfo{get; private set;}

    public MediaControlsViewModel? MediaControls {get; set;}


    public DetailsModel(UserMediaService userMediaService, MediaService<Series> seriesService,ProfileService profileService,TmdbService tmdbService)
    {
        _userMediaService = userMediaService;
        _seriesService = seriesService;
        _profileService = profileService;
        _tmdbService = tmdbService;
    }

    public async Task<IActionResult> OnGet(int tmdbId)
    {
        var response = await _tmdbService.GetSeriesDetails(tmdbId);
        SeriesView = response != null ? new(response,_tmdbService.PosterUrlBuilder) : null;

        if (SeriesView is null) { return NotFound();}

        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        UserMediaInfo = (await _userMediaService.GetUserMediaByProfileIdAndTmdbIdOptional(profileId,tmdbId))?.Flatten() ?? null;

        // setup partial view model.
        MediaControls = new()
        {
            TmdbId = tmdbId,
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
    public async Task<IActionResult> OnPostToggleWatchLater(int tmdbId)
    {   
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        UserMediaInfo ??= await FetchOrCreateUserMedia(tmdbId,profileId);
        UserMediaInfo = (await _userMediaService
        .UpdateWatchStatus<UserSeries>(profileId,UserMediaInfo.UserMediaId,WatchStatus.WantToWatch))
        .Flatten();
        return new JsonResult( new {status = UserMediaInfo.Status});
    }

    public async Task<IActionResult> OnPostRate(int tmdbId, int rating) 
    {
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        UserMediaInfo ??= await FetchOrCreateUserMedia(tmdbId,profileId);
        UserMediaInfo = (await _userMediaService
        .RateUserMedia(profileId,UserMediaInfo.UserMediaId,rating))
        .Flatten();
        return new JsonResult( new {rating = UserMediaInfo.Rating});  
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
        var dto = await _tmdbService.GetSeriesDetails(tmdbId);
        var series = Series.Create(dto!,_tmdbService.PosterUrlBuilder);
        series = await _seriesService.PersistMedia(series);

        var profile = await _profileService.FetchProfile(profileId);

        return (await _userMediaService
            .CreateUserSeries(profile, series))
            .Flatten();
    }
}   