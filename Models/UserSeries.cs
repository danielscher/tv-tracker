using TvTracker.Models.Enums;

namespace TvTracker.Models;

public class UserSeries(Profile profile, Series series) : UserMedia(profile)
{
    public Series Series = series;
}