using System.Text.Json.Serialization;

namespace TvTracker.Models.DTOs;
public record MovieDetailsResponse(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("original_title")] string Title,
    [property: JsonPropertyName("poster_path")] string PosterPath,
    [property: JsonPropertyName("runtime")] int Runtime,
    [property: JsonPropertyName("release_date")] string ReleaseDate,
    [property: JsonPropertyName("genres")] List<GenreReponse> Genres,
    [property: JsonPropertyName("original_language")] string Language,
    [property: JsonPropertyName("credits")] CreditsResponse Credits
);

public record CreditsResponse (
    [property: JsonPropertyName("cast")] List<CastMemberResponse> Cast
);