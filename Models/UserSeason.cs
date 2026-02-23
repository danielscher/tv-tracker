using TvTracker.Models.DTOs;
using TvTracker.Models.Enums;

namespace TvTracker.Models;
public class UserSeason : UserMedia
{   
    public Season Season {get;}

    public UserSeason(Profile profile, Season season) : base(profile,season.Id)
    {
        Season = season;
    }

    // EF materialization.
    private UserSeason() : base()
    {
        Season = null!;
    }
}