using TvTracker.Models.Enums;

public static class GenreMapper
{
    private static readonly Dictionary<int, Genre> _tmdbToDomain =
        new()
        {
            { 28, Genre.Action },
            { 18, Genre.Drama },
            { 35, Genre.Comedy },
            { 53, Genre.Thriller }
        };

    public static Genre? ToDomain(int tmdbGenreId)
    {
        return _tmdbToDomain.TryGetValue(tmdbGenreId, out var genre)
            ? genre
            : null;
    }
}