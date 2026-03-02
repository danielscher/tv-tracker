using TvTracker.Models.DTOs;

namespace TvTracker.Models.View;
public class SeriesView
{
    public Guid? MediaId {get;set;}
    public int TmdbId {get;}
    public string Title {get;}
    public DateTime? ReleaseDate {get;}
    public string Description {get;}
    public string? PosterUrl {get;}
    public ICollection<SeasonView> Seasons {get;} =[];
    public ICollection<CastMemberView> Cast = [];


    public SeriesView(SeriesDetailsResponse seriesResponse, Func<string,string> imageUrlBuilder)
    {
        MediaId = null;
        TmdbId = seriesResponse.Id;
        Title = seriesResponse.Title;
        ReleaseDate = seriesResponse.ReleaseDate != null ? DateTime.Parse(seriesResponse.ReleaseDate) : null;
        Description = seriesResponse.Description;
        PosterUrl = imageUrlBuilder(seriesResponse.PosterPath);
        foreach (var season in seriesResponse.Seasons)
        {
            Seasons.Add(new(season,imageUrlBuilder));
        }
        foreach (var cast in seriesResponse.Credits.Cast)
        {
            Cast.Add(new CastMemberView(cast,imageUrlBuilder));
        }
    }
}