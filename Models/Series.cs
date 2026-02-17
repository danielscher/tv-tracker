using TvTracker.Models.Enums;

namespace TvTracker.Models;

/// <summary>
/// Represents a Tv Show.
/// </summary>
public class Series : Media
{
    public AirStatus AirStatus{get;set;}

    private readonly ICollection<Season> _seasons = [];

    public IEnumerable<Season> Seasons => _seasons;

    public Series(MediaMetaInfo mediaInfo, AirStatus airStatus) : base(mediaInfo)
    {
        AirStatus = airStatus;
    }

    // EF materialization.
    private Series(AirStatus airStatus) : base()
    {
        AirStatus = airStatus;
    }
}