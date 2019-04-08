namespace omniflix.Common.Parsers.GoogleDrive
{
    public class QualityMapper
    {
        public string MapQuality(int? quality)
        {
            if (quality == null) { return "medium"; }
            else if (quality == 360) { return "medium"; }
            else if (quality == 480) { return "large"; }
            else if (quality == 720) { return "hd720"; }
            else if (quality == 1080) { return "hd1080"; }
            else { return "meduim"; }
        }
    }
}