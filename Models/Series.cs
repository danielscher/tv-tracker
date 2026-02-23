using TvTracker.Models.DTOs;
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

    public void AddSeason(Season season)
    {
        _seasons.Add(season);
    }

    public void AddSeasonRange(ICollection<Season> seasons)
    {
        foreach (var s in seasons)
        {
            AddSeason(s);
        }
    }

    public override MediaView ToResponse()
    {
        return new MediaView(Id, MediaType.Series, MediaInfo.Title, MediaInfo.PosterPath);
    }
}