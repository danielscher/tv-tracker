namespace TvTracker.Utils;

public static class CookieUtils
{
    public static int ExtractProfileIdFromCookie(HttpRequest request)
    {
        var id = request.Cookies["SelectedProfileId"];

        if (string.IsNullOrEmpty(id))
            throw new UnauthorizedAccessException("No profile selected.");

        return int.Parse(id);
    }
}
