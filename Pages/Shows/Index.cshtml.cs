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
    public List<MediaView> Trending {get; private set;} = [];
    public List<MediaView> Upcoming {get; private set;} = [];

    public ShowsModel(UserMediaService userMediaService, TmdbService tmbdService)
    {
        _userMediaService = userMediaService;
        _tmdbService = tmbdService;
    }

    public async Task OnGet()
    {
        var profileId = CookieUtils.GetProfileId(Request);

        // UserMedia
        WatchList = (await _userMediaService.GetSaved<UserSeries>(profileId)).Select(x=>x.ToView()).ToList();
        AlreadyWatch = (await _userMediaService.GetHistory<UserSeries>(profileId)).Select(x=>x.ToView()).ToList();

        // Tmdb
        Trending = (await _tmdbService.GetTrendingSeries()).Select(MapToView).ToList(); 
        Upcoming = (await _tmdbService.GetUpcomingEpisodes()).Select(MapToView).ToList();  
    }

    public async Task<IActionResult> OnPostSearchAsync([FromBody] string searchQuery)
    {
        var data = await _tmdbService.SearchSeries(searchQuery);
        return new JsonResult(data);
    }

        private MediaView MapToView(SearchResponseView response)
    {
        return new MediaView(
                response.TmdbId,
                Models.Enums.MediaType.Series,
                response.Title!,
                response.PosterUrl,
                null,null,false, false, null
            );
    }
}