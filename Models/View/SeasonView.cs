using TvTracker.Models.DTOs;

namespace TvTracker.Models.View;
public class SeasonView
{
    public Guid? MediaId {get;set;}
    public int TmdbId {get;}
    public string Title {get;}
    public string? PosterUrl {get;}
    public string Description {get;}
    public int SeasonNumber{get;}
    public int EpisodeCount {get;}
    public DateTime? ReleaseDate {get;}

    public SeasonView(SeasonResponse seasonResponse, Func<string,string> imageUrlBuilder)
    {
        MediaId = null;
        TmdbId = seasonResponse.Id;
        Title = seasonResponse.Title;
        PosterUrl = imageUrlBuilder(seasonResponse.PosterPath);
        Description = seasonResponse.Description;
        SeasonNumber = seasonResponse.SeasonNumber;
        EpisodeCount = seasonResponse.EpisodeCount;
        ReleaseDate = seasonResponse.ReleaseDate != null ?  DateTime.Parse(seasonResponse.ReleaseDate) : null;
    }
}