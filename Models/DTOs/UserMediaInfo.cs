using TvTracker.Models.Enums;

namespace TvTracker.Models.DTOs;
public class UserMediaInfo(Guid id, int? rating, WatchStatus status, DateTime? watchDate)
{
    public Guid UserMediaId {get;} = id;
    public int? Rating {get;} = rating;
    public WatchStatus Status {get;} = status;
    public DateTime? WatchDate {get;} = watchDate;
}