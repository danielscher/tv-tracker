using System.Text.Json.Serialization;

namespace TvTracker.Models.DTOs;
public record MovieDetailsResponse(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("original_title")] string Title,
    [property: JsonPropertyName("overview")] string Description,
    [property: JsonPropertyName("poster_path")] string PosterPath,
    [property: JsonPropertyName("runtime")] int Runtime,
    [property: JsonPropertyName("release_date")] string ReleaseDate,
    [property: JsonPropertyName("genres")] List<GenreReponse> Genres,
    [property: JsonPropertyName("original_language")] string Language,
    [property: JsonPropertyName("credits")] CreditsResponse Credits,
    [property: JsonPropertyName("belongs_to_collection")] CollectionInfoResponse CollectionInfo
);

public record CollectionInfoResponse(
    [property: JsonPropertyName("id")] int Id
);
