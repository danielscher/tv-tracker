using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Services;
using TvTracker.Utils;

namespace TvTracker.Pages.Shows;

public class ShowsModel: PageModel
{
    private readonly UserMediaService _userMediaService;
    private readonly MediaService<Series> _seriesService;

    public List<UserSeries> WatchList {get; private set;}= [];
    public List<UserSeries> AlreadyWatch {get; private set;} = [];
    public List<Series> SearchedSeries {get; private set;} = [];

    public ShowsModel(UserMediaService userMediaService, MediaService<Series> seriesService)
    {
        _userMediaService = userMediaService;
        _seriesService = seriesService;
    }

    public async Task OnGet()
    {
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        WatchList = await _userMediaService.GetUserSeriesWatchList(profileId);
        AlreadyWatch = await _userMediaService.GetUserSeriesAlreadyWatchedList(profileId);
    }

    public async Task<IActionResult> OnPostSearchAsync([FromBody] string searchQuery)
    {
        SearchedSeries = await _seriesService.SearchMedia(searchQuery);
        Console.WriteLine($"@ num of series: {SearchedSeries.Count}");
        return new JsonResult(SearchedSeries);
    }
}