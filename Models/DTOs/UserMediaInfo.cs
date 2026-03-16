using TvTracker.Models.Enums;

namespace TvTracker.Models.DTOs;
public class UserMediaInfo(Guid id, int? rating, bool watched, bool saved, DateTime? watchDate)
{
    public Guid UserMediaId {get;} = id;
    public int? Rating {get;} = rating;
    public bool Watched {get;} = watched;
    public bool Saved {get;} = saved;
    public DateTime? WatchDate {get;} = watchDate;
}