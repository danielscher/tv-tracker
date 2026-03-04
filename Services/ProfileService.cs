using Microsoft.EntityFrameworkCore;
using TvTracker.Data;
using TvTracker.Exception;
using TvTracker.Models;

namespace TvTracker.Services;
public class ProfileService(TvTrackerContext context)
{
    private readonly TvTrackerContext _context = context;

     /// <summary>
    /// Fetch a profile from the Database.
    /// </summary>
    /// <param name="profileId"> id of the profile to fetch </param>
    /// <returns>Profile with the id specified </returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<Profile> FetchProfile(int profileId)
    {
        var profile = await _context.Profiles.FindAsync(profileId) ?? throw new NotFoundException($"Profile with id {profileId} not found");
        return profile;
    }

    public async Task<ICollection<Profile>> FetchAllProfiles()
    {
        return await _context.Profiles.ToListAsync();
    }

    /// <summary>
    /// persists profile.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Newly created profile </returns>
    public async Task<Profile> CreateProfile(Profile profile)
    {
        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    /// <summary>
    /// Removes profile with the specified id.
    /// </summary>
    /// <param name="profileId"></param>
    public async Task DeleteProfile(int profileId)
    {
        int pid = profileId;
        await _context.Profiles.Where( p => p.Id == pid).ExecuteDeleteAsync();
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
        await _context.SaveChangesAsync();
        return profile;
    }


    /// <summary>
    /// Creates new profile and persists it in the Database
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"> is thrown if 4 profiles already exist </exception>
    public async Task<Profile> CreateProfile(string name)
    {
        if (await _context.Profiles.CountAsync() >= 4)
        {
            throw new InvalidOperationException("Cannot create more than 4 profiles.");
        }
        var normalizedName = name.Trim();
        var profile = new Profile(normalizedName);
        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();
        return profile;
    }


    /// <summary>
    /// Appends selected profile id to an http only cookie.
    /// </summary>
    public void AuthorizeProfile(HttpResponse response, int profileId)
    {
        response.Cookies.Append(
            "SelectedProfileId",
            profileId.ToString(),
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                Expires = DateTimeOffset.UtcNow.AddDays(7) // expires in 7 days.
            }
        );
    }

    /// <summary>
    /// Removes Selected profile id from cookie.
    /// </summary>
    public void UnauthorizeProfile(HttpResponse response)
    {
        response.Cookies.Delete("SelectedProfileId");
    }

    public void SwitchProfile(HttpResponse response, int profileId)
    {
        UnauthorizeProfile(response);
        AuthorizeProfile(response,profileId);
    }

}