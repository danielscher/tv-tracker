using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Models.View;
using TvTracker.Services;
using TvTracker.Utils;

namespace TvTracker.Pages.Home;

public class HomeModel: PageModel
{
    private readonly UserMediaService _userMediaService;
    private readonly ProfileService _profileService;

    public string? ProfileName {get; private set;}

    public List<UserSeries> UserSeries {get; private set;} = [];
    public List<UserMovie> UserMovies {get; private set;} = [];
    public List<MediaView> RecentlyWatched {get; private set;} = [];

    public int SeriesCount {get; private set;}
    public int SeriesWatchTime {get; private set;}

    public int MoviesCount {get; private set;}
    public int MoviesWatchTime {get; private set;}



    public HomeModel(UserMediaService mediaService, ProfileService profileService)
    {
        _userMediaService = mediaService;
        _profileService = profileService;
    }

    public async Task OnGet()
    {
        var profileId = CookieUtils.GetProfileId(Request);
        ProfileName = (await _profileService.FetchProfile(profileId)).Name;
        
        RecentlyWatched = (await _userMediaService.GetHistory<UserMedia>(profileId)).Select(x=>x.ToView()).ToList();

        SeriesCount = await _userMediaService.GetTotalMediaWatchedCount<UserSeries>(profileId);
        SeriesWatchTime = _userMediaService.GetTotalSeriesWatchTime(profileId);

        MoviesCount = await _userMediaService.GetTotalMediaWatchedCount<UserMovie>(profileId);
        MoviesWatchTime = _userMediaService.GetTotalMovieWatchTime(profileId);
    }
}
