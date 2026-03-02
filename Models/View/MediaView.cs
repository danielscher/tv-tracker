using TvTracker.Models.Enums;
namespace TvTracker.Models.View;
public class MediaView(int tmdbId, MediaType type, string title, string? posterUrl, DateTime? releaseDate, int? rating, WatchStatus? status, DateTime? watchTime)
{
    public int TmdbId = tmdbId;
    public MediaType Type = type;

    public string? Title = title;
    public string? PosterUrl = posterUrl;
    public DateTime? ReleaseDate = releaseDate;

    public int? Rating = rating;
    public WatchStatus? Status = status;
    public DateTime? WatchDate = watchTime;


}