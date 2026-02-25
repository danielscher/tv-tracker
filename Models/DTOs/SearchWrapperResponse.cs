using System.Text.Json.Serialization;

namespace TvTracker.Models.DTOs;

/// <summary>
/// </summary>
public record SearchWrapperResponse<T>(
    [property: JsonPropertyName("results")] ICollection<T> Results,
    [property: JsonPropertyName("page")] int PageNumber
);



