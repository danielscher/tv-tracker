using TvTracker.Models;

public class NavbarProfileView(ICollection<Profile> profiles, int profileId)
{
    public ICollection<Profile> Profiles = profiles;
    public int ActiveProfileId = profileId;
}