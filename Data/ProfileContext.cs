using Microsoft.EntityFrameworkCore;
using TvTracker.Exception;
using TvTracker.Models;

namespace TvTracker.Data;

public class ProfileContext(DbContextOptions<ProfileContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles {get; set;}

    /// <summary>
    /// Fetch a profile from the Database.
    /// </summary>
    /// <param name="profileId"> id of the profile to fetch </param>
    /// <returns>Profile with the id specified </returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<Profile> FetchProfile(int profileId)
    {
        var profile = await Profiles.FindAsync(profileId) ?? throw new NotFoundException($"Profile with id {profileId} not found");
        return profile;
    }

    public async Task<ICollection<Profile>> FetchAllProfiles()
    {
        return await Profiles.ToListAsync();
    }

    /// <summary>
    /// persists profile.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Newly created profile </returns>
    public async Task<Profile> CreateProfile(Profile profile)
    {
        Profiles.Add(profile);
        await SaveChangesAsync();
        return profile;
    }

    /// <summary>
    /// Removes profile with the specified id.
    /// </summary>
    /// <param name="profileId"></param>
    public async Task DeleteProfile(int profileId)
    {
        int pid = profileId;
        await Profiles.Where( p => p.Id == pid).ExecuteDeleteAsync();
    }

    /// <summary>
    /// Performs an update on an existing profile
    /// </summary>
    /// <param name="profileId"> the id of the profile to update </param>
    /// <param name="newName"></param>
    /// <returns>updated profile</returns>
    public async Task<Profile> UpdateProfile(int profileId,string newName)
    {
        var profile = await FetchProfile(profileId);
        profile.Name = newName;
        await SaveChangesAsync();
        return profile;
    }
}