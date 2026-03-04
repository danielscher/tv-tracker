namespace  TvTracker.Models.View;

public class MediaCardsSectionView(string sectionName, string redirectUrl, ICollection<MediaView> views)
{
    public string SectionName = sectionName;
    public string RedirectUrl = redirectUrl;
    public ICollection<MediaView> MediaViews = views;
}