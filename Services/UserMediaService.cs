using TvTracker.Data;
using TvTracker.Exception;
using TvTracker.Models;
using TvTracker.Models.Enums;
using Microsoft.EntityFrameworkCore;
using TvTracker.Models.DTOs;
using TvTracker.Models.View;


namespace TvTracker.Services;

public class UserMediaService(TvTrackerContext context)
{
    private readonly TvTrackerContext _context = context;
    
    public async Task<UserMedia?> GetUserMediaByIdAndProfileIdOptional(int profileId, Guid id) 
    {
        var um =  await GetUserMedia<UserMedia>(profileId)
        .Where(u=> u.Id == id)
        .SingleOrDefaultAsync(); 
        return um;
    }

    public async Task<UserMedia> GetUserMediaByIdAndProfileId(int profileId, Guid mediaId)
    {
        return (await GetUserMediaByIdAndProfileIdOptional(profileId,mediaId)) 
        ?? throw new NotFoundException($"The UserMedia {mediaId} for profile {profileId} not found."); 
    }

    public async Task<UserMedia?> GetUserMediaByProfileIdAndTmdbIdOptional(int profileId, int tmdbId) 
    {
        var um = await GetUserMedia<UserMedia>(profileId)
        .Where(u=> u.TmdbId == tmdbId)
        .SingleOrDefaultAsync(); 
        return um;
    }

    public async Task<UserMedia> GetUserMediaByProfileIdAndTmdbId(int profileId, int tmdbId) 
    {
        var um =  await GetUserMediaByProfileIdAndTmdbIdOptional(profileId,tmdbId);
        return um ?? throw new NotFoundException($"The media with tmdbId:{tmdbId} for profile {profileId} not found");
    }


    public async Task RemoveAllUserMedia(int profileId)
    {
       await GetUserMedia<UserMedia>(profileId).ExecuteDeleteAsync();
    }

    public async Task<UserMovie> CreateUserMovie(Profile profile, Movie movie)
    {
        UserMovie um = new(profile, movie);
        _context.UserMedia.Add(um);
        await _context.SaveChangesAsync();
        return um;
    }

    public async Task<UserSeries> CreateUserSeries(Profile profile, Series series)
    {
        UserSeries us = new(profile, series);
        _context.UserMedia.Add(us);
        await _context.SaveChangesAsync();
        return us;
    }

    public async Task<UserSeason> CreateUserSeason(Profile profile, Season season)
    {
        UserSeason us = new(profile, season);
        _context.UserMedia.Add(us);
        await _context.SaveChangesAsync();
        return us;
    }

    public async Task<UserMedia> RateUserMedia(int profileId, Guid userMediaId ,int? rating)
    {
        var um = await GetUserMediaByIdAndProfileId(profileId,userMediaId);
        um.Rating = rating;
        await _context.SaveChangesAsync();
        return um;
    }

    public async Task<UserMedia> MarkAsWatched(int profileId, Guid userMediaId)
    {
        var um = await GetUserMediaByIdAndProfileId(profileId,userMediaId);
        um.Watch();
        await _context.SaveChangesAsync();
        return um;
    }

    public async Task<UserMedia> MarkAsSaved(int profileId, Guid userMediaId)
    {
        var um = await GetUserMediaByIdAndProfileId(profileId,userMediaId);
        um.Saved = !um.Saved;
        await _context.SaveChangesAsync();
        return um;
    }


    /// <summary>
    /// Retrieves all watched media ordered by watch date.
    /// </summary>
    public Task<List<T>> GetHistory<T>(int profileId) where T : UserMedia
    {
        return GetUserMedia<T>(profileId)
        .Where( x=>x.Watched)
        .OrderBy(x=>x.WatchedAt)
        .ToListAsync();
    }

    /// <summary>
    /// Retrieves all saved media ordered by Id.
    /// </summary>
    public Task<List<T>> GetSaved<T>(int profileId) where T : UserMedia
    {
        return GetUserMedia<T>(profileId)
        .OrderBy(x=>x.Id)
        .ToListAsync();
    }

    public Task<int> GetTotalMediaWatchedCount<T>(int profileId) where T: UserMedia 
    {
        return GetUserMedia<T>(profileId).Where(x=>x.Watched).CountAsync();
    }

    public int GetTotalMovieWatchTime(int profileId)
    {
        return GetUserMedia<UserMovie>(profileId).Where(x=>x.Watched)
            .Sum(x => x.Movie.Length);
    }

    public int GetTotalSeriesWatchTime(int profileId)
    {
        return GetUserMedia<UserSeries>(profileId)
            .Where(x=> x.Watched)
            .SelectMany(x => x.Series.Seasons)
            .Sum(season => season.EpisodeLength * season.Episodes);
    }

    private IQueryable<T> GetUserMedia<T>(int profileId) where T: UserMedia
    {
        return _context.UserMedia.OfType<T>().Where(x=>x.ProfileId == profileId);
    }
}
