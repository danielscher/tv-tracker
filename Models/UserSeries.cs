using TvTracker.Models.Enums;

namespace TvTracker.Models;

public class UserSeries : UserMedia
{
    public Series Series { get; }


    public UserSeries(Profile profile, Series series) : base(profile)
    {
        Series = series;
    }

    private UserSeries() : base()
    {
        Series = null!;   
    }
}