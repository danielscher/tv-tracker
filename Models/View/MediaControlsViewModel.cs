using TvTracker.Models.Enums;
namespace TvTracker.Models.View;
public class MediaControlsViewModel
{
    public Guid MediaId { get; set; }
    public int? Rating { get; set; }
    public WatchStatus? Status { get; set; }
    public DateTime? WatchDate { get; set; }
}