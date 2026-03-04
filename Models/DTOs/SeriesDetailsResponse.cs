using System.Text.Json.Serialization;

namespace TvTracker.Models.DTOs;
public record SeriesDetailsResponse(
    [property: JsonPropertyName("id")] int Id,

    [property: JsonPropertyName("original_name")] string Title,
    [property: JsonPropertyName("overview")] string Description,
    [property: JsonPropertyName("poster_path")] string PosterPath,
    [property: JsonPropertyName("first_air_date")] string ReleaseDate,
    [property: JsonPropertyName("genres")] List<GenreReponse> Genres,
    [property: JsonPropertyName("original_language")] string Language,
    [property: JsonPropertyName("number_of_seasons")] int NumberOfSeasons,
    [property: JsonPropertyName("seasons")] ICollection<SeasonResponse> Seasons,
    [property: JsonPropertyName("status")] string Status,

    // with append_to_response
    [property: JsonPropertyName("credits")] CreditsResponse Credits
);
