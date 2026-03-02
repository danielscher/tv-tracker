namespace  TvTracker.Models.View;

public class MediaCardsSectionView(string sectionName, ICollection<MediaView> views)
{
    public string SectionName = sectionName;
    public ICollection<MediaView> MediaViews = views;
}