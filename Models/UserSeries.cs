using TvTracker.Models.DTOs;
using TvTracker.Models.Enums;

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
}