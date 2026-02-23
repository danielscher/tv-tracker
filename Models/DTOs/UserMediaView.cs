using TvTracker.Models.Enums;

namespace TvTracker.Models.DTOs;

/// <summary>
/// A flattened versioned of UserMedia used to display a mix of UserMedia derived classes in a single list.
/// </summary>
public class MediaView(Guid mediaId, MediaType type, string title, string? poster)
{
    public Guid MediaId = mediaId;
    public MediaType MediaType = type;
    public string MediaTitle = title;
    public string? MediaPosterPath = poster;
}