using Microsoft.EntityFrameworkCore;
using TvTracker.Data;
using TvTracker.Models;

namespace TvTracker.Services;
public class ProfileService(ProfileContext context)
{
    private readonly ProfileContext _context = context;

    public async Task<Profile> GetProfile(int profileId) => await _context.FetchProfile(profileId);

    public async Task<ICollection<Profile>> GetAllProfiles() => await _context.FetchAllProfiles();

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
        Console.WriteLine("new Profile Created");
        return await _context.CreateProfile(profile);
    }

    public async Task DeleteProfile(int profileId) => await _context.DeleteProfile(profileId);

}