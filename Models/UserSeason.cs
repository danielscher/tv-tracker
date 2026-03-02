using TvTracker.Models.DTOs;
using TvTracker.Models.Enums;
using TvTracker.Models.View;

namespace TvTracker.Models;
public class UserSeason : UserMedia
{   
    public Season Season {get;}

    public UserSeason(Profile profile, Season season) : base(profile,season.Id,season.TmdbId)
    {
        Season = season;
    }

    // EF materialization.
    private UserSeason() : base()
    {
        Season = null!;
    }

    public override MediaView ToView()
    {
        return new (
            Season.TmdbId,
            Enums.MediaType.Season,
            $"Season {Season.SeasonNumber}",
            null,
            Season.ReleaseDate,
            Rating,
            Status,
            WatchedAt
        );
    }
}