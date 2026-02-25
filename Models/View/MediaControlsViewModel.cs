using TvTracker.Models.Enums;
namespace TvTracker.Models.View;
public class MediaControlsViewModel
{
    public int TmdbId {get; set;}
    public int? Rating { get; set; }
    public WatchStatus? Status { get; set; }
    public DateTime? WatchDate { get; set; }
}