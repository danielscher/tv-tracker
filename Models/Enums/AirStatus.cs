namespace TvTracker.Models.Enums;

/// <summary>
/// Represents media's release status.
/// </summary>
public enum AirStatus
{
    /// <summary>
    /// Future media parts (seasons/episodes) are still being released.
    /// </summary>
    Running,

    /// <summary>
    /// Media released in the fullest and there are no planned future releases.
    /// </summary>
    Finished,

    /// <summary>
    /// Media is incomplete and there is no planned future releases.
    /// </summary>
    Cancelled,

    /// <summary>
    /// Planned to be released and there are not current releases.
    /// </summary>
    Planned,
}