using System.Text.Json.Serialization;

public record GenreReponse(
[property: JsonPropertyName("id")] int Id,
[property: JsonPropertyName("name")] string GenreName
);