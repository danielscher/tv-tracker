namespace TvTracker.Utils;

public static class CookieUtils
{
    public static int? TryExtractProfileId(HttpRequest request)
    {
        var id = request.Cookies["SelectedProfileId"];

        if (string.IsNullOrEmpty(id))
            return null;

        return int.Parse(id);
    }

    public static int GetProfileId(HttpRequest request)
    {
        var id = TryExtractProfileId(request) ?? throw new UnauthorizedAccessException("No profile selected.");
        return id;
    }
}
