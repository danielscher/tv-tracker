public static class TimeFormatting
{
    public static string FormatMinutes(int totalMinutes)
    {
        var t = TimeSpan.FromMinutes(totalMinutes);
        if (t.Days > 0) return $"{t.Days}d {t.Hours}h {t.Minutes}m";
        if (t.Hours > 0) return $"{t.Hours}h {t.Minutes}m";
        return $"{t.Minutes}m";
    }
}