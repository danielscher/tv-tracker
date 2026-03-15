using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TvTracker.Models;
using TvTracker.Models.DTOs;
using TvTracker.Models.View;
using TvTracker.Services;
using TvTracker.Utils;

namespace TvTracker.Pages.Shows;

public class ShowsModel: PageModel
{
    private readonly UserMediaService _userMediaService;
    private readonly TmdbService _tmdbService;

    public List<MediaView> WatchList {get; private set;}= [];
    public List<MediaView> AlreadyWatch {get; private set;} = [];

    public ShowsModel(UserMediaService userMediaService, TmdbService tmbdService)
    {
        _userMediaService = userMediaService;
        _tmdbService = tmbdService;
    }

    public async Task OnGet()
    {
        var profileId = CookieUtils.GetProfileId(Request);
        WatchList = (await _userMediaService.GetUserSeriesWatchList(profileId)).Select(x=>x.ToView()).ToList();
        AlreadyWatch = (await _userMediaService.GetUserSeriesAlreadyWatchedList(profileId)).Select(x=>x.ToView()).ToList();
    }

    public async Task<IActionResult> OnPostSearchAsync([FromBody] string searchQuery)
    {
        var data = await _tmdbService.SearchSeries(searchQuery);
        return new JsonResult(data);
    }
}