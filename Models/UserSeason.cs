using TvTracker.Models.Enums;

namespace TvTracker.Models;
public class UserSeason(Profile profile, Season season) : UserMedia(profile)
{   
    public Season Season {get;set;} = season;
}