using TvTracker.Models.Enums;
namespace TvTracker.Models.View;
public class MediaView(int tmdbId, MediaType type, string title, string? posterUrl, DateTime? releaseDate, int? rating, bool watched, bool saved, DateTime? watchTime)
{
    public int TmdbId = tmdbId;
    public MediaType Type = type;

    public string? Title = title;
    public string? PosterUrl = posterUrl;
    public DateTime? ReleaseDate = releaseDate;

    public int? Rating = rating;
    public bool Watched = watched;
    public bool Saved = saved;
    public DateTime? WatchDate = watchTime;


}