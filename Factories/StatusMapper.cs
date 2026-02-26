using TvTracker.Models.Enums;

public static class StatusMapper
{
    private static readonly Dictionary<string, AirStatus> _tmdbToDomain =
        new()
        {
            {"Returning Series", AirStatus.Running },
            {"In Production", AirStatus.Running},
            {"Pilot",AirStatus.Running},
            {"Planned", AirStatus.Planned},
            {"Ended", AirStatus.Finished},
            {"Canceled",AirStatus.Cancelled}
        };

    public static AirStatus ToDomain(string tmdbStatus)
    {
        return _tmdbToDomain.TryGetValue(tmdbStatus, out var status)
            ? status
            : throw new NotSupportedException($"Status {tmdbStatus} is not supported.");
    }
}