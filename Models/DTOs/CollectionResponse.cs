using System.Text.Json.Serialization;

namespace TvTracker.Models.DTOs;
public record CollectionResponse(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("parts")] ICollection<CollectionEntry> Entries
);

public record CollectionEntry(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("original_name")] string Title,
    [property: JsonPropertyName("poster_path")] string? PosterPath,
    [property: JsonPropertyName("release_date")] string? ReleaseDate
);