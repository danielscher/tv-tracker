using TvTracker.Models.DTOs;

namespace TvTracker.Models.View;
public class MovieView
{
    public Guid? MediaId {get;set;}
    public int TmdbId {get;}
    public string Title {get;}
    public string? PosterUrl {get;}
    public ICollection<CastMemberView> Cast = [];

    public MovieView(MovieDetailsResponse movieResponse,Func<string,string> imageUrlBuilder)
    {
        MediaId = null;
        TmdbId = movieResponse.Id;
        Title = movieResponse.Title;
        PosterUrl = imageUrlBuilder(movieResponse.PosterPath);
        foreach (var cast in movieResponse.Credits.Cast)
        {
            Cast.Add(new CastMemberView(cast,imageUrlBuilder));
        }
    }
}