using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Models.DTOs;
using TvTracker.Services;
using TvTracker.Utils;

namespace TvTracker.Pages.Shows;

public class ShowsModel: PageModel
{
    private readonly UserMediaService _userMediaService;
    private readonly MediaService<Series> _seriesService;
    private readonly TmbdService _tmdbService;

    public List<MediaView> WatchList {get; private set;}= [];
    public List<MediaView> AlreadyWatch {get; private set;} = [];
    public List<MediaView> SearchedSeries {get; private set;} = [];

    public ShowsModel(UserMediaService userMediaService, MediaService<Series> seriesService,TmbdService tmbdService)
    {
        _userMediaService = userMediaService;
        _seriesService = seriesService;
        _tmdbService = tmbdService;
    }

    public async Task OnGet()
    {
        var profileId = CookieUtils.ExtractProfileIdFromCookie(Request);
        WatchList = (await _userMediaService.GetUserSeriesWatchList(profileId)).Select(_userMediaService.ToView).ToList();
        AlreadyWatch = (await _userMediaService.GetUserSeriesAlreadyWatchedList(profileId)).Select(_userMediaService.ToView).ToList();
    }

    public async Task<IActionResult> OnPostSearchAsync([FromBody] string searchQuery)
    {
        var data = await _tmdbService.SearchSeries(searchQuery);
        return new JsonResult(data);
    }
}