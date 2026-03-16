using TvTracker.Models.DTOs;
using TvTracker.Models.Enums;
using TvTracker.Models.View;

namespace TvTracker.Models;

public class UserSeries : UserMedia
{
    public Series Series { get; }

    public ICollection<UserSeason> UserSeasons {get;} = [];


    public UserSeries(Profile profile, Series series) : base(profile,series.Id,series.TmdbId)
    {
        Series = series;
        foreach (var season in Series.Seasons)
        {
            var userSeason = new UserSeason(profile,season);
            UserSeasons.Add(userSeason);
        }
    }

    private UserSeries() : base()
    {
        Series = null!;   
    }

        public override MediaView ToView()
    {
        return new (
            Series.TmdbId,
            Enums.MediaType.Series,
            Series.MediaInfo.Title,
            Series.MediaInfo.PosterPath,
            Series.MediaInfo.ReleaseDate,
            Rating,
            Watched,
            Saved,
            WatchedAt
        );
    }
}