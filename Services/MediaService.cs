using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using TvTracker.Data;
using TvTracker.Exception;
using TvTracker.Models;

namespace TvTracker.Services;

public class MediaService<T> where T: Media
{
    private readonly TvTrackerContext _context;

    private readonly DbSet<T> _dbSet;

    public MediaService(TvTrackerContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetMedia(Guid mediaId)
    {
        return _dbSet.Find(mediaId) ?? throw new NotFoundException($"Media with id {mediaId} not found.");
    }

    public async Task<List<T>> SearchMedia(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
        return [];
        }

        return await _dbSet
        .Where(m => m.MediaInfo.Title.ToLower().Contains(query.ToLower()))
        .OrderBy(m => m.MediaInfo.Title)
        .ToListAsync();
    }

    public async Task<List<CastMember>> Experimental(Guid movieId)
    {
            var cast = await _context.Cast.Where(x=> x.MediaId == movieId).ToListAsync();
            Console.WriteLine(cast.Count);
            return cast ?? throw new NotFoundException("cast was not found.");
    }

}