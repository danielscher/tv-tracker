using TvTracker.Models.DTOs;

namespace TvTracker.Models.View;
public class MovieView
{
    public Guid? MediaId {get;set;}
    public int TmdbId {get;}
    public string Title {get;}
    public string? PosterPath {get;}
    public ICollection<CastMemberView> Cast = [];

    public MovieView(MovieDetailsResponse movieResponse)
    {
        MediaId = null;
        TmdbId = movieResponse.Id;
        Title = movieResponse.Title;
        PosterPath = $"https://image.tmdb.org/t/p/w500{movieResponse.PosterPath}";
        foreach (var cast in movieResponse.Credits.Cast)
        {
            Cast.Add(new CastMemberView(cast));
        }
    }
}